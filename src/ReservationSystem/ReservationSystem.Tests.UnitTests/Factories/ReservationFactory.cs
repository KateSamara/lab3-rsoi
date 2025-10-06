using ReservationSystem.Domain.Models;

namespace ReservationSystem.Tests.UnitTests.Factories;

public static class ReservationFactory
{
    public static Reservation Create(int id = 1,
        Guid? reservationUuid = null,
        string username = "Username",
        Guid? bookUuid = null,
        Guid? libraryUuid = null,
        Domain.Models.ReservationStatus status = Domain.Models.ReservationStatus.RENTED,
        DateOnly? startDate = null,
        DateOnly? tillDate = null)
    {
        return new Reservation
        {
            Id = id,
            ReservationUuid = reservationUuid ?? Guid.NewGuid(),
            Username = username,
            BookUuid = bookUuid ?? Guid.NewGuid(),
            LibraryUuid = libraryUuid ?? Guid.NewGuid(),
            Status = status,
            StartDate = startDate ?? new DateOnly(),
            TillDate = tillDate ?? new DateOnly()
        };
    }
}