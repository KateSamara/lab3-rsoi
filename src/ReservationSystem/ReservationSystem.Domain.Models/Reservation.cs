namespace ReservationSystem.Domain.Models;

public record Reservation
{
    public required int Id { get; init; }
    public required Guid ReservationUuid { get; init; }
    public required string Username { get; init; }
    public required Guid BookUuid { get; init; }
    public required Guid LibraryUuid { get; init; }
    public required ReservationStatus Status { get; init; }
    public required DateOnly StartDate { get; init; }
    public required DateOnly TillDate { get; init; }
}