namespace ReservationSystem.DataAccess.Models;

public class ReservationDb
{
    public int Id { get; set; }
    public Guid ReservationUuid { get; set; } 
    public string Username { get; set; }
    public Guid BookUuid { get; set; }
    public Guid LibraryUuid { get; set; }
    public ReservationStatusDb Status { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly TillDate { get; set; }

    public ReservationDb(int id, Guid reservationUuid, string username, Guid bookUuid, Guid libraryUuid, ReservationStatusDb status, DateOnly startDate, DateOnly tillDate)
    {
        Id = id;
        ReservationUuid = reservationUuid;
        Username = username;
        BookUuid = bookUuid;
        LibraryUuid = libraryUuid;
        Status = status;
        StartDate = startDate;
        TillDate = tillDate;
    }
}