namespace PlexBuddy.Shared;

public class TmdbSearchResult
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
    public string ReleaseDate { get; set; } = string.Empty;
}