using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlexWishlist.Api.Services
{
    public class PlexAuthService
    {
        private readonly HttpClient _httpClient;
        private const string ClientIdentifier = "Your-App-GUID-Here"; // Generate a random GUID and keep it static
        private const string ApplicationName = "My Plex Wishlist App";

        public PlexAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://plex.tv/api/v2/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Step 1: Request a PIN from Plex
        public async Task<PlexPinResponse> GetPinAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "pins");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "strong", "true" },
                { "X-Plex-Product", ApplicationName },
                { "X-Plex-Client-Identifier", ClientIdentifier }
            });

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PlexPinResponse>(content);
        }

        // Step 2: Check if the user has authorized the PIN
        public async Task<string?> CheckPinAsync(string pinId)
        {
            var response = await _httpClient.GetAsync($"pins/{pinId}?code={pinId}&X-Plex-Client-Identifier={ClientIdentifier}");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PlexPinResponse>(content);

            // If authToken is present, the user has successfully logged in
            return result?.AuthToken; 
        }

        // Step 3: Get User Details using the Auth Token
        public async Task<PlexUser> GetUserAsync(string authToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://plex.tv/api/v2/user");
            request.Headers.Add("X-Plex-Token", authToken);
            request.Headers.Add("X-Plex-Client-Identifier", ClientIdentifier);
            request.Headers.Add("X-Plex-Product", ApplicationName);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PlexUser>(content);
        }
    }

    // DTOs for Plex Responses
    public class PlexPinResponse
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("code")] public string Code { get; set; }
        [JsonPropertyName("authToken")] public string AuthToken { get; set; }
    }

    public class PlexUser
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("username")] public string Username { get; set; }
        [JsonPropertyName("email")] public string Email { get; set; }
        [JsonPropertyName("thumb")] public string Thumb { get; set; } // User Avatar URL
    }
}