using System.Text.Json;
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

public class LibraryGateway(IOptions<LibrarySystemConfiguration> librarySystemConfiguration) : ILibraryGateway
{
    private readonly LibrarySystemConfiguration _librarySystemConfiguration = 
        librarySystemConfiguration.Value ?? throw new ArgumentNullException(nameof(librarySystemConfiguration));
    
    public async Task<LibraryPaged> GetLibrariesByCityPagedAsync(int page, int size, string city)
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
            throw new LibraryGatewayException($"Failed to get libraries by city = {city}", e);
        }
    }

    public async Task<BookPaged> GetBooksPagedByLibraryUuid(Guid libraryUid, int page, int size, bool showAll)
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
            throw new LibraryGatewayException($"Failed to get books by library uid = {libraryUid}", e);
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
            throw new LibraryGatewayException(
                $"Failed to update books count with book uid = {bookUuid} and library uid = {libraryUuid}", e);
        }
    }

    public async Task<List<Book>> GetBooksByIdsAsync(List<Guid> bookUuids)
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
            throw new LibraryGatewayException($"Failed to get books by ids = {bookUuids}", e);
        }
    }

    public async Task<List<Library>> GetLibrariesByIdsAsync(List<Guid> libraryUuids)
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
            throw new LibraryGatewayException($"Failed to get libraries by ids = {libraryUuids}", e);
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
}