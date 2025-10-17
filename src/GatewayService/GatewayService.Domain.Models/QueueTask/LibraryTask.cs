using GatewayService.Domain.Models.Reservations;

namespace GatewayService.Domain.Models.QueueTask;

public record LibraryTask
{
    public required string Username { get; init; }
    public required ReservationShort Reservation { get; init; }
    public required ReservationDelete ReservationDelete { get; init; }
}