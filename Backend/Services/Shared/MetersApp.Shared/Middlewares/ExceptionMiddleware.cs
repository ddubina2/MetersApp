namespace MetersApp.Shared.Middlewares;

using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception occurred");

        var problemDetails = ex switch
        {
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error occurred",
            },
        };

        if (_env.IsDevelopment())
        {
            problemDetails.Extensions["stackTrace"] = ex.StackTrace;
            problemDetails.Detail = ex.Message;
        }

        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = problemDetails.Status ?? 500;
        await response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}
