namespace GatewayService.Domain.Models.Reservations;

public record ReservationShort
{
    public required Guid ReservationUuid { get; init; }
    
    public required Guid BookUuid { get; init; }
    
    public required Guid LibraryUuid { get; init; }
    
    public required string Status { get; init; }
    
    public required DateOnly StartDate { get; init; }
    
    public required DateOnly TillDate { get; init; }
}