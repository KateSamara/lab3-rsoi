using System.Text.Json.Serialization;

namespace LibrarySystem.Web.Dto;

public class LibraryDto
{
    [JsonRequired]
    [JsonPropertyName("libraryUid")]
    public Guid LibraryUuid { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("city")]
    public string City { get; set; }

    public LibraryDto(Guid libraryUuid, string name, string address, string city)
    {
        LibraryUuid = libraryUuid;
        Name = name;
        Address = address;
        City = city;
    }

    public LibraryDto()
    {
    }
}