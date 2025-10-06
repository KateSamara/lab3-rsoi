using LibrarySystem.DataAccess.Models;

namespace LibrarySystem.Tests.UnitTests.Factories;

public static class BookDbFactory
{
    public static BookDb Create(int id = 1,
        Guid? bookUuid = null,
        string name = "Name",
        string? author = null,
        string? genre = null,
        BookConditionDb condition = BookConditionDb.EXCELLENT)
    {
        return new BookDb(id: id,
            bookUuid: bookUuid ?? Guid.NewGuid(),
            name: name,
            author: author,
            genre: genre,
            condition: condition);
    }
}