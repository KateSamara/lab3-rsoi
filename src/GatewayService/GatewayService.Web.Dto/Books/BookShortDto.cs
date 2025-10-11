using System.Text.Json.Serialization;

namespace GatewayService.Web.Dto.Books;

public class BookShortDto
{
    [JsonRequired]
    [JsonPropertyName("bookUid")]
    public Guid BookUuid { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("author")]
    public string? Author { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("genre")]
    public string? Genre { get; set; }

    public BookShortDto(Guid bookUuid, string name, string? author, string? genre)
    {
        BookUuid = bookUuid;
        Name = name;
        Author = author;
        Genre = genre;
    }
}