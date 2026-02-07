using System.Xml.Linq;

namespace PlexBuddy.Api.Services
{
    public class PlexScannerService
    {
        private readonly HttpClient _httpClient;
        private readonly string _plexServerUrl = "http://YOUR_LOCAL_IP:32400";
        private readonly string _plexToken = "YOUR_PLEX_TOKEN";

        public PlexScannerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsFileOnServerAsync(string title, int? year = null)
        {
            // We query Plex's library search endpoint
            var url = $"{_plexServerUrl}/library/all?X-Plex-Token={_plexToken}&title={Uri.EscapeDataString(title)}";
            
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var doc = XDocument.Parse(content);

            // Check if any "Video" or "Directory" tags exist in the XML response
            var match = doc.Descendants("Video")
                .FirstOrDefault(x => x.Attribute("title")?.Value.ToLower() == title.ToLower());

            return match != null;
        }
    }
}