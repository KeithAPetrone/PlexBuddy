namespace PlexBuddy.Shared;

public class PlexUser
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Thumb { get; set; } = string.Empty; // Avatar URL
    public string AuthToken { get; set; } = string.Empty;
}