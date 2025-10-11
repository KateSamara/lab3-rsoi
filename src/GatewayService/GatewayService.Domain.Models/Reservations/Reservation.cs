using GatewayService.Domain.Models.Books;
using GatewayService.Domain.Models.Libraries;
using GatewayService.Domain.Models.Ratings;

namespace GatewayService.Domain.Models.Reservations;

public record Reservation
{
    public required Guid ReservationUuid { get; init; }
    
    public required string Status { get; init; }
    
    public required DateOnly StartDate { get; init; }
    
    public required DateOnly TillDate { get; init; }
    
    public required BookShort Book { get; init; }
    
    public required Library Library { get; init; }
    
    public required Rating? Rating { get; init; }
}