using System.Text.Json.Serialization;
using GatewayService.Web.Dto.Books;

namespace GatewayService.Web.Dto.Libraries;

public class LibraryBookDto
{
    [JsonRequired]
    [JsonPropertyName("availableCount")]
    public int AvailableCount { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("library")]
    public LibraryDto Library { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("book")]
    public BookDto Book { get; set; }

    public LibraryBookDto(int availableCount, LibraryDto library, BookDto book)
    {
        AvailableCount = availableCount;
        Library = library;
        Book = book;
    }
}