// Usando System.Net;
// Importa las bibliotecas necesarias
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace App.API.Foundation.Exceptions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger; 

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                _logger.LogError(ex, "Uncaught exception"); 
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = ex switch
            {
                MiddleNotFoundException _ => HttpStatusCode.NotFound,
                MiddleUnauthorizedException _ => HttpStatusCode.Unauthorized,
                MiddleBadRequestException _ => HttpStatusCode.BadRequest,
                TimeoutException _ => HttpStatusCode.RequestTimeout,
                InvalidOperationException _ => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError,
            };

            var result = JsonSerializer.Serialize(new ErrorDetails
            {
                StatusCode = (int)code,
                Message = ex.Message 
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
