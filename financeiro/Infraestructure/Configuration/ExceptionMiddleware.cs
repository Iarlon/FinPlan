using Financeiro.Domain.Exceptions;

namespace Financeiro.Infraestructure.Configuration;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var response = new
            {
                erro = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                error = new
                {
                    code = "internal_server_error",
                    message = ex.Message,
                    detail = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                },
                timestamp = DateTime.UtcNow
            });
        }
    }
}
