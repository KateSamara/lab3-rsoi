using ReservationSystem.Domain.Models;

namespace ReservationSystem.Domain.Interfaces.Services;

public interface IReservationService
{
    public Task<List<Reservation>> GetReservationsByUsernameAsync(string username);
    
    public Task<int> GetReservationsCountByStatusAndUsernameAsync(ReservationStatus status, string username);

    public Task<Reservation> AddReservationAsync(ReservationCreate reservation);
    
    public Task<Reservation?> DeleteReservationAsync(Guid reservationUuid, DateOnly returnDate);
    
    public Task DeleteReservationAsync(Guid reservationUuid);
}