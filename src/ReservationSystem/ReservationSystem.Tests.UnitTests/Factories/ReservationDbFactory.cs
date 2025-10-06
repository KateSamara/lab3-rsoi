using ReservationSystem.DataAccess.Models;

namespace ReservationSystem.Tests.UnitTests.Factories;

public static class ReservationDbFactory
{
    public static ReservationDb Create(int id = 1,
        Guid? reservationUuid = null,
        string username = "Username",
        Guid? bookUuid = null,
        Guid? libraryUuid = null,
        ReservationStatusDb status = ReservationStatusDb.RENTED,
        DateOnly? startDate = null,
        DateOnly? tillDate = null)
    {
        return new ReservationDb(id: id,
            reservationUuid: reservationUuid ?? Guid.NewGuid(),
            username: username,
            bookUuid: bookUuid ?? Guid.NewGuid(),
            libraryUuid: libraryUuid ?? Guid.NewGuid(),
            status: status,
            startDate: startDate ?? new DateOnly(),
            tillDate: tillDate ?? new DateOnly());
    }
}