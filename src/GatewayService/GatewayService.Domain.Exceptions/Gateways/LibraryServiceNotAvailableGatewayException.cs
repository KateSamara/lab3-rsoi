namespace GatewayService.Domain.Exceptions.Gateways;

public class LibraryServiceNotAvailableGatewayException : Exception
{
    public LibraryServiceNotAvailableGatewayException(string message) : base(message) { }
    
    public LibraryServiceNotAvailableGatewayException(string message, Exception innerException) : base(message, innerException) { }
}