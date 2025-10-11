using GatewayService.Domain.Exceptions.Services;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Interfaces.Services;
using GatewayService.Domain.Models.Libraries;

namespace GatewayService.Application.Services;

public class LibraryService(ILibraryGateway libraryGateway) : ILibraryService
{
    private readonly ILibraryGateway _libraryGateway = libraryGateway ?? throw new ArgumentNullException(nameof(libraryGateway));

    public async Task<LibraryPaged> GetLibrariesByCityPagedAsync(int page, int size, string city)
    {
        try
        {
            return await _libraryGateway.GetLibrariesByCityPagedAsync(page, size, city);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get libraries by city = {city}", e);
            throw new LibraryServiceException($"Failed to get libraries by city = {city}", e);
        }
    }
}