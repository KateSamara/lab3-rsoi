using System.Text.Json.Serialization;
using GatewayService.Web.Dto.Books;
using GatewayService.Web.Dto.Libraries;
using GatewayService.Web.Dto.Ratings;

namespace GatewayService.Web.Dto.Reservations;

public class ReservationDto
{
    [JsonRequired]
    [JsonPropertyName("reservationUid")]
    public Guid ReservationUuid { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("startDate")]
    public DateOnly StartDate { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("tillDate")]
    public DateOnly TillDate { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("book")]
    public BookShortDto Book { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("library")]
    public LibraryDto Library { get; set; }
    
    [JsonPropertyName("rating")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RatingDto? Rating { get; set; }

    public ReservationDto(Guid reservationUuid, string status, DateOnly startDate, DateOnly tillDate, BookShortDto book, LibraryDto library, RatingDto? rating)
    {
        ReservationUuid = reservationUuid;
        Status = status;
        StartDate = startDate;
        TillDate = tillDate;
        Book = book;
        Library = library;
        Rating = rating;
    }
}