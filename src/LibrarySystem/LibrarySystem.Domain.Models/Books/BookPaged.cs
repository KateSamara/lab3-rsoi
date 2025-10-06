namespace LibrarySystem.Domain.Models.Books;

public record BookPaged
{
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalElements { get; init; }
    public required List<Book> Items { get; init; }
}