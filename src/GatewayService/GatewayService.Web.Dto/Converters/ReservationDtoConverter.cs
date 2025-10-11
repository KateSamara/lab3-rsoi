using GatewayService.Domain.Models.Reservations;
using GatewayService.Web.Dto.Reservations;

namespace GatewayService.Web.Dto.Converters;

public static class ReservationDtoConverter
{
    public static ReservationDto ToDto(this Reservation reservation)
    {
       return new ReservationDto(reservationUuid: reservation.ReservationUuid,
            status: reservation.Status,
            startDate: reservation.StartDate,
            tillDate: reservation.TillDate,
            book: reservation.Book.ToDto(),
            library: reservation.Library.ToDto(),
            rating: reservation.Rating?.ToDto());
    }

    public static ReservationCreate ToDomain(this ReservationCreateDto reservationCreateDto)
    {
        return new ReservationCreate
        {
            BookUuid = reservationCreateDto.BookUuid,
            LibraryUuid = reservationCreateDto.LibraryUuid,
            TillDate = reservationCreateDto.TillDate
        };
    }

    public static ReservationDelete ToDomain(this ReservationDeleteDto reservationDeleteDto)
    {
        return new ReservationDelete
        {
            Condition = reservationDeleteDto.Condition,
            Date = reservationDeleteDto.Date
        };
    }
}