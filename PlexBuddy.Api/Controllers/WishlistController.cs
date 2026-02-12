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
        => await _context.WishlistItems.OrderByDescending(x => x.RequestedAt).ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] MediaItem item)
    {
        // Prevent duplicates by TMDB ID
        var exists = await _context.WishlistItems.AnyAsync(x => x.TmdbId == item.TmdbId);
        if (exists) return BadRequest("Item already in wishlist.");

        _context.WishlistItems.Add(item);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.WishlistItems.FindAsync(id);
        if (item == null) return NotFound();

        _context.WishlistItems.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}