using GatewayService.Domain.Exceptions.Gateways;
using GatewayService.Domain.Exceptions.Services;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Interfaces.Services;
using GatewayService.Domain.Models.Books;
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
        catch (LibraryServiceNotAvailableGatewayException)
        {
            Console.WriteLine("Library service not available.");
            throw new LibraryServiceNotAvailableServiceException("Library service not available.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get libraries by city = {city}", e);
            throw new LibraryServiceException($"Failed to get libraries by city = {city}", e);
        }
    }

    public async Task<BookPaged> GetBooksPagedByLibraryUuid(Guid libraryUid, int page, int size, bool showAll)
    {
        try
        {
            return await _libraryGateway.GetBooksPagedByLibraryUuid(libraryUid, page, size, showAll);
        }
        catch (LibraryServiceNotAvailableGatewayException)
        {
            Console.WriteLine("Library service not available.");
            throw new LibraryServiceNotAvailableServiceException("Library service not available.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get books by library uid = {libraryUid}", e);
            throw new LibraryServiceException($"Failed to get books by library uid = {libraryUid}", e);
        }
    }
}