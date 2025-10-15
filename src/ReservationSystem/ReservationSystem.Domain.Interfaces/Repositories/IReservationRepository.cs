using ReservationSystem.Domain.Models;

namespace ReservationSystem.Domain.Interfaces.Repositories;

public interface IReservationRepository
{
    public Task<List<Reservation>> GetReservationsByUsernameAsync(string username);
    
    public Task<int> GetReservationsCountByStatusAndUsernameAsync(ReservationStatus status, string username);
    
    public Task<Reservation> AddReservationAsync(ReservationCreate reservation);
    
    public Task<Reservation?> FindReservationByUuidAsync(Guid reservationUuid);
    
    public Task<Reservation> UpdateReservationStatusAsync(Guid reservationUuid, ReservationStatus status);
    
    public Task DeleteReservationAsync(Guid reservationUuid);
}