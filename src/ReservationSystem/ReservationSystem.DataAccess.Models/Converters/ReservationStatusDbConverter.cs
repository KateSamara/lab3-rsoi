using ReservationSystem.Domain.Models;

namespace ReservationSystem.DataAccess.Models.Converters;

public static class ReservationStatusDbConverter
{
    public static ReservationStatus ToDomain(this ReservationStatusDb reservationStatusDb)
    {
        return reservationStatusDb switch
        {
            ReservationStatusDb.RENTED => ReservationStatus.RENTED,
            ReservationStatusDb.RETURNED => ReservationStatus.RETURNED,
            ReservationStatusDb.EXPIRED => ReservationStatus.EXPIRED,
            _ => ReservationStatus.RETURNED
        };
    }
    
    public static ReservationStatusDb ToDb(this ReservationStatus reservationStatus)
    {
        return reservationStatus switch
        {
            ReservationStatus.RENTED => ReservationStatusDb.RENTED,
            ReservationStatus.RETURNED => ReservationStatusDb.RETURNED,
            ReservationStatus.EXPIRED => ReservationStatusDb.EXPIRED,
            _ => ReservationStatusDb.RETURNED
        };
    }
}