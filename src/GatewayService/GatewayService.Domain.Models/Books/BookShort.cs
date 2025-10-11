namespace GatewayService.Domain.Models.Books;

public record BookShort
{
    public required Guid BookUuid { get; init; }
    
    public required string Name { get; init; }
    
    public required string? Author { get; init; }
    
    public required string? Genre { get; init; }
}