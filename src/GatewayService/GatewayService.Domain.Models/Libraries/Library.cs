namespace GatewayService.Domain.Models.Libraries;

public record Library
{
    public required Guid LibraryUuid { get; init; }
    
    public required string Name { get; set; }
    
    public required string Address { get; set; }
    
    public required string City { get; set; }
}