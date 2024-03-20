using System.Net;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException notFound)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(notFound.Message);

            logger.LogWarning(notFound.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}