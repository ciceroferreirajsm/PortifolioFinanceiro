using System.Text.Json.Serialization;

namespace PortifolioFinanceiro.Models.Response
{
    /// <summary>
    /// Padrão de resposta para todas as APIs
    /// </summary>
    /// <typeparam name="T">Tipo do objeto de dados</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Dados da resposta
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Mensagem de resposta
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Código de status HTTP
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Indica se a operação foi bem-sucedida
        /// </summary>
        [JsonIgnore]
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;

        /// <summary>
        /// Timestamp da resposta
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Erros de validação (opcional)
        /// </summary>
        public List<ValidationError>? ValidationErrors { get; set; }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public ApiResponse()
        {
        }

        /// <summary>
        /// Construtor para resposta de sucesso
        /// </summary>
        public ApiResponse(T data, string message = "Success", int statusCode = 200)
        {
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Construtor para resposta de erro
        /// </summary>
        public ApiResponse(string message, int statusCode, List<ValidationError>? validationErrors = null)
        {
            Message = message;
            StatusCode = statusCode;
            ValidationErrors = validationErrors;
        }

        /// <summary>
        /// Cria uma resposta de sucesso
        /// </summary>
        public static ApiResponse<T> Success(T data, string message = "Operation completed successfully")
        {
            return new ApiResponse<T>(data, message, 200);
        }

        /// <summary>
        /// Cria uma resposta de sucesso para criação
        /// </summary>
        public static ApiResponse<T> Created(T data, string message = "Resource created successfully")
        {
            return new ApiResponse<T>(data, message, 201);
        }

        /// <summary>
        /// Cria uma resposta de erro
        /// </summary>
        public static ApiResponse<T> Error(string message, int statusCode = 500, List<ValidationError>? validationErrors = null)
        {
            return new ApiResponse<T>(message, statusCode, validationErrors);
        }

        /// <summary>
        /// Cria uma resposta de não encontrado
        /// </summary>
        public static ApiResponse<T> NotFound(string message = "Resource not found")
        {
            return new ApiResponse<T>(message, 404);
        }

        /// <summary>
        /// Cria uma resposta de dados inválidos
        /// </summary>
        public static ApiResponse<T> BadRequest(string message = "Invalid data provided", List<ValidationError>? validationErrors = null)
        {
            return new ApiResponse<T>(message, 400, validationErrors);
        }

        /// <summary>
        /// Cria uma resposta de erro interno
        /// </summary>
        public static ApiResponse<T> InternalServerError(string message = "An internal server error occurred")
        {
            return new ApiResponse<T>(message, 500);
        }
    }

    /// <summary>
    /// Resposta simples sem dados tipados
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse() : base() { }
        public ApiResponse(object data, string message = "Success", int statusCode = 200) : base(data, message, statusCode) { }
        public ApiResponse(string message, int statusCode, List<ValidationError>? validationErrors = null) : base(message, statusCode, validationErrors) { }

        /// <summary>
        /// Cria uma resposta de sucesso simples
        /// </summary>
        public static ApiResponse Success(string message = "Operation completed successfully")
        {
            return new ApiResponse(null, message, 200);
        }

        /// <summary>
        /// Cria uma resposta de sucesso com dados
        /// </summary>
        public static ApiResponse Success(object data, string message = "Operation completed successfully")
        {
            return new ApiResponse(data, message, 200);
        }
    }

    /// <summary>
    /// Erro de validação
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Campo que contém o erro
        /// </summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// Mensagem de erro
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Valor que causou o erro
        /// </summary>
        public object? Value { get; set; }

        public ValidationError() { }

        public ValidationError(string field, string message, object? value = null)
        {
            Field = field;
            Message = message;
            Value = value;
        }
    }
}
