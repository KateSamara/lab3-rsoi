using ReservationSystem.Domain.Models;

namespace ReservationSystem.Web.Dto.Converters;

public static class ReservationDtoConverter
{
    public static ReservationDto ToDto(this Reservation reservation)
    {
        return new ReservationDto(reservationUuid: reservation.ReservationUuid,
            status: reservation.Status.ToString(),
            startDate: new DateOnly(
                reservation.StartDate.Year,
                reservation.StartDate.Month, 
                reservation.StartDate.Day),
            tillDate: new DateOnly(
                reservation.TillDate.Year,
                reservation.TillDate.Month, 
                reservation.TillDate.Day),
            bookUuid: reservation.BookUuid,
            libraryUuid: reservation.LibraryUuid);
    }

    public static ReservationCreate ToDomain(this ReservationCreateDto reservationCreateDto, string username)
    {
        return new ReservationCreate
        {
            LibraryUuid = reservationCreateDto.LibraryUuid,
            BookUuid = reservationCreateDto.BookUuid,
            TillDate = reservationCreateDto.TillDate,
            Username = username
        };
    }
}