using GatewayService.DataAccess.Gateways.Configuration;
using Microsoft.Extensions.Options;

namespace GatewayService.DataAccess.Gateways.CircuitBreakers;

public class CircuitBreaker<TResult>(IOptions<CircuitBreakerConfiguration> circuitBreakerConfiguration)  where TResult : class
{
    private int _failureCount;
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(circuitBreakerConfiguration.Value.Timeout);
    private CircuitBreakerStatus _status = CircuitBreakerStatus.Closed;
    private readonly object _lock = new();
    private readonly int _failureThreshold = circuitBreakerConfiguration.Value.FailureThreshold;

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, Func<Task<T>> fallbackAction, Func<Task<bool>> checkHealthAction)
    {
        if (_status == CircuitBreakerStatus.Open)
        {
            Console.WriteLine($"{typeof(TResult).FullName}: Circuit is OPEN - returning fallback");
            return await fallbackAction();
        }

        try
        {
            var result = await action();
            
            lock (_lock)
            {
                _status = CircuitBreakerStatus.Closed;
                _failureCount = 0;
            }
            
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{typeof(TResult).FullName}: Request failed, failure count: {_failureCount + 1}", e);

            var shouldOpenCircuit = false;
            lock (_lock)
            {
                _failureCount++;
                
                if (_failureCount > _failureThreshold || _status == CircuitBreakerStatus.HalfOpen)
                {
                    _status = CircuitBreakerStatus.Open;
                    shouldOpenCircuit = true;
                    Console.WriteLine($"{typeof(TResult).FullName}: Circuit OPENED after {_failureCount} failures");
                }
            }
            
            if (shouldOpenCircuit)
                _ = Task.Run(() => StartRecoveryCheckAsync(checkHealthAction));
            
            return await fallbackAction();
        }
    }

    private async Task StartRecoveryCheckAsync(Func<Task<bool>> checkHealthAction)
    {
        await Task.Delay(_timeout);

        if (await checkHealthAction())
        {
            lock (_lock)
            {
                _status = CircuitBreakerStatus.Closed;
                _failureCount = 0;
                Console.WriteLine($"{typeof(TResult).FullName}: Circuit CLOSED");
            }
        }
        else
        {
            lock (_lock)
            {
                _status = CircuitBreakerStatus.HalfOpen;
                _failureCount = 0;
                Console.WriteLine($"{typeof(TResult).FullName}: Circuit HALF OPEN");
            }
        }
    }
}