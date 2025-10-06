using GatewayService.Web.Dto.Books;
using GatewayService.Web.Dto.Libraries;
using GatewayService.Web.Dto.Ratings;
using GatewayService.Web.Dto.Reservations;

namespace GatewayService.Web.Dto.Converters;

public static class ReservationDtoConverter
{
    public static ReservationFullDto ToFullDto(this ReservationDto reservationDto, 
        BookDto bookDto,
        LibraryDto libraryDto,
        RatingDto? ratingDto)
    {
        var reservationFull = new ReservationFullDto(reservationUuid: reservationDto.ReservationUuid,
            status: reservationDto.Status,
            startDate: reservationDto.StartDate,
            tillDate: reservationDto.TillDate,
            book: bookDto,
            library: libraryDto,
            rating: ratingDto);
        
        reservationFull.Book.Condition = null;
        reservationFull.Book.AvailableCount = null;
        
        return reservationFull;
    }
}