using LibrarySystem.Domain.Models;
using LibrarySystem.Domain.Models.Books;

namespace LibrarySystem.DataAccess.Models.Converters;

public static class BookDbConverter
{
    public static BookDb ToDb(this Book book)
    {
        return new BookDb(id: book.Id,
            bookUuid: book.BookUuid,
            name: book.Name,
            author: book.Author,
            genre: book.Genre,
            condition: book.Condition.ToDb());
    }

    public static Book ToDomain(this BookDb book, int availableCount)
    {
        return new Book
        {
            Id = book.Id,
            BookUuid = book.BookUuid,
            Name = book.Name,
            Author = book.Author,
            Genre = book.Genre,
            Condition = book.Condition.ToDomain(),
            AvailableCount = availableCount
        };
    }

    public static Book ToDomain(this BookDb bookDb)
    {
        return new Book
        {
            Id = bookDb.Id,
            BookUuid = bookDb.BookUuid,
            Name = bookDb.Name,
            Author = bookDb.Author,
            Genre = bookDb.Genre,
            Condition = bookDb.Condition.ToDomain()
        };
    }
}