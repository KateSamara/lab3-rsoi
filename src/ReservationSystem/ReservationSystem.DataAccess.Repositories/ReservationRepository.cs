using Microsoft.EntityFrameworkCore;
using ReservationStatus.Domain.Exceptions.Repositories;
using ReservationSystem.DataAccess.Context;
using ReservationSystem.DataAccess.Models.Converters;
using ReservationSystem.Domain.Interfaces.Repositories;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.DataAccess.Repositories;

public class ReservationRepository(ReservationSystemContext reservationSystemContext) : IReservationRepository
{
    private readonly ReservationSystemContext _reservationSystemContext = reservationSystemContext ?? throw new ArgumentNullException(nameof(reservationSystemContext));

    public async Task<List<Reservation>> GetReservationsByUsernameAsync(string username)
    {
        try
        {
            var reservationsDb = await _reservationSystemContext.Reservations
                .AsNoTracking()
                .Where(r => r.Username == username)
                .ToListAsync();

            return reservationsDb.ConvertAll(r => r.ToDomain());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ReservationRepositoryException($"Error while getting reservations by username = {username}.", e);
        }
    }

    public async Task<int> GetReservationsCountByStatusAndUsernameAsync(Domain.Models.ReservationStatus status, string username)
    {
        try
        {
            var statusDb = status.ToDb();
            
            return await _reservationSystemContext.Reservations
                .AsNoTracking()
                .Where(r => r.Status == statusDb && r.Username == username)
                .CountAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ReservationRepositoryException($"Error while getting reservations by status = {status.ToString()} and username = {username}.", e);
        }
    }

    public async Task<Reservation> AddReservationAsync(ReservationCreate reservation)
    {
        try
        {
            int id;
            if (await _reservationSystemContext.Reservations.CountAsync() == 0)
                id = 1;
            else
                id = await _reservationSystemContext.Reservations.MaxAsync(r => r.Id) + 1;

            var reservationDb = reservation.ToDb(id);
            
            _reservationSystemContext.Reservations.Add(reservationDb);
            await _reservationSystemContext.SaveChangesAsync();

            return reservationDb.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ReservationRepositoryException($"Error while adding reservation {reservation}.", e);
        }
    }

    public async Task<Reservation?> FindReservationByUuidAsync(Guid reservationUuid)
    {
        try
        {
            var reservationDb = await _reservationSystemContext.Reservations
                .AsNoTracking()
                .Where(r => r.ReservationUuid == reservationUuid)
                .FirstOrDefaultAsync();
            
            return reservationDb?.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ReservationRepositoryException($"Error while finding reservation by uid = {reservationUuid}.", e);
        }
    }

    public async Task<Reservation> UpdateReservationStatusAsync(Guid reservationUuid, Domain.Models.ReservationStatus status)
    {
        try
        {
            var reservationDb = await _reservationSystemContext.Reservations
                .Where(r => r.ReservationUuid == reservationUuid)
                .FirstAsync();
            reservationDb.Status = status.ToDb();
            
            await _reservationSystemContext.SaveChangesAsync();

            return reservationDb.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ReservationRepositoryException($"Error while updating reservation by uid {reservationUuid}.", e);
        }
    }
}