using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Infrastructure.Identity;

public class TokenEndpointResponse
{
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }
    
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("refresh_token")]
    [CanBeNull]
    public string RefreshToken { get; set; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}