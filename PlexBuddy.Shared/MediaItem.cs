namespace PlexBuddy.Shared;

public class MediaItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string TmdbId { get; set; } = string.Empty;
    public string? PosterUrl { get; set; }
    public string? Year { get; set; }
    public string MediaType { get; set; } = "movie";
    public string PlexUserId { get; set; } = "Admin";
    public int Status { get; set; } = 0; 
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}