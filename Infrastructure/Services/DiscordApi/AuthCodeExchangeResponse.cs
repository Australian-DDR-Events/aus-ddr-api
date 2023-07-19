using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Infrastructure.Services.DiscordApi;

public class AuthCodeExchangeResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
        
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
        
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
        
    [JsonPropertyName("refresh_token")] 
    #nullable enable
    public string? RefreshToken { get; set; }
    #nullable disable
        
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}