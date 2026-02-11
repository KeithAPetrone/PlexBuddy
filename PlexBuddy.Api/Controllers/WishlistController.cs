using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlexBuddy.Api.Data;
using PlexBuddy.Shared;

namespace PlexBuddy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WishlistController : ControllerBase
{
    private readonly AppDbContext _context;

    public WishlistController(AppDbContext context) => _context = context;

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<MediaItem>>> GetAll() 
        => await _context.WishlistItems.ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] MediaItem item)
    {
        // Check if this user already requested this movie
        var exists = await _context.WishlistItems.AnyAsync(x => x.TmdbId == item.TmdbId && x.PlexUserId == item.PlexUserId);
        if (exists) return BadRequest("Already in wishlist");

        _context.WishlistItems.Add(item);
        await _context.SaveChangesAsync();
        return Ok();
    }
}