namespace LibrarySystem.DataAccess.Models;

public class BookDb
{
    public int Id { get; set; }
    public Guid BookUuid { get; set; }
    public string Name { get; set; }
    public string? Author { get; set; }
    public string? Genre { get; set; }
    public BookConditionDb Condition { get; set; } = BookConditionDb.EXCELLENT;

    public List<LibraryBookDb> Libraries { get; set; } = [];

    public BookDb(int id, Guid bookUuid, string name, string? author, string? genre, BookConditionDb condition)
    {
        Id = id;
        BookUuid = bookUuid;
        Name = name;
        Author = author;
        Genre = genre;
        Condition = condition;
    }

    public BookDb()
    {
    }
}