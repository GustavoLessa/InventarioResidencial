using System.Net;
using FluentValidation;
using Inventario.API.Models;

namespace Inventario.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Algo correu mal durante o processamento da requisicao.");
            await HandleExceptionAsync(httpContext, ex, _environment.IsDevelopment());
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, bool includeDetails)
    {
        context.Response.ContentType = "application/json";

        var errorDetails = new ErrorDetails();

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            errorDetails.StatusCode = context.Response.StatusCode;
            errorDetails.Message = "Falha na validacao dos dados.";
            errorDetails.Errors = validationException.Errors.Select(e => e.ErrorMessage);

            return context.Response.WriteAsync(errorDetails.ToString());
        }

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        errorDetails.StatusCode = context.Response.StatusCode;

        if (includeDetails)
        {
            var rootCause = GetRootCause(exception);
            errorDetails.Message = rootCause.Message;
            errorDetails.Errors = BuildDevelopmentErrors(exception);
        }
        else
        {
            errorDetails.Message = "Erro interno no servidor.";
        }

        return context.Response.WriteAsync(errorDetails.ToString());
    }

    private static Exception GetRootCause(Exception exception)
    {
        var current = exception;

        while (current.InnerException != null)
        {
            current = current.InnerException;
        }

        return current;
    }

    private static IEnumerable<string> BuildDevelopmentErrors(Exception exception)
    {
        var errors = new List<string>();
        var current = exception;
        var depth = 0;

        while (current != null)
        {
            errors.Add($"[{depth}] {current.GetType().Name}: {current.Message}");
            current = current.InnerException;
            depth++;
        }

        return errors;
    }
}
