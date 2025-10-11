using GatewayService.DataAccess.Models.Reservations;
using GatewayService.Domain.Models.Reservations;

namespace GatewayService.DataAccess.Models.Converters;

public static class ReservationConverter
{
    public static ReservationCreateDto ToDto(this ReservationCreate reservationCreate)
    {
        return new ReservationCreateDto(bookUuid: reservationCreate.BookUuid,
            libraryUuid: reservationCreate.LibraryUuid,
            tillDate: reservationCreate.TillDate);
    }

    public static ReservationShort ToDomain(this ReservationShortDto reservationShort)
    {
        return new ReservationShort
        {
            ReservationUuid = reservationShort.ReservationUuid,
            BookUuid = reservationShort.BookUuid,
            LibraryUuid = reservationShort.LibraryUuid,
            Status = reservationShort.Status,
            StartDate = reservationShort.StartDate,
            TillDate = reservationShort.TillDate
        };
    }
}