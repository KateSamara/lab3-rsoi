namespace GatewayService.Domain.Exceptions.Services;

public class LibraryServiceNotAvailableServiceException : Exception
{
    public LibraryServiceNotAvailableServiceException(string message) : base(message) { }
    
    public LibraryServiceNotAvailableServiceException(string message, Exception innerException) : base(message, innerException) { }
}