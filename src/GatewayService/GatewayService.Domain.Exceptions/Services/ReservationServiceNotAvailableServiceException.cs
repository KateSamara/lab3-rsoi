namespace GatewayService.Domain.Exceptions.Services;

public class ReservationServiceNotAvailableServiceException : Exception
{
    public ReservationServiceNotAvailableServiceException(string message) : base(message) { }
    
    public ReservationServiceNotAvailableServiceException(string message, Exception innerException) : base(message, innerException) { }
}