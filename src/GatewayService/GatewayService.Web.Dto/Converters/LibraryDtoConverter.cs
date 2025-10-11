using GatewayService.Domain.Models.Libraries;
using GatewayService.Web.Dto.Libraries;

namespace GatewayService.Web.Dto.Converters;

public static class LibraryDtoConverter
{
    public static LibraryDto ToDto(this Library library)
    {
        return new LibraryDto(libraryUuid: library.LibraryUuid,
            name: library.Name,
            address: library.Address,
            city: library.City);
    }

    public static LibraryPagedDto ToDto(this LibraryPaged libraryPaged)
    {
        return new LibraryPagedDto(page: libraryPaged.Page,
            pageSize: libraryPaged.PageSize,
            totalItems: libraryPaged.TotalItems,
            items: libraryPaged.Items.ConvertAll(l => l.ToDto()));
    }
}