using Book_An_Appointment1.AppService;
using Book_An_Appointment1.Model;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Book_An_Appointment1.Services
{
    public class TokenService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _config;

        public TokenService(IHttpContextAccessor context,
                            IHttpClientFactory factory,
                            IConfiguration config)
        {
            _context = context;
            _factory = factory;
            _config = config;
        }

        public async Task<string> GetTokenAsync()
        {
            var session = _context.HttpContext?.Session;

            if (session == null)
                throw new Exception("Session not available");

            // 🔥 Check existing token
            var token = session.GetString("AuthToken");

            if (!string.IsNullOrEmpty(token))
                return token;

            // 🔥 IMPORTANT: TokenClient (NO handler)
            var client = _factory.CreateClient("TokenClient");

            var request = new
            {
                userName = _config["TokenAuth:UserName"],
                password = _config["TokenAuth:Password"]
            };

            var response = await client.PostAsJsonAsync(ConstantValuecs.GetToken, request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Token API failed");

            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (string.IsNullOrEmpty(result?.access_token))
                throw new Exception("Token not received");

            // 🔥 Store in session
            session.SetString("AuthToken", result.access_token);

            return result.access_token;
        }

        public void ClearToken()
        {
            var session = _context.HttpContext?.Session;
            session?.Remove("AuthToken");
        }
    }
}