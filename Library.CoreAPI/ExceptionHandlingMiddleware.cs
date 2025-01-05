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
                await HandleException(context, ex);
            }
            catch(NotFoundException ex)
            {
                await HandleException(context, ex);
            }
            catch(UnauthorizedException ex)
            {
                await HandleException(context, ex);
            }
            catch(ConflictException ex)
            {
                await HandleException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
            
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            int statusCode;
            string message,details;

            switch (exception)
            {
                case BadRequestException badRequestException:
                    statusCode = 400;
                    message = badRequestException.Message;
                    details = badRequestException.Details;
                    break;
                case NotFoundException notFoundException:
                    statusCode = 404;
                    message = notFoundException.Message;
                    details = notFoundException.Details; 
                    break;
                case UnauthorizedException unauthorizedAccessException:
                    statusCode = 401;
                    message = unauthorizedAccessException.Message;
                    details = unauthorizedAccessException.Details;
                    break;
                case ConflictException conflictException:
                    statusCode = 409;
                    message = conflictException.Message;
                    details = conflictException.Details;
                    break;
                default:
                    statusCode = 500;
                    message = "An unexpected error occurred.";
                    details = exception.Message;
                    break;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = statusCode,
                Message = message,
                Details = details
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
