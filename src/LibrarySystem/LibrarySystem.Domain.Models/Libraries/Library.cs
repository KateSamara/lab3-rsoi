namespace LibrarySystem.Domain.Models.Libraries;

public record Library
{
    public required int Id { get; init; }
    public required Guid LibraryUuid { get; init; }
    public required string Name { get; init; }
    public required string City { get; init; }
    public required string Address { get; init; }
}