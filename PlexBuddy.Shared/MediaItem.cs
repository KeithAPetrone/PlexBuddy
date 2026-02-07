namespace PlexBuddy.Shared;

public class MediaItem
{
    public int Id { get; set; }
    public string PlexUserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string TmdbId { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string MediaType { get; set; } = "movie"; // "movie" or "tv"
    
    public ItemStatus Status { get; set; } = ItemStatus.Wishlist;
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}