using Microsoft.EntityFrameworkCore;
using PlexWishlist.Api.Models;

namespace PlexWishlist.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MediaItem> Wishlist { get; set; }
    }
}