using System.Text.Json.Serialization;

namespace ReservationSystem.Web.Dto;

public class ReservationCountDto
{
    [JsonRequired]
    [JsonPropertyName("count")]
    public int Count { get; set; }

    public ReservationCountDto(int count)
    {
        Count = count;
    }
}