using GatewayService.Domain.Models.Books;
using GatewayService.Domain.Models.Libraries;

namespace GatewayService.Domain.Interfaces.Gateways;

public interface ILibraryGateway
{
    public Task<LibraryPaged> GetLibrariesByCityPagedAsync(int page, int size, string city);

    public Task<BookPaged> GetBooksPagedByLibraryUuid(Guid libraryUid, int page, int size, bool showAll);
}