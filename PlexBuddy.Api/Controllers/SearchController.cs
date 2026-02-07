using Microsoft.AspNetCore.Mvc;
using PlexBuddy.Api.Services;
using PlexBuddy.Shared;

namespace PlexBuddy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly TmdbService _tmdbService;

    public SearchController(TmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    // This handles GET /api/search (no query)
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Search endpoint is active. Please provide a query, e.g., /api/search/matrix");
    }

    // This handles GET /api/search/{query}
    [HttpGet("{query}")]
    public async Task<ActionResult<List<TmdbSearchResult>>> Get(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) 
            return BadRequest("Query cannot be empty");

        try
        {
            var results = await _tmdbService.SearchMoviesAsync(query);
            return Ok(results);
        }
        catch (Exception ex)
        {
            // This usually happens if the TMDB API Key is invalid or missing
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}