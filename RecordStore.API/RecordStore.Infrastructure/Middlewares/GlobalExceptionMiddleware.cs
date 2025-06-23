using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecordStore.Infrastructure.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                ArgumentNullException argEx => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Error = "Bad Request",
                    Message = $"Required parameter is missing: {argEx.ParamName}",
                    Details = argEx.Message
                },
                ArgumentException argEx => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Error = "Bad Request",
                    Message = "Invalid argument provided",
                    Details = argEx.Message
                },
                KeyNotFoundException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Error = "Not Found",
                    Message = "The requested resource was not found",
                    Details = exception.Message
                },
                UnauthorizedAccessException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Error = "Unauthorized",
                    Message = "Access to this resource is unauthorized",
                    Details = exception.Message
                },
                DbUpdateException dbEx => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Error = "Database Conflict",
                    Message = "A database conflict occurred",
                    Details = GetDatabaseErrorMessage(dbEx)
                },
                InvalidOperationException invalidOpEx => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Error = "Invalid Operation",
                    Message = "The requested operation is not valid",
                    Details = invalidOpEx.Message
                },
                TimeoutException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.RequestTimeout,
                    Error = "Request Timeout",
                    Message = "The request timed out",
                    Details = exception.Message
                },
                _ => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Error = "Internal Server Error",
                    Message = "An unexpected error occurred",
                    Details = "Please contact support if the problem persists"
                }
            };

            context.Response.StatusCode = response.StatusCode;

            response.Timestamp = DateTime.UtcNow;
            response.Path = context.Request.Path;
            response.Method = context.Request.Method;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private string GetDatabaseErrorMessage(DbUpdateException dbEx)
        {
            var message = dbEx.InnerException?.Message ?? dbEx.Message;

            if (message.Contains("UNIQUE constraint", StringComparison.OrdinalIgnoreCase))
            {
                return "A record with these details already exists.";
            }

            if (message.Contains("FOREIGN KEY constraint", StringComparison.OrdinalIgnoreCase))
            {
                return "Cannot perform this operation due to related data constraints.";
            }

            if (message.Contains("CHECK constraint", StringComparison.OrdinalIgnoreCase))
            {
                return "The provided data does not meet the required constraints.";
            }

            return "A database error occurred while processing your request.";
        }

        private class ErrorResponse
        {
            public int StatusCode { get; set; }
            public string Error { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public string Details { get; set; } = string.Empty;
            public DateTime Timestamp { get; set; }
            public string Path { get; set; } = string.Empty;
            public string Method { get; set; } = string.Empty;
        }
    }
}
