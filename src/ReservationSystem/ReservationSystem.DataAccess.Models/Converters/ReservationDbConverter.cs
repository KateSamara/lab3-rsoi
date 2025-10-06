using ReservationSystem.Domain.Models;

namespace ReservationSystem.DataAccess.Models.Converters;

public static class ReservationDbConverter
{
    public static Reservation ToDomain(this ReservationDb reservationDb)
    {
        return new Reservation
        {
            Id = reservationDb.Id,
            ReservationUuid = reservationDb.ReservationUuid,
            Username = reservationDb.Username,
            BookUuid = reservationDb.BookUuid,
            LibraryUuid = reservationDb.LibraryUuid,
            Status = reservationDb.Status.ToDomain(),
            StartDate = reservationDb.StartDate,
            TillDate = reservationDb.TillDate
        };
    }

    public static ReservationDb ToDb(this ReservationCreate reservation, int id)
    {
        var currentTime = DateTime.Now;
        return new ReservationDb(id: id,
            reservationUuid: Guid.NewGuid(),
            username: reservation.Username,
            libraryUuid: reservation.LibraryUuid,
            bookUuid: reservation.BookUuid,
            status: ReservationStatusDb.RENTED,
            startDate: new DateOnly(year: currentTime.Year, month: currentTime.Month, day: currentTime.Day),
            tillDate: reservation.TillDate);
    }
}