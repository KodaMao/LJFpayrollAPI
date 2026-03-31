using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LJFpayrollAPI.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {

            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            var (statusCode, title) = exception switch
            {
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                InvalidOperationException => (StatusCodes.Status400BadRequest, "Bad Request"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Validation Error"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message, 
                Instance = httpContext.Request.Path,
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
