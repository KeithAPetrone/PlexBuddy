using System.ComponentModel.DataAnnotations;

namespace PlexWishlist.Api.Models
{
    public enum ItemStatus { Wishlist, Available, Watched }
    public enum MediaType { Movie, Show }

    public class MediaItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string PlexUserId { get; set; } // We store the Plex ID, not email, as it's immutable
        
        [Required]
        public string Title { get; set; }
        
        public string TmdbId { get; set; } // Critical for matching metadata later
        
        public MediaType Type { get; set; }
        
        public ItemStatus Status { get; set; } = ItemStatus.Wishlist;
        
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }
}