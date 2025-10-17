using GatewayService.Domain.Exceptions.Services;

namespace GatewayService.Web.Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (LibraryServiceNotAvailableServiceException)
        {
            Console.WriteLine("Library Service unavailable");
            context.Response.StatusCode = 503;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { message = "Library Service unavailable" });
        }
        catch (ReservationServiceNotAvailableServiceException)
        {
            Console.WriteLine("Reservation Service unavailable");
            context.Response.StatusCode = 503;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { message = "Reservation Service unavailable" });
        }
        catch (RatingServiceNotAvailableServiceException)
        {
            Console.WriteLine("Bonus Service unavailable");
            context.Response.StatusCode = 503;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { message = "Bonus Service unavailable" });
        }
        catch (Exception e)
        {
            Console.WriteLine("Unhandled Exception", e);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { message = "Unhandled Exception" });
        }
    }
}