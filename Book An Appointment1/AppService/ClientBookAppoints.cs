using Book_An_Appointment1.Model;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace Book_An_Appointment1.AppService
{
    public class ClientBookAppoints
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClientBookAppoints> _logger;

        public ClientBookAppoints(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<ClientBookAppoints> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Facility list fetch karta hai.
        /// facilityCode optional hai — null pass karo sab facilities lene ke liye.
        /// </summary>
        public async Task<FacilityOutput> GetFacilityAsync(
            string facilityCode = null,
            string hospitalLocationId = null)
        {
            // ── "FacilityClient" use karo ─────────────────────────
            // AuthHandler automatically Bearer token attach karega
            // BaseAddress already set hai Program.cs mein
            var client = _httpClientFactory.CreateClient("FacilityClient");

            // ── URL build karo ────────────────────────────────────
             facilityCode = "3";
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(facilityCode))
                queryParams.Add($"facilityCode={Uri.EscapeDataString(facilityCode)}");

            if (!string.IsNullOrEmpty(hospitalLocationId))
                queryParams.Add($"hospitalLocationId={Uri.EscapeDataString(hospitalLocationId)}");

            var url = ConstantValuecs.GetFacility;
            if (queryParams.Any())
                url += "?" + string.Join("&", queryParams);

            _logger.LogInformation("Calling Facility API: {Url}", url);

            // ── API call karo ─────────────────────────────────────
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("Facility API response: {StatusCode} | {Content}",
                response.StatusCode, content);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    // API direct List return kare toh
                    if (content.TrimStart().StartsWith("["))
                    {
                        var list = JsonConvert.DeserializeObject<List<FacilityItem>>(content);
                        return new FacilityOutput { Success = true, Data = list };
                    }
                    // API object return kare toh
                    return JsonConvert.DeserializeObject<FacilityOutput>(content)
                           ?? new FacilityOutput { Success = false, Message = "Empty response" };

                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Token invalid ya expire ho gaya.");

                case HttpStatusCode.NotFound:
                    throw new Exception($"API endpoint nahi mila: {url} — appsettings mein BaseUrl check karo.");

                default:
                    throw new Exception($"API Error: {response.StatusCode} | {content}");
            }
        }
    }
}

//public async Task<FacilityOutput> GetFacilityAsync(string facilityCode, string hospitalLocationId)
//{
//    var baseUrl = _configuration["ApiSettings:BaseUrl"];
//    var fullUrl = $"{baseUrl}{ConstantValuecs.GetFacility}?facilityCode={facilityCode}&hospitalLocationId={hospitalLocationId}";
//    var client = _httpClientFactory.CreateClient();
//    var response = await client.GetAsync(fullUrl);
//    var content = await response.Content.ReadAsStringAsync();

//    if (response.IsSuccessStatusCode)
//    {
//        var result = JsonConvert.DeserializeObject<FacilityOutput>(content);
//        return result;
//    }
//    else
//    {
//        throw new Exception($"API failed: {response.StatusCode}, {content}");
//    }
//}






