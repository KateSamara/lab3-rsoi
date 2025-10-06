namespace ReservationStatus.Domain.Exceptions.Repositories;

public class ReservationRepositoryException : Exception
{
    public ReservationRepositoryException(string message) : base(message) { }
    
    public ReservationRepositoryException(string message, Exception innerException) : base(message, innerException) { }
}