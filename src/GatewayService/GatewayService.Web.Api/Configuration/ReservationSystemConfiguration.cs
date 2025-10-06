namespace GatewayService.Configuration;

public record ReservationSystemConfiguration
{
    public required string IpAddress { get; init; }
    public required string BaseUrl { get; init; }
    public required string UsernameHeader { get; init; }
}