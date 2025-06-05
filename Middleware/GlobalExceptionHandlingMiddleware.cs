using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Exceptions;

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
        catch (ApiException apiEx)
        {
            _logger.Log(
                logLevel:LogLevel.Error,
                message: "[{Endpoint}]: {Title} - {Detail} - {Code}.",
                context.Request.Path,
                apiEx.ErrorTitle,
                apiEx.Message,
                apiEx.ErrorCode
             );
            await HandleApiExceptionAsync(context, apiEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió una excepción no controlada para la solicitud {Method}, {Path}",
                             context.Request.Method, context.Request.Path);

            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private static async Task HandleApiExceptionAsync(HttpContext context, ApiException aex)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = aex.ErrorStatus;
        var problemDetails = new ProblemDetails
        {
            Status = aex.ErrorStatus,
            Title = aex.ErrorTitle,
            Detail = aex.Message,
            Extensions = { ["code"] = aex.ErrorCode },
        };
        var jsonResponse = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(jsonResponse);
    }

    private async Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Unexpected Server Error Occurred",
            Detail = _env.IsDevelopment()
                ? $"An unexpected error occurred on the server: {exception.Message}"
                : "An unexpected error occurred while processing your request. Please try again later or contact support if the problem persists.",
            Extensions = { ["code"] = "INTERNAL_SERVER_ERROR" }
        };

        if (_env.IsDevelopment())
        {
            problemDetails.Extensions.Add("stackTrace", exception.StackTrace);
            if (exception.InnerException != null)
            {
                problemDetails.Extensions.Add("innerException", exception.InnerException.Message);
            }
        }

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