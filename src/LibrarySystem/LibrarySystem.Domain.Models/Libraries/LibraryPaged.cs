namespace LibrarySystem.Domain.Models.Libraries;

public record LibraryPaged
{
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalItems { get; init; }
    public required List<Library> Items { get; init; }
}