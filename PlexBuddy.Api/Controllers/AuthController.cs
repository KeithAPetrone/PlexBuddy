using Microsoft.AspNetCore.Mvc;
using PlexWishlist.Api.Services;

namespace PlexWishlist.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly PlexAuthService _plexService;

        public AuthController(PlexAuthService plexService)
        {
            _plexService = plexService;
        }

        [HttpPost("login-start")]
        public async Task<IActionResult> StartLogin()
        {
            var pin = await _plexService.GetPinAsync();
            
            // Generate the URL the frontend should redirect the user to
            var authUrl = $"https://app.plex.tv/auth#?clientID=Your-App-GUID-Here&code={pin.Code}&context[device][product]=MyWishlistApp";
            
            return Ok(new { PinId = pin.Id, AuthUrl = authUrl });
        }

        [HttpGet("login-check/{pinId}")]
        public async Task<IActionResult> CheckLogin(string pinId)
        {
            var token = await _plexService.CheckPinAsync(pinId);
            
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Message = "User has not authorized yet." });
            }

            // SUCCESS! Now get user details
            var user = await _plexService.GetUserAsync(token);

            // TODO: Here is where you would issue your OWN JWT token for the session
            // For now, we return the Plex User details
            return Ok(new { Token = token, User = user });
        }
    }
}