namespace Infrastructure.Identity
{
    public record OAuth2IdentityConfig
    {
        public string UserinfoEndpoint { get; init; }
        public string Audience { get; init; }
        public string Issuer { get; init; }
    };
}