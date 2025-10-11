using GatewayService.Domain.Exceptions.Services;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Interfaces.Services;
using GatewayService.Domain.Models.Books;
using GatewayService.Domain.Models.Libraries;
using GatewayService.Domain.Models.Ratings;
using GatewayService.Domain.Models.Reservations;

namespace GatewayService.Application.Services;

public class ReservationService(IReservationGateway reservationGateway,
    IRatingGateway ratingGateway,
    ILibraryGateway libraryGateway) : IReservationService
{
    private readonly IReservationGateway _reservationGateway = reservationGateway ?? throw new ArgumentNullException(nameof(reservationGateway));
    private readonly IRatingGateway _ratingGateway = ratingGateway ?? throw new ArgumentNullException(nameof(ratingGateway));
    private readonly ILibraryGateway _libraryGateway = libraryGateway ?? throw new ArgumentNullException(nameof(libraryGateway));
    
    public async Task<Reservation?> CreateReservationAsync(string username, ReservationCreate reservationCreate)
    {
        try
        {
            var currentReservationsCount =
                await _reservationGateway.GetCurrentReservationsCountByUsernameAsync(username);

            var rating = await _ratingGateway.GetRatingsByUsernameAsync(username);

            if (currentReservationsCount >= rating.Stars)
                return null;
            
            var newReservation = await _reservationGateway.AddReservationAsync(username, reservationCreate);

            var libraryBook = await _libraryGateway.UpdateAvailableBooksCount(reservationCreate.BookUuid,
                reservationCreate.LibraryUuid, false);
            
            return CreateReservation(newReservation, libraryBook.Book, libraryBook.Library, rating);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to create reservation for {username}", e);
            throw new ReservationServiceException($"Failed to create reservation for {username}", e);
        }
    }

    public async Task<List<Reservation>> GetReservationsByUsernameAsync(string username)
    {
        try
        {
            var reservations = await _reservationGateway.GetReservationsByUsernameAsync(username);
            
            var bookUuids = reservations.Select(r => r.BookUuid).Distinct().ToList();
            var libraryUuids = reservations.Select(r => r.LibraryUuid).Distinct().ToList();
            
            var books = await _libraryGateway.GetBooksByIdsAsync(bookUuids);
            var libraries = await _libraryGateway.GetLibrariesByIdsAsync(libraryUuids);
            
            List<Reservation> reservationsFull = [];
            reservationsFull.AddRange(reservations.Select(reservation => 
                CreateReservation(reservation, books.First(b => b.BookUuid == reservation.BookUuid), 
                    libraries.First(b => b.LibraryUuid == reservation.LibraryUuid), null)));
            
            return reservationsFull;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get reservations for {username}", e);
            throw new ReservationServiceException($"Failed to get reservations for {username}", e);
        }
    }

    public async Task<bool> DeleteReservationAsync(string username, Guid reservationId, ReservationDelete reservationDelete)
    {
        try
        {
            var reservation = await _reservationGateway.DeleteReservationAsync(reservationId, reservationDelete.Date);
            if (reservation is null)
                return false;

            var libraryBook =
                await _libraryGateway.UpdateAvailableBooksCount(reservation.BookUuid, reservation.LibraryUuid, true);
            
            var starDifference = 0;
            if (reservation.Status != "EXPIRED" && libraryBook.Book.Condition == reservationDelete.Condition)
            {
                starDifference++;
            }
            else
            {
                if (reservation.Status == "EXPIRED")
                    starDifference -= 10;
                if (libraryBook.Book.Condition != reservationDelete.Condition)
                    starDifference -= 10;
            }

            await _ratingGateway.UpdateRatingAsync(username, starDifference);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to delete reservation for {username} with uid = {reservationId}", e);
            throw new ReservationServiceException($"Failed to delete reservation for {username} with uid = {reservationId}", e);
        }
    }

    private Reservation CreateReservation(ReservationShort reservation, Book book, Library library, Rating? rating)
    {
        return new Reservation
        {
            Book = new BookShort
            {
                Author = book.Author,
                Name = book.Name,
                BookUuid = book.BookUuid,
                Genre = book.Genre
            },
            Library = library,
            Rating = rating,
            ReservationUuid = reservation.ReservationUuid,
            StartDate = reservation.StartDate,
            Status = reservation.Status,
            TillDate = reservation.TillDate
        };
    }
}