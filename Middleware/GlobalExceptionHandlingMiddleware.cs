using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace OnlineCourse.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment env)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
    private readonly IHostEnvironment _env = env;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }

        catch (Exception ex)
        {
            // Registra la excepci�n no controlada
            _logger.LogError(ex, "Ocurri� una excepci�n no controlada para la solicitud {Method}, {Path}",
                             context.Request.Method, context.Request.Path);

            // Prepara la respuesta de error
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json"; // Usar el tipo de contenido para ProblemDetails
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Crear un objeto ProblemDetails para una respuesta de error estandarizada
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error interno del servidor.",
            // Incluir m�s detalles solo en desarrollo por seguridad
            Detail = _env.IsDevelopment()
                ? $"Ocurri� un error inesperado: {exception.Message}" // Mensaje detallado en desarrollo
                : "Ocurri� un error inesperado. Por favor, intenta nuevamente m�s tarde." // Mensaje gen�rico en producci�n
        };

        if (_env.IsDevelopment())
        {
            problemDetails.Extensions.Add("stackTrace", exception.StackTrace);
            if (exception.InnerException != null)
            {
                problemDetails.Extensions.Add("innerException", exception.InnerException.Message);
            }
        }

        // Serializar ProblemDetails a JSON y escribirlo en la respuesta
        var jsonResponse = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(jsonResponse);
    }
}
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}