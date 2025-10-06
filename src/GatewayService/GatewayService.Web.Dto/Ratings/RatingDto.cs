using System.Text.Json.Serialization;

namespace GatewayService.Web.Dto.Ratings;

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