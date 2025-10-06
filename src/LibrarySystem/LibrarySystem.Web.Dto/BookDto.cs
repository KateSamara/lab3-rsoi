using System.Text.Json.Serialization;

namespace LibrarySystem.Web.Dto;

public class BookDto
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
    
    [JsonRequired]
    [JsonPropertyName("condition")]
    public string Condition { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("availableCount")]
    public int AvailableCount { get; set; }

    public BookDto(Guid bookUuid, string name, string? author, string? genre, string condition, int availableCount)
    {
        BookUuid = bookUuid;
        Name = name;
        Author = author;
        Genre = genre;
        Condition = condition;
        AvailableCount = availableCount;
    }


    public BookDto()
    {
    }
}