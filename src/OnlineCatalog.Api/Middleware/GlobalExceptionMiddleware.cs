using System.Diagnostics;
using System.Text.Json;
using FluentValidation;
using OnlineCatalog.Api.Models;
using OnlineCatalog.Domain.Exceptions;

namespace OnlineCatalog.Api.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        var response = exception switch
        {
            ValidationException vex => BuildValidationError(vex, traceId),
            NotFoundException => new ErrorResponse { Status = 404, Message = exception.Message, TraceId = traceId },
            ConflictException => new ErrorResponse { Status = 409, Message = exception.Message, TraceId = traceId },
            ForbiddenException => new ErrorResponse { Status = 403, Message = exception.Message, TraceId = traceId },
            UnprocessableEntityException uex => new ErrorResponse
            {
                Status = 422,
                Message = "Validation failed.",
                TraceId = traceId,
                Errors = new Dictionary<string, string[]> { [uex.Field] = [uex.Message] }
            },
            UnauthorizedAccessException => new ErrorResponse { Status = 401, Message = "Unauthorized.", TraceId = traceId },
            _ => null
        };

        if (response is null)
        {
            logger.LogError(exception, "Unhandled exception. TraceId: {TraceId}", traceId);
            response = new ErrorResponse { Status = 500, Message = "An unexpected error occurred.", TraceId = traceId };
        }

        context.Response.StatusCode = response.Status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        }));
    }

    private static ErrorResponse BuildValidationError(ValidationException vex, string traceId)
    {
        var errors = vex.Errors
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => char.ToLowerInvariant(g.Key[0]) + g.Key[1..],
                g => g.Select(f => f.ErrorMessage).ToArray());

        return new ErrorResponse { Status = 400, Message = "Validation failed.", TraceId = traceId, Errors = errors };
    }
}
