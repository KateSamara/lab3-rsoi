using System.Text.Json.Serialization;

namespace GatewayService.Web.Dto.Reservations;

public class ReservationCreateDto
{
    [JsonRequired]
    [JsonPropertyName("bookUid")]
    public Guid BookUuid { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("libraryUid")]
    public Guid LibraryUuid { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("tillDate")]
    public DateOnly TillDate { get; set; }

    public ReservationCreateDto(Guid bookUuid, Guid libraryUuid, DateOnly tillDate)
    {
        BookUuid = bookUuid;
        LibraryUuid = libraryUuid;
        TillDate = tillDate;
    }
}