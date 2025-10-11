namespace GatewayService.Domain.Models.Reservations;

public record ReservationDelete
{
    public required string Condition { get; init; }
    
    public required DateOnly Date { get; init; }
}