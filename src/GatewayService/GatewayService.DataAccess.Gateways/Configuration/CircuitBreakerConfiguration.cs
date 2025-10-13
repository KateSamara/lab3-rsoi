namespace GatewayService.DataAccess.Gateways.Configuration;

public record CircuitBreakerConfiguration
{
    public required int FailureThreshold { get; init; }
    
    public required int Timeout { get; init; }
}