namespace GatewayService.DataAccess.Gateways.CircuitBreakers;

public enum CircuitBreakerStatus
{
    Open = 0,
    HalfOpen = 1,
    Closed = 2
}