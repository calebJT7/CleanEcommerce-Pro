using System.Net;
using System.Text.Json;

namespace Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;   // Guardo siguiente middleware
            _logger = logger; // Uso logger para errores
        }

        // Manejo excepciones por petición
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Intento procesar la petición
                await _next(context);
            }
            catch (Exception ex) // Capturo excepciones
            {
                _logger.LogError(ex, "Algo salió mal: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Mensaje = "Ocurrió un error interno en el servidor. Por favor, contacte a soporte.",
                Detalle = exception.Message // No muestro detalle en producción
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}