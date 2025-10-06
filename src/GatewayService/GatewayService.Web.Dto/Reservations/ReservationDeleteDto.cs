using System.Text.Json.Serialization;

namespace GatewayService.Web.Dto.Reservations;

public class ReservationDeleteDto
{
    [JsonRequired]
    [JsonPropertyName("condition")]
    public string Condition { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }

    public ReservationDeleteDto(string condition, DateOnly date)
    {
        Condition = condition;
        Date = date;
    }
}