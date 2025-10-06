namespace LibrarySystem.Domain.Models.Books;

public record Book
{
    public required int Id { get; init; }
    public required Guid BookUuid { get; init; }
    public required string Name { get; init; }
    public required string? Author { get; init; }
    public required string? Genre { get; init; }
    public required BookCondition Condition { get; init; }
    public int AvailableCount { get; init; }
}