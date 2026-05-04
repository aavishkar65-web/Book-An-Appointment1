using Book_An_Appointment1.Services;
using System.Net.Http.Headers;

namespace Book_An_Appointment1.Handlers
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;

        public AuthHandler(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = await _tokenService.GetTokenAsync();

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(request, cancellationToken);

            // 🔥 Auto refresh on 401
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _tokenService.ClearToken();
            }

            return response;
        }
    }
}