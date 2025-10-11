using GatewayService.Domain.Models.Books;
using GatewayService.Domain.Models.Libraries;

namespace GatewayService.Domain.Interfaces.Services;

public interface ILibraryService
{
    public Task<LibraryPaged> GetLibrariesByCityPagedAsync(int page, int size, string city); 
    
    public Task<BookPaged> GetBooksPagedByLibraryUuid(Guid libraryUid, int page, int size, bool showAll);
}