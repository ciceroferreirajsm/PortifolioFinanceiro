using PortifolioFinanceiro.Models.Response;
using System.Text.Json;

namespace PortifolioFinanceiro.Middleware
{
    /// <summary>
    /// Middleware para padronizar respostas de sucesso
    /// </summary>
    public class ResponseFormattingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseFormattingMiddleware> _logger;

        public ResponseFormattingMiddleware(RequestDelegate next, ILogger<ResponseFormattingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Captura a resposta original
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            // Só processa se for uma resposta de sucesso e não for Swagger
            if (ShouldFormatResponse(context))
            {
                await FormatResponse(context, originalBodyStream);
            }
            else
            {
                // Copia a resposta original para o stream original
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private bool ShouldFormatResponse(HttpContext context)
        {
            // Não formata se:
            // - Não for uma resposta de sucesso (200-299)
            // - For uma requisição para Swagger
            // - For uma resposta que já foi formatada (contém "application/json")
            // - A resposta estiver vazia

            if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
                return false;

            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/api-docs"))
                return false;

            if (context.Response.Body.Length == 0)
                return false;

            return context.Response.ContentType?.Contains("application/json") == true;
        }

        private async Task FormatResponse(HttpContext context, Stream originalBodyStream)
        {
            try
            {
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(responseText))
                {
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await context.Response.Body.CopyToAsync(originalBodyStream);
                    return;
                }

                // Verifica se a resposta já está no formato ApiResponse
                if (IsAlreadyFormatted(responseText))
                {
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await context.Response.Body.CopyToAsync(originalBodyStream);
                    return;
                }

                // Deserializa o conteúdo original
                object? originalData = null;
                try
                {
                    originalData = JsonSerializer.Deserialize<object>(responseText);
                }
                catch
                {
                    // Se não conseguir deserializar, usa o texto original
                    originalData = responseText;
                }

                // Cria a resposta padronizada
                var apiResponse = new ApiResponse(
                    data: originalData,
                    message: GetSuccessMessage(context),
                    statusCode: context.Response.StatusCode
                );

                // Serializa a nova resposta
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var formattedResponse = JsonSerializer.Serialize(apiResponse, jsonOptions);
                var bytes = System.Text.Encoding.UTF8.GetBytes(formattedResponse);

                // Atualiza o Content-Length
                context.Response.ContentLength = bytes.Length;
                context.Response.Body = originalBodyStream;

                await context.Response.Body.WriteAsync(bytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error formatting response");
                // Em caso de erro, retorna a resposta original
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await context.Response.Body.CopyToAsync(originalBodyStream);
            }
        }

        private bool IsAlreadyFormatted(string responseText)
        {
            try
            {
                using var document = JsonDocument.Parse(responseText);
                var root = document.RootElement;

                return root.TryGetProperty("statusCode", out _) &&
                       root.TryGetProperty("message", out _) &&
                       root.TryGetProperty("data", out _);
            }
            catch
            {
                return false;
            }
        }

        private string GetSuccessMessage(HttpContext context)
        {
            return context.Response.StatusCode switch
            {
                200 => "Operation completed successfully",
                201 => "Resource created successfully",
                202 => "Request accepted",
                204 => "Operation completed successfully",
                _ => "Success"
            };
        }
    }

    /// <summary>
    /// Extensão para facilitar o registro do middleware
    /// </summary>
    public static class ResponseFormattingMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseFormatting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseFormattingMiddleware>();
        }
    }
}
