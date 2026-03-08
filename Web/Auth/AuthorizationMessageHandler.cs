using Microsoft.JSInterop;

namespace Web.Auth
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthorizationMessageHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 🎟️ Obtenemos el token del localStorage
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            // 🔐 Si existe el token, lo agregamos al header de autorización
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            // Enviamos la petición con el token
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
