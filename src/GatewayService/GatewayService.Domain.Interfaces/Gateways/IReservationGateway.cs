using GatewayService.Domain.Models.Reservations;

namespace GatewayService.Domain.Interfaces.Gateways;

public interface IReservationGateway
{
    public Task<int> GetCurrentReservationsCountByUsernameAsync(string username);

    public Task<ReservationShort> AddReservationAsync(string username, ReservationCreate reservationCreate);
    
    public Task<List<ReservationShort>> GetReservationsByUsernameAsync(string username);

    public Task<ReservationShort?> DeleteReservationAsync(Guid reservationId, DateOnly date);
    
    public Task DeleteReservationAsync(Guid reservationId);
}