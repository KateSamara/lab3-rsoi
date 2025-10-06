using LibrarySystem.Domain.Models.Books;
using LibrarySystem.Domain.Models.Libraries;

namespace LibrarySystem.Domain.Models;

public record LibraryBook
{
    public required int BookId { get; init; }
    public required int LibraryId { get; init; }
    public required int AvailableCount { get; init; }
    
    public Library Library { get; init; }
    public Book Book { get; init; }
}