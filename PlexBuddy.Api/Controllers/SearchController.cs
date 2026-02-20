using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace PlexBuddy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SearchController(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    [HttpGet("{query}")]
    public async Task<IActionResult> Search(string query)
    {
        var apiKey = _configuration["Tmdb:ApiKey"];
        
        // Initial search to get basic movie/TV data (Titles, IDs, Poster paths)
        var url = $"https://api.themoviedb.org/3/search/multi?api_key={apiKey}&query={Uri.EscapeDataString(query)}";
        var response = await _httpClient.GetFromJsonAsync<TmdbResponse>(url);
        
        if (response?.Results == null) return Ok(new List<object>());

        // We create a list of 'Tasks' to fetch IMDb IDs for every result simultaneously.
        // This prevents the "n+1" performance bottleneck.
        var tasks = response.Results
            .Where(r => r.MediaType == "movie" || r.MediaType == "tv")
            .Select(async r => {
                // Extract release year strictly from the YYYY-MM-DD string
                var rawDate = r.MediaType == "movie" ? r.ReleaseDate : r.FirstAirDate;
                string? yearOnly = (!string.IsNullOrWhiteSpace(rawDate) && rawDate.Length >= 4) 
                    ? rawDate.Substring(0, 4) 
                    : null;

                // Call the 'external_ids' endpoint to get the specific 'tt12345' IMDb identifier
                var externalIdUrl = $"https://api.themoviedb.org/3/{r.MediaType}/{r.Id}/external_ids?api_key={apiKey}";
                var ids = await _httpClient.GetFromJsonAsync<ExternalIds>(externalIdUrl);

                return new {
                    Id = r.Id,
                    Title = r.MediaType == "movie" ? r.Title : r.Name,
                    PosterUrl = r.PosterPath,
                    MediaType = r.MediaType,
                    Year = yearOnly,
                    // Construct a direct URL. If no IMDb ID found, fallback to a general IMDb search
                    ImdbUrl = !string.IsNullOrEmpty(ids?.ImdbId) 
                        ? $"https://www.imdb.com/title/{ids.ImdbId}/" 
                        : $"https://www.imdb.com/find?q={Uri.EscapeDataString(r.Title ?? r.Name ?? "")}"
                };
            });

        // Task.WhenAll waits for all parallel IMDb requests to finish before returning the final list
        var results = await Task.WhenAll(tasks);
        return Ok(results);
    }
}

// Data models for JSON deserialization
public class ExternalIds { [JsonPropertyName("imdb_id")] public string? ImdbId { get; set; } }
public class TmdbResponse { [JsonPropertyName("results")] public List<TmdbSearchResult> Results { get; set; } = new(); }
public class TmdbSearchResult {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Name { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("media_type")] public string? MediaType { get; set; }
    [JsonPropertyName("release_date")] public string? ReleaseDate { get; set; }
    [JsonPropertyName("first_air_date")] public string? FirstAirDate { get; set; }
}