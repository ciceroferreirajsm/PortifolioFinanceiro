using Microsoft.AspNetCore.Mvc;
using PortifolioFinanceiro.Models.Response;

namespace PortifolioFinanceiro.Extensions
{
    /// <summary>
    /// Extensões para facilitar o retorno de respostas padronizadas nos controllers
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Retorna uma resposta de sucesso (200)
        /// </summary>
        public static IActionResult ApiOk<T>(this ControllerBase controller, T data, string message = "Operation completed successfully")
        {
            var response = ApiResponse<T>.Success(data, message);
            return controller.Ok(response);
        }

        /// <summary>
        /// Retorna uma resposta de sucesso sem dados (200)
        /// </summary>
        public static IActionResult ApiOk(this ControllerBase controller, string message = "Operation completed successfully")
        {
            var response = ApiResponse.Success(message);
            return controller.Ok(response);
        }

        /// <summary>
        /// Retorna uma resposta de criação (201)
        /// </summary>
        public static IActionResult ApiCreated<T>(this ControllerBase controller, T data, string message = "Resource created successfully")
        {
            var response = ApiResponse<T>.Created(data, message);
            return controller.StatusCode(201, response);
        }

        /// <summary>
        /// Retorna uma resposta de criação com location (201)
        /// </summary>
        public static IActionResult ApiCreated<T>(this ControllerBase controller, string actionName, object routeValues, T data, string message = "Resource created successfully")
        {
            var response = ApiResponse<T>.Created(data, message);
            return controller.CreatedAtAction(actionName, routeValues, response);
        }

        /// <summary>
        /// Retorna uma resposta de não encontrado (404)
        /// </summary>
        public static IActionResult ApiNotFound(this ControllerBase controller, string message = "Resource not found")
        {
            var response = ApiResponse.NotFound(message);
            return controller.NotFound(response);
        }

        /// <summary>
        /// Retorna uma resposta de dados inválidos (400)
        /// </summary>
        public static IActionResult ApiBadRequest(this ControllerBase controller, string message = "Invalid data provided", List<ValidationError>? validationErrors = null)
        {
            var response = ApiResponse.BadRequest(message, validationErrors);
            return controller.BadRequest(response);
        }

        /// <summary>
        /// Retorna uma resposta de dados inválidos baseada no ModelState (400)
        /// </summary>
        public static IActionResult ApiBadRequest(this ControllerBase controller, string message = "Validation failed")
        {
            var validationErrors = controller.ModelState
                .SelectMany(x => x.Value?.Errors ?? new Microsoft.AspNetCore.Mvc.ModelBinding.ModelErrorCollection())
                .Select(e => new ValidationError(
                    field: controller.ModelState.First(x => x.Value?.Errors.Contains(e) == true).Key,
                    message: e.ErrorMessage
                ))
                .ToList();

            var response = ApiResponse.BadRequest(message, validationErrors);
            return controller.BadRequest(response);
        }

        /// <summary>
        /// Retorna uma resposta de erro interno (500)
        /// </summary>
        public static IActionResult ApiInternalServerError(this ControllerBase controller, string message = "An internal server error occurred")
        {
            var response = ApiResponse.InternalServerError(message);
            return controller.StatusCode(500, response);
        }

        /// <summary>
        /// Retorna uma resposta sem conteúdo (204)
        /// </summary>
        public static IActionResult ApiNoContent(this ControllerBase controller, string message = "Operation completed successfully")
        {
            var response = ApiResponse.Success(message);
            response.StatusCode = 204;
            return controller.StatusCode(204, response);
        }

        /// <summary>
        /// Retorna uma resposta de erro customizada
        /// </summary>
        public static IActionResult ApiError(this ControllerBase controller, string message, int statusCode, List<ValidationError>? validationErrors = null)
        {
            var response = ApiResponse.Error(message, statusCode, validationErrors);
            return controller.StatusCode(statusCode, response);
        }

        /// <summary>
        /// Retorna uma resposta customizada
        /// </summary>
        public static IActionResult ApiCustomResponse<T>(this ControllerBase controller, T data, string message, int statusCode)
        {
            var response = new ApiResponse<T>(data, message, statusCode);
            return controller.StatusCode(statusCode, response);
        }

    }
}
