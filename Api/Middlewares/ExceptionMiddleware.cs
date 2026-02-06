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
            _next = next;   // El siguiente paso en la tubería
            _logger = logger; // Para guardar el error en los logs (consola)
        }

        // Este método se ejecuta en CADA petición
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Intenta dejar pasar la petición...
                await _next(context);
            }
            catch (Exception ex) // ¡Si algo explota en el camino, cae aquí!
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
                Detalle = exception.Message // En producción, esto no se suele mostrar
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}