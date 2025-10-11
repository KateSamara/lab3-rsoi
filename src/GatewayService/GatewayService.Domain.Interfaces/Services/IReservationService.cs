using GatewayService.Domain.Models.Reservations;

namespace GatewayService.Domain.Interfaces.Services;

public interface IReservationService
{
    public Task<Reservation?> CreateReservationAsync(string username, ReservationCreate reservationCreate);
    
    public Task<List<Reservation>> GetReservationsByUsernameAsync(string username);

    public Task<bool> DeleteReservationAsync(string username, Guid reservationId, ReservationDelete reservationDelete);
}