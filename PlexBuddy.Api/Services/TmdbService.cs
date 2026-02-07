using TMDbLib.Client;
using PlexBuddy.Shared;

namespace PlexBuddy.Api.Services;

public class TmdbService
{
    private readonly TMDbClient _client;
    // Replace this with your actual key from themoviedb.org
    private const string ApiKey = "YOUR_TMDB_API_KEY"; 

    public TmdbService()
    {
        _client = new TMDbClient(ApiKey);
    }

    public async Task<List<TmdbSearchResult>> SearchMoviesAsync(string query)
    {
        var results = await _client.SearchMovieAsync(query);
        
        return results.Results.Select(m => new TmdbSearchResult
        {
            Id = m.Id,
            Title = m.Title,
            Overview = m.Overview,
            ReleaseDate = m.ReleaseDate?.ToString("yyyy") ?? "N/A",
            // We build the full URL for the poster image here
            PosterUrl = string.IsNullOrEmpty(m.PosterPath) 
                ? "https://via.placeholder.com/500x750?text=No+Poster" 
                : $"https://image.tmdb.org/t/p/w500{m.PosterPath}"
        }).ToList();
    }
}