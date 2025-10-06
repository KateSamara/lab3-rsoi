using LibrarySystem.Domain.Models;

namespace LibrarySystem.Web.Dto.Converters;

public static class LibraryBookDtoConverter
{
    public static LibraryBookDto ToDto(this LibraryBook libraryBook)
    {
        return new LibraryBookDto(availableCount: libraryBook.AvailableCount,
            library: libraryBook.Library.ToDto(),
            book: libraryBook.Book.ToDto());
    }
}