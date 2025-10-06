namespace ReservationSystem.Domain.Models;

public record ReservationCreate
{
    public required string Username { get; init; }
    public required Guid BookUuid { get; init; }
    public required Guid LibraryUuid { get; init; }
    public required DateOnly TillDate { get; init; }
}