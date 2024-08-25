using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace App.API.Foundation.Validations
{
    public class AddRequiredHeadersOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Timestamp",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Nonce",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }
    }

    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEY = "X-Api-Key";
        private const string TIMESTAMP = "X-Timestamp";
        private const string NONCE = "X-Nonce";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<AllowAnonymousApiKeyAttribute>() != null;

            if (allowAnonymous)
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey) ||
                !context.Request.Headers.TryGetValue(TIMESTAMP, out var extractedTimestamp) ||
                !context.Request.Headers.TryGetValue(NONCE, out var extractedNonce))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key, Timestamp, or Nonce was not provided.");
                return;
            }

            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>("X-Api-Key");

            if (apiKey == null || !apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            if (!long.TryParse(extractedTimestamp, out var timestamp))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid timestamp.");
                return;
            }

            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var requestTime = DateTimeOffset.FromUnixTimeSeconds(timestamp);

            // if (Math.Abs((currentTime - timestamp)) > 300)
            // {
            //     context.Response.StatusCode = 401;
            //     await context.Response.WriteAsync("Request timestamp is outside the allowable window.");
            //     return;
            // }

            // Implement nonce logic

            await _next(context);
        }

    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }

}
