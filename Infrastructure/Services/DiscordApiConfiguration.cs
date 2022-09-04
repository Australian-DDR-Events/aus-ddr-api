namespace Infrastructure.Services;

public class DiscordApiConfiguration
{
    public string DiscordApiEndpoint { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RedirectUri { get; set; }
}