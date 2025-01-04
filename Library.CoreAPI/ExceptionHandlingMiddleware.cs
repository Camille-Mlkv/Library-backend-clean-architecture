using Library.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Library.Infrastructure
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (BadRequestException ex)
            {
                await HandleBadRequestException(context, ex);
            }
            catch(NotFoundException ex)
            {
                await HandleNotFoundException(context, ex);
            }
            catch(UnauthorizedException ex)
            {
                await HandleUnauthorizedException(context, ex);
            }
            catch(ConflictException ex)
            {
                await HandleConflictException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An unexpected error occurred. Please try again later.",
                Details = exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }

        private Task HandleBadRequestException(HttpContext context, BadRequestException exception)
        {
            _logger.LogWarning(exception, "A handled exception occurred.");

            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = 400,
                Message = exception.Message,
                Details = exception.Details
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }

        private Task HandleNotFoundException(HttpContext context, NotFoundException exception)
        {
            _logger.LogWarning(exception, "A handled exception occurred.");

            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = 404,
                Message = exception.Message,
                Details = exception.Details
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }

        private Task HandleUnauthorizedException(HttpContext context, UnauthorizedException exception)
        {
            _logger.LogWarning(exception, "A handled exception occurred.");

            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = 401,
                Message = exception.Message,
                Details = exception.Details
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }

        private Task HandleConflictException(HttpContext context, ConflictException exception)
        {
            _logger.LogWarning(exception, "A handled exception occurred.");

            context.Response.StatusCode = 409;
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = 409,
                Message = exception.Message,
                Details = exception.Details
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
