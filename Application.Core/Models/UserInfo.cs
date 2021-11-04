using System.Text.Json.Serialization;

namespace Application.Core.Models
{
    public class UserInfo
    {
        [JsonPropertyName("sub")]
        public string UserId { get; set; }
        [JsonPropertyName("nickname")]
        public string Name { get; set; }
        [JsonPropertyName("custom:legacy_id")]
        public string LegacyId { get; set; }
    }
}