using LibrarySystem.Domain.Models;

namespace LibrarySystem.DataAccess.Models.Converters;

public static class LibraryBookDbConverter
{
    public static LibraryBookDb ToDb(this LibraryBook libraryBook, int id)
    {
        return new LibraryBookDb(id: id,
            bookId: libraryBook.BookId,
            libraryId: libraryBook.LibraryId,
            availableCount: libraryBook.AvailableCount);
    }

    public static LibraryBook ToDomain(this LibraryBookDb libraryBook)
    {
        return new LibraryBook
        {
            BookId = libraryBook.BookId,
            LibraryId = libraryBook.LibraryId,
            AvailableCount = libraryBook.AvailableCount,
            Book = libraryBook.Book.ToDomain(),
            Library = libraryBook.Library.ToDomain()
        };
    }
}