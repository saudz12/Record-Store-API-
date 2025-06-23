using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecordStore.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecordStore.Infrastructure.Middlewares
{
    public class IdValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IdValidationMiddleware> _logger;

        public IdValidationMiddleware(RequestDelegate next, ILogger<IdValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var validationResult = ValidateRouteIds(context);
                if (!validationResult.IsValid)
                {
                    await HandleInvalidId(context, validationResult.ErrorMessage);
                    return;
                }

                await _next(context);
            }
            catch (ArtistNotFoundException ex)
            {
                _logger.LogError(ex, "An error occurred in IdValidationMiddleware");
                await RespondToExceptionAsync(context, HttpStatusCode.NotFound, ex.Message, ex);
            }
            catch (RecordNotFoundException ex)
            {
                _logger.LogError(ex, "An error occurred in IdValidationMiddleware");
                await RespondToExceptionAsync(context, HttpStatusCode.NotFound, ex.Message, ex);
            }
            catch (OrderNotFoundException ex)
            {
                _logger.LogError(ex, "An error occurred in IdValidationMiddleware");
                await RespondToExceptionAsync(context, HttpStatusCode.NotFound, ex.Message, ex);
            }
            catch (UserNotFoundException ex) 
            {
                _logger.LogError(ex, "An error occurred in IdValidationMiddleware");
                await RespondToExceptionAsync(context, HttpStatusCode.NotFound, ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in IdValidationMiddleware");
                await HandleException(context, ex);
            }
        }

        private static Task RespondToExceptionAsync(HttpContext context, HttpStatusCode failureStatusCode, string message, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)failureStatusCode;

            var response = new
            {
                Message = message,
                Error = exception.GetType().Name,
                Timestamp = DateTime.UtcNow
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }));
        }

        private IdValidationResult ValidateRouteIds(HttpContext context)
        {
            var routeValues = context.Request.RouteValues;

            var idParameters = new[] { "id", "recordId", "artistId", "userId", "orderId", "reviewId" };

            foreach (var paramName in idParameters)
            {
                if (routeValues.ContainsKey(paramName))
                {
                    var idValue = routeValues[paramName]?.ToString();

                    if (string.IsNullOrWhiteSpace(idValue))
                    {
                        return new IdValidationResult
                        {
                            IsValid = false,
                            ErrorMessage = $"Parameter '{paramName}' cannot be empty."
                        };
                    }

                    if (!int.TryParse(idValue, out int id))
                    {
                        return new IdValidationResult
                        {
                            IsValid = false,
                            ErrorMessage = $"Parameter '{paramName}' must be a valid integer. Received: '{idValue}'"
                        };
                    }

                    if (id <= 0)
                    {
                        return new IdValidationResult
                        {
                            IsValid = false,
                            ErrorMessage = $"Parameter '{paramName}' must be a positive integer greater than 0. Received: {id}"
                        };
                    }

                    if (id > int.MaxValue / 2) // overflow in data
                    {
                        return new IdValidationResult
                        {
                            IsValid = false,
                            ErrorMessage = $"Parameter '{paramName}' is too large. Maximum allowed value is {int.MaxValue / 2}. Received: {id}"
                        };
                    }
                }
            }

            return new IdValidationResult { IsValid = true };
        }

        private async Task HandleInvalidId(HttpContext context, string errorMessage)
        {
            _logger.LogWarning("Invalid ID parameter: {ErrorMessage} for request {Method} {Path}",
                errorMessage, context.Request.Method, context.Request.Path);

            context.Response.StatusCode = 400; // bad
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = "Invalid ID Parameter",
                message = errorMessage,
                timestamp = DateTime.UtcNow,
                path = context.Request.Path.Value,
                method = context.Request.Method
            };

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500; //gg
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = "Internal Server Error",
                message = "An unexpected error occurred while processing the request.",
                timestamp = DateTime.UtcNow,
                path = context.Request.Path.Value,
                method = context.Request.Method
            };

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private class IdValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
        }
    }
}
