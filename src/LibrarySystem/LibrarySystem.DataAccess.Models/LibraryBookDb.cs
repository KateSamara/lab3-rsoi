namespace LibrarySystem.DataAccess.Models;

public class LibraryBookDb
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int LibraryId { get; set; }
    public int AvailableCount { get; set; }
    
    public BookDb? Book { get; set; }
    public LibraryDb? Library { get; set; }

    public LibraryBookDb(int id, int bookId, int libraryId, int availableCount)
    {
        Id = id;
        BookId = bookId;
        LibraryId = libraryId;
        AvailableCount = availableCount;
    }

    public LibraryBookDb()
    {
    }
}