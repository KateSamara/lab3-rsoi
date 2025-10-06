using System.Text.Json.Serialization;

namespace RatingSystem.Web.Dto;

public class RatingDto
{
    [JsonRequired]
    [JsonPropertyName("stars")]
    public int Stars { get; set; }

    public RatingDto(int stars)
    {
        Stars = stars;
    }
}