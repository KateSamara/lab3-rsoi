namespace ReservationStatus.Domain.Exceptions.Services;

public class ReservationServiceException : Exception
{
    public ReservationServiceException(string message) : base(message) { }
    
    public ReservationServiceException(string message, Exception innerException) : base(message, innerException) { }
}