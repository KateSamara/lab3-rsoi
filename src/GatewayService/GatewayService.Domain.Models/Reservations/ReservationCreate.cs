namespace GatewayService.Domain.Models.Reservations;

public record ReservationCreate
{
    public required Guid BookUuid { get; init; }
    
    public required Guid LibraryUuid { get; init; }
    
    public required DateOnly TillDate { get; init; }
}