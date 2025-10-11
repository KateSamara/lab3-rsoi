namespace GatewayService.Domain.Models.Books;

public record Book
{
    public required Guid BookUuid { get; init; }
    
    public required string Name { get; init; }
    
    public required string? Author { get; init; }
    
    public required string? Genre { get; init; }
    
    public required string Condition { get; init; }
    
    public required int AvailableCount { get; init; }
}