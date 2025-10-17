namespace GatewayService.Domain.Exceptions.Gateways;

public class ReservationServiceNotAvailableGatewayException : Exception
{
    public ReservationServiceNotAvailableGatewayException(string message) : base(message) { }
    
    public ReservationServiceNotAvailableGatewayException(string message, Exception innerException) : base(message, innerException) { }
}