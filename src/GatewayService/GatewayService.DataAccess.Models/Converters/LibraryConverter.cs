using GatewayService.DataAccess.Models.Libraries;
using GatewayService.Domain.Models.Libraries;

namespace GatewayService.DataAccess.Models.Converters;

public static class LibraryConverter
{
    public static Library ToDomain(this LibraryDto libraryDto)
    {
        return new Library
        {
            Name = libraryDto.Name,
            LibraryUuid = libraryDto.LibraryUuid,
            Address = libraryDto.Address,
            City = libraryDto.City
        };
    }

    public static LibraryPaged ToDomain(this LibraryPagedDto libraryPagedDto)
    {
        return new LibraryPaged
        {
            PageSize = libraryPagedDto.PageSize,
            Page = libraryPagedDto.Page,
            TotalItems = libraryPagedDto.TotalItems,
            Items = libraryPagedDto.Items.ConvertAll(l => l.ToDomain())
        };
    }

    public static LibraryBook ToDomain(this LibraryBookDto libraryBookDto)
    {
        return new LibraryBook
        {
            AvailableCount = libraryBookDto.AvailableCount,
            Book = libraryBookDto.Book.ToDomain(),
            Library = libraryBookDto.Library.ToDomain()
        };
    }
}