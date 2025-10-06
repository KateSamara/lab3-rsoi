using LibrarySystem.Domain.Models;
using LibrarySystem.Domain.Models.Libraries;

namespace LibrarySystem.DataAccess.Models.Converters;

public static class LibraryDbConverter
{
    public static LibraryDb ToDb(this Library library)
    {
        return new LibraryDb(id: library.Id,
            libraryUuid: library.LibraryUuid,
            name: library.Name,
            city: library.City,
            address: library.Address);
    }

    public static Library ToDomain(this LibraryDb libraryDb)
    {
        return new Library
        {
            Id = libraryDb.Id,
            LibraryUuid = libraryDb.LibraryUuid,
            Name = libraryDb.Name,
            City = libraryDb.City,
            Address = libraryDb.Address
        };
    }
}