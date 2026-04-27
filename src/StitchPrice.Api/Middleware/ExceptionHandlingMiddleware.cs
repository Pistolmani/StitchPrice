using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StitchPrice.Application.Common.Exceptions;

namespace StitchPrice.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private const string ProblemContentType = "application/problem+json";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await WriteValidationProblemAsync(context, ex);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Resource not found", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception while processing {Path}", context.Request.Path);
            await WriteProblemAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred",
                "An unexpected error occurred. Please try again later.");
        }
    }

    private static Task WriteValidationProblemAsync(HttpContext context, ValidationException ex)
    {
        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

        var problem = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = ProblemContentType;
        return context.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonOptions));
    }

    private static Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = ProblemContentType;
        return context.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonOptions));
    }
}
