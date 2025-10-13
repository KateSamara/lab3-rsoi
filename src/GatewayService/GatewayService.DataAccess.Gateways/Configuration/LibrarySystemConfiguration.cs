namespace GatewayService.DataAccess.Gateways.Configuration;

public record LibrarySystemConfiguration
{
    public required string IpAddress { get; init; }
    public required string BaseUrl { get; init; }
    public required string GetBooksSuffix { get; init; }
    public required string BaseBookUrl { get; init; }
    public required string SearchByIdsSuffix { get; init; }
    public required string CheckHealth { get; init; }
}