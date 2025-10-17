using GatewayService.Application.Helpers.Queues;
using GatewayService.Domain.Exceptions.Gateways;
using GatewayService.Domain.Exceptions.Services;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Interfaces.Services;
using GatewayService.Domain.Models.Books;
using GatewayService.Domain.Models.Libraries;
using GatewayService.Domain.Models.QueueTask;
using GatewayService.Domain.Models.Ratings;
using GatewayService.Domain.Models.Reservations;

namespace GatewayService.Application.Services;

public class ReservationService(IReservationGateway reservationGateway,
    IRatingGateway ratingGateway,
    ILibraryGateway libraryGateway,
    TaskQueue<LibraryTask> libraryQueue,
    TaskQueue<RatingTask> ratingTask) : IReservationService
{
    private readonly IReservationGateway _reservationGateway = reservationGateway ?? throw new ArgumentNullException(nameof(reservationGateway));
    private readonly IRatingGateway _ratingGateway = ratingGateway ?? throw new ArgumentNullException(nameof(ratingGateway));
    private readonly ILibraryGateway _libraryGateway = libraryGateway ?? throw new ArgumentNullException(nameof(libraryGateway));
    private readonly TaskQueue<LibraryTask> _libraryQueue = libraryQueue ?? throw new ArgumentNullException(nameof(libraryQueue));
    private readonly TaskQueue<RatingTask> _ratingTask = ratingTask ?? throw new ArgumentNullException(nameof(ratingTask));
    
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

            LibraryBook libraryBook;

            try
            {
                libraryBook = await _libraryGateway.UpdateAvailableBooksCount(reservationCreate.BookUuid,
                    reservationCreate.LibraryUuid, false);
            }
            catch (LibraryServiceNotAvailableGatewayException e)
            {
                await _reservationGateway.DeleteReservationAsync(newReservation.ReservationUuid);
                throw new LibraryServiceNotAvailableServiceException("Library Service not available.", e);
            }

            return CreateReservation(newReservation, libraryBook.Book, libraryBook.Library, rating);
        }
        catch (ReservationServiceNotAvailableGatewayException)
        {
            Console.WriteLine("Reservation service not available.");
            throw new ReservationServiceNotAvailableServiceException("Reservation service not available.");
        }
        catch (RatingServiceNotAvailableGatewayException)
        {
            Console.WriteLine("Rating service not available.");
            throw new RatingServiceNotAvailableServiceException("Rating service not available.");
        }
        catch (LibraryServiceNotAvailableServiceException)
        {
            throw;
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
        catch (ReservationServiceNotAvailableGatewayException)
        {
            Console.WriteLine("Reservation service not available.");
            throw new ReservationServiceNotAvailableServiceException("Reservation service not available.");
        }
        catch (LibraryServiceNotAvailableGatewayException)
        {
            Console.WriteLine("Library service not available.");
            throw new LibraryServiceNotAvailableServiceException("Library service not available.");
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

            LibraryBook libraryBook;

            try
            {
                libraryBook =
                    await _libraryGateway.UpdateAvailableBooksCount(reservation.BookUuid, reservation.LibraryUuid,
                        true);
            }
            catch (LibraryServiceNotAvailableGatewayException)
            {
                await _libraryQueue.EnqueueAsync(new LibraryTask
                {
                    Username = username,
                    Reservation = reservation,
                    ReservationDelete = reservationDelete
                });
                Console.WriteLine("Failed to connect to library service");
                return true;
            }

            try
            {
                await UpdateRatingAsync(username, reservation.Status, libraryBook.Book.Condition, reservationDelete.Condition);
            }
            catch (RatingServiceNotAvailableServiceException)
            {
                await _ratingTask.EnqueueAsync(new RatingTask
                {
                    Username = username,
                    Status = reservation.Status,
                    OldCondition = libraryBook.Book.Condition,
                    NewCondition = reservationDelete.Condition
                });
                Console.WriteLine("Failed to connect to rating service");
            }
            
            return true;
        }
        catch (ReservationServiceNotAvailableGatewayException)
        {
            Console.WriteLine("Reservation service not available.");
            throw new ReservationServiceNotAvailableServiceException("Reservation service not available.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to delete reservation for {username} with uid = {reservationId}", e);
            throw new ReservationServiceException($"Failed to delete reservation for {username} with uid = {reservationId}", e);
        }
    }

    private async Task UpdateRatingAsync(string username, string status, string oldCondition, string newCondition)
    {
        try
        {
            var starDifference = 0;
            if (status != "EXPIRED" && oldCondition == newCondition)
            {
                starDifference++;
            }
            else
            {
                if (status == "EXPIRED")
                    starDifference -= 10;
                if (oldCondition != newCondition)
                    starDifference -= 10;
            }

            await _ratingGateway.UpdateRatingAsync(username, starDifference);
        }
        catch (RatingServiceNotAvailableGatewayException e)
        {
            Console.WriteLine("Rating service not available.");
            throw new RatingServiceNotAvailableServiceException("Rating service not available.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to update rating for {username}", e);
            throw new RatingServiceException($"Failed to update rating for {username}", e);
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