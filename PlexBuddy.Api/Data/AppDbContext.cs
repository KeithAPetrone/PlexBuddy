using Microsoft.EntityFrameworkCore;
using PlexBuddy.Shared;

namespace PlexBuddy.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<MediaItem> WishlistItems { get; set; }
}