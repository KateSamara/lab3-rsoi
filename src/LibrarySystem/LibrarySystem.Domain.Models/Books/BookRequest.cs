namespace LibrarySystem.Domain.Models.Books;

public record BookRequest
{
    public required int Page { get; init; }
    public required int Size { get; init; }
    public required bool ShowAll { get; init; }
    public required Guid LibraryUuid { get; init; }
}