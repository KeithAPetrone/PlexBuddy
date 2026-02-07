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

    [HttpGet("all")] // For Admin
    public async Task<ActionResult<IEnumerable<MediaItem>>> GetAll() 
        => await _context.WishlistItems.ToListAsync();

    [HttpGet("{userId}")] // For User
    public async Task<ActionResult<IEnumerable<MediaItem>>> GetUserList(string userId) 
        => await _context.WishlistItems.Where(x => x.PlexUserId == userId).ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] MediaItem item)
    {
        var exists = await _context.WishlistItems.AnyAsync(x => x.TmdbId == item.TmdbId && x.PlexUserId == item.PlexUserId);
        if (exists) return BadRequest("Already in wishlist");

        _context.WishlistItems.Add(item);
        await _context.SaveChangesAsync();
        return Ok();
    }
}