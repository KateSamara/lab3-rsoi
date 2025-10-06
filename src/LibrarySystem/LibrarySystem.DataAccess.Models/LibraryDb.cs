namespace LibrarySystem.DataAccess.Models;

public class LibraryDb
{
    public int Id { get; set; }
    public Guid LibraryUuid { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }

    public List<LibraryBookDb> Books { get; set; } = [];

    public LibraryDb(int id, Guid libraryUuid, string name, string city, string address)
    {
        Id = id;
        LibraryUuid = libraryUuid;
        Name = name;
        City = city;
        Address = address;
    }

    public LibraryDb()
    {
    }
}