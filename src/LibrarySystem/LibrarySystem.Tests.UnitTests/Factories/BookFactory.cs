using LibrarySystem.Domain.Models.Books;

namespace LibrarySystem.Tests.UnitTests.Factories;

public static class BookFactory
{
    public static Book Create(int id = 1,
        Guid? bookUuid = null,
        string name = "Name",
        string? author = null,
        string? genre = null,
        BookCondition condition = BookCondition.EXCELLENT,
        int availableCount = 0)
    {
        return new Book
        {
            Id = id,
            BookUuid = bookUuid ?? Guid.NewGuid(),
            Name = name,
            Author = author,
            Genre = genre,
            Condition = condition,
            AvailableCount = availableCount
        };
    }
}