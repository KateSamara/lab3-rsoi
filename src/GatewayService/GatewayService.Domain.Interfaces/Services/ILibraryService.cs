using GatewayService.Domain.Models.Libraries;

namespace GatewayService.Domain.Interfaces.Services;

public interface ILibraryService
{
    public Task<LibraryPaged> GetLibrariesByCityPagedAsync(int page, int size, string city); 
}