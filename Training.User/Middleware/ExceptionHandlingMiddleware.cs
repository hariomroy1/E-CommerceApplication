using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Training.User.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                    // Handle other exceptions and log them
                    LogException(context, ex);
                
            }
        }
        private void LogException(HttpContext context, Exception ex)
        {
            // Log the exception and handle it based on its type
            _logger.LogError($"An error occurred: {ex.Message}", ex);

            context.Response.StatusCode =
                   ex is InvalidOperationException
                // Client-side error
                ? (int)HttpStatusCode.BadRequest
                // Internal server error
                : (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Title = ex is InvalidOperationException ? "Bad Request" : "Internal Server Error",
                Detail = $"Error Message: {ex.Message}",
                Status = context.Response.StatusCode,
                Instance = context.Request.Path,
            };

            _logger.LogError($"An error occurred, StackTrace: {ex.StackTrace}, Message: {ex.Message}");

            // Write the response as JSON
            context.Response.WriteAsJsonAsync(problemDetails).Wait();
        }
    }
}
