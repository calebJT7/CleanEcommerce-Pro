using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace Web.Auth
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthHeaderHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 1. Buscamos el token VIP en la memoria del navegador
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", new object[] { "authToken" });

            // 2. Si lo tenemos, se lo pegamos a la cabecera (Header) de la petición
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // 3. Dejamos que la petición siga su viaje hacia Azure
            return await base.SendAsync(request, cancellationToken);
        }
    }
}