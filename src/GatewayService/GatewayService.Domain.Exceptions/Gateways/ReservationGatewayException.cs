namespace GatewayService.Domain.Exceptions.Gateways;

public class ReservationGatewayException : Exception
{
    public ReservationGatewayException(string message) : base(message) { }
    
    public ReservationGatewayException(string message, Exception innerException) : base(message, innerException) { }
}