using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Training.Product.Middleware;

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

            int statusCode = StatusCodes.Status500InternalServerError;

            switch(ex)
            {
                case NotFoundException _:
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case BadRequestException _:
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case InvalidOperationException _:
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

           

            var errorResponse = new ErrorResponse
            {
                statusCode = statusCode,
                message = ex.Message,
            };

            _logger.LogError($"An error occurred, StackTrace: {ex.StackTrace}, Message: {ex.Message}");
 
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            context.Response.WriteAsJsonAsync(errorResponse).Wait();
        }
    }
}
