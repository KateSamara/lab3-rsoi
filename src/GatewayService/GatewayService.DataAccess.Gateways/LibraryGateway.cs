using System.Text.Json;
using GatewayService.DataAccess.Gateways.CircuitBreakers;
using GatewayService.DataAccess.Gateways.Configuration;
using GatewayService.DataAccess.Models;
using GatewayService.DataAccess.Models.Books;
using GatewayService.DataAccess.Models.Converters;
using GatewayService.DataAccess.Models.Libraries;
using GatewayService.Domain.Exceptions.Gateways;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Models.Books;
using GatewayService.Domain.Models.Libraries;
using Microsoft.Extensions.Options;

namespace GatewayService.DataAccess.Gateways;

public class LibraryGateway(IOptions<LibrarySystemConfiguration> librarySystemConfiguration,
    CircuitBreaker<LibraryGateway> libraryCircuitBreaker) : ILibraryGateway
{
    private readonly LibrarySystemConfiguration _librarySystemConfiguration = 
        librarySystemConfiguration.Value ?? throw new ArgumentNullException(nameof(librarySystemConfiguration));
    private readonly CircuitBreaker<LibraryGateway> _libraryCircuitBreaker = libraryCircuitBreaker ?? throw new ArgumentNullException(nameof(libraryCircuitBreaker));
    
    public async Task<LibraryPaged> GetLibrariesByCityPagedAsync(int page, int size, string city)
    {
        return await _libraryCircuitBreaker.ExecuteAsync(
            action: async () => await CallGetLibrariesByCityPagedAsync(page, size, city),
            fallbackAction: () =>
            {
                Console.WriteLine("Library service is unavailable.");
                throw new LibraryServiceNotAvailableGatewayException("Library service is unavailable.");
            },
            checkHealthAction: async () => await IsLibraryServiceAvailableAsync()
        );
    }
    
    private async Task<LibraryPaged> CallGetLibrariesByCityPagedAsync(int page, int size, string city)
    {
        try
        {
            using var client = new HttpClient();
        
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_librarySystemConfiguration.IpAddress}/{_librarySystemConfiguration.BaseUrl}" +
                $"?page={page}&size={size}&city={city}");
        
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
        
            var libraryPaged = JsonSerializer.Deserialize<LibraryPagedDto>(json);

            return libraryPaged!.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get libraries by city = {city}", e);
            throw new LibraryServiceNotAvailableGatewayException($"Failed to get libraries by city = {city}", e);
        }
    }

    public async Task<BookPaged> GetBooksPagedByLibraryUuid(Guid libraryUid, int page, int size, bool showAll)
    {
        return await _libraryCircuitBreaker.ExecuteAsync(
            action: async () => await CallGetBooksPagedByLibraryUuid(libraryUid, page, size, showAll),
            fallbackAction: () =>
            {
                Console.WriteLine("Library service is unavailable.");
                throw new LibraryServiceNotAvailableGatewayException("Library service is unavailable.");
            },
            checkHealthAction: async () => await IsLibraryServiceAvailableAsync()
        );
    }

    private async Task<BookPaged> CallGetBooksPagedByLibraryUuid(Guid libraryUid, int page, int size, bool showAll)
    {
        try
        {
            using var client = new HttpClient();
        
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_librarySystemConfiguration.IpAddress}/{_librarySystemConfiguration.BaseUrl}/" +
                $"{libraryUid}/{_librarySystemConfiguration.GetBooksSuffix}?page={page}&size={size}&showAll={showAll}");
        
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
        
            var booksPaged = JsonSerializer.Deserialize<BookPagedDto>(json);

            return booksPaged!.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get books by library uid = {libraryUid}", e);
            throw new LibraryServiceNotAvailableGatewayException($"Failed to get books by library uid = {libraryUid}", e);
        }
    }

    public async Task<LibraryBook> UpdateAvailableBooksCount(Guid bookUuid, Guid libraryUuid, bool isIncrease)
    {
        try
        {
            using var client = new HttpClient();
            
            using var request = new HttpRequestMessage(HttpMethod.Patch,
                $"{_librarySystemConfiguration.IpAddress}/{_librarySystemConfiguration.BaseUrl}/" +
                $"{libraryUuid}/{_librarySystemConfiguration.GetBooksSuffix}/{bookUuid}?isIncrease={isIncrease}");

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
        
            var libraryBook = JsonSerializer.Deserialize<LibraryBookDto>(json);
        
            return libraryBook!.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to update books count with book uid = {bookUuid} and library uid = {libraryUuid}", e);
            throw new LibraryServiceNotAvailableGatewayException($"Failed to update books count with book uid = {bookUuid} and library uid = {libraryUuid}", e);
        }
    }

    public async Task<List<Book>> GetBooksByIdsAsync(List<Guid> bookUuids)
    {
        return await _libraryCircuitBreaker.ExecuteAsync(
            action: async () => await CallGetBooksByIdsAsync(bookUuids),
            fallbackAction: () => Task.FromResult(
                bookUuids.Select(bookUuid => new Book
                {
                    BookUuid = bookUuid,
                    Name = string.Empty,
                    Author = null,
                    AvailableCount = -1,
                    Condition = string.Empty,
                    Genre = null,
                }).ToList()
            ),
            checkHealthAction: async () => await IsLibraryServiceAvailableAsync()
        );
    }

    private async Task<List<Book>> CallGetBooksByIdsAsync(List<Guid> bookUuids)
    {
        try
        {
            using var client = new HttpClient();
            
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_librarySystemConfiguration.IpAddress}/{_librarySystemConfiguration.BaseBookUrl}/{_librarySystemConfiguration.SearchByIdsSuffix}{BuildPartUrlWithIds(bookUuids)}");
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
        
            var books = JsonSerializer.Deserialize<List<BookDto>>(json);
        
            return books!.ConvertAll(b => b.ToDomain());
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get books by ids = {bookUuids}", e);
            throw new LibraryServiceNotAvailableGatewayException($"Failed to get books by ids = {bookUuids}", e);
        }
    }

    public async Task<List<Library>> GetLibrariesByIdsAsync(List<Guid> libraryUuids)
    {
        return await _libraryCircuitBreaker.ExecuteAsync(
            action: async () => await CallGetLibrariesByIdsAsync(libraryUuids),
            fallbackAction: () => Task.FromResult(
                libraryUuids.Select(libraryUuid => new Library
                {
                    LibraryUuid = libraryUuid,
                    Name = string.Empty,
                    Address = string.Empty,
                    City = string.Empty
                }).ToList()
            ),
            checkHealthAction: async () => await IsLibraryServiceAvailableAsync()
        );
    }

    private async Task<List<Library>> CallGetLibrariesByIdsAsync(List<Guid> libraryUuids)
    {
        try
        {
            using var client = new HttpClient();
            
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_librarySystemConfiguration.IpAddress}/{_librarySystemConfiguration.BaseUrl}/{_librarySystemConfiguration.SearchByIdsSuffix}{BuildPartUrlWithIds(libraryUuids)}");
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
        
            var libraries = JsonSerializer.Deserialize<List<LibraryDto>>(json);

            return libraries!.ConvertAll(l => l.ToDomain());
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get libraries by ids = {libraryUuids}", e);
            throw new LibraryServiceNotAvailableGatewayException($"Failed to get libraries by ids = {libraryUuids}", e);
        }
    }
    
    private string BuildPartUrlWithIds(List<Guid> ids)
    {
        var url = "?";
        foreach (var id in ids)
        {
            url += $"ids={id}&";
        }
        return url;
    }
    
    private async Task<bool> IsLibraryServiceAvailableAsync()
    {
        try
        {
            using var client = new HttpClient();
        
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_librarySystemConfiguration.IpAddress}/{_librarySystemConfiguration.CheckHealth}");
        
            using var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}