using ReservationStatus.Domain.Exceptions.Services;
using ReservationSystem.Domain.Interfaces.Repositories;
using ReservationSystem.Domain.Interfaces.Services;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Services;

public class ReservationService(IReservationRepository reservationRepository) : IReservationService
{
    private readonly IReservationRepository _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
    
    public async Task<List<Reservation>> GetReservationsByUsernameAsync(string username)
    {
        try
        {
            return await _reservationRepository.GetReservationsByUsernameAsync(username);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ReservationServiceException($"Error while getting reservations by username = {username}.", e);
        }
    }

    public async Task<int> GetReservationsCountByStatusAndUsernameAsync(Domain.Models.ReservationStatus status, string username)
    {
        try
        {
            return await _reservationRepository.GetReservationsCountByStatusAndUsernameAsync(status, username);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ReservationServiceException($"Error while getting reservations by status = {status.ToString()} and username = {username}.",
                e);
        }   
    }

    public async Task<Reservation> AddReservationAsync(ReservationCreate reservation)
    {
        try
        {
            return await _reservationRepository.AddReservationAsync(reservation);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ReservationServiceException($"Error while adding reservations {reservation}.", e);
        }
    }

    public async Task<Reservation?> DeleteReservationAsync(Guid reservationUuid, DateOnly returnDate)
    {
        try
        {
            var reservation = await _reservationRepository.FindReservationByUuidAsync(reservationUuid);
            if (reservation == null)
                return null;
            
            Domain.Models.ReservationStatus status;
            if (returnDate > reservation.TillDate)
                status = Domain.Models.ReservationStatus.EXPIRED;
            else
                status = Domain.Models.ReservationStatus.RETURNED;
            
            return await _reservationRepository.UpdateReservationStatusAsync(reservationUuid, status);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ReservationServiceException($"Error while unable reservation by uid = {reservationUuid}.", e);
        }
    }

    public async Task DeleteReservationAsync(Guid reservationUuid)
    {
        try
        {
            await _reservationRepository.DeleteReservationAsync(reservationUuid);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ReservationServiceException($"Error while deleting reservation by uid = {reservationUuid}.", e);
        }
    }
}