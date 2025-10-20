using PortifolioFinanceiro.Models.Response;
using System.Net;
using System.Text.Json;

namespace PortifolioFinanceiro.Middleware
{
    /// <summary>
    /// Middleware para tratamento global de exceções
    /// </summary>
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
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                ArgumentException argEx => ApiResponse.BadRequest(argEx.Message),
                KeyNotFoundException notFoundEx => ApiResponse.NotFound(notFoundEx.Message),
                UnauthorizedAccessException => ApiResponse.Error("Unauthorized access", (int)HttpStatusCode.Unauthorized),
                InvalidOperationException invalidOpEx => ApiResponse.BadRequest(invalidOpEx.Message),
                NotSupportedException notSupportedEx => ApiResponse.BadRequest(notSupportedEx.Message),
                TimeoutException => ApiResponse.Error("Request timeout", (int)HttpStatusCode.RequestTimeout),
                _ => ApiResponse.InternalServerError("An internal server error occurred")
            };

            // Define o status code na response HTTP
            context.Response.StatusCode = response.StatusCode;

            // Adiciona informações extras em ambiente de desenvolvimento
            if (IsDevelopmentEnvironment())
            {
                response.Data = new
                {
                    ExceptionType = exception.GetType().Name,
                    StackTrace = exception.StackTrace,
                    InnerException = exception.InnerException?.Message
                };
            }

            // Serializa e retorna a resposta
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private bool IsDevelopmentEnvironment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        }
    }

    /// <summary>
    /// Extensão para facilitar o registro do middleware
    /// </summary>
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
