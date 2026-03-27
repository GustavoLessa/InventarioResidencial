using System.Net;
using FluentValidation;
using Inventario.API.Models;

namespace Inventario.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Algo correu mal: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var errorDetails = new ErrorDetails
        {
            Message = "Erro interno no servidor." 
        };

        // Aqui capturamos especificamente os erros do FluentValidation
        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            errorDetails.StatusCode = context.Response.StatusCode;
            errorDetails.Message = "Falha na validação dos dados.";
            errorDetails.Errors = validationException.Errors.Select(e => e.ErrorMessage);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            errorDetails.StatusCode = context.Response.StatusCode;
            // Em produção, não mostramos os detalhes da exception por segurança
            errorDetails.Message = exception.Message; 
        }

        return context.Response.WriteAsync(errorDetails.ToString());
    }
}