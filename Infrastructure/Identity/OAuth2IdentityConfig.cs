namespace Infrastructure.Identity
{
    public record OAuth2IdentityConfig
    {
        public string UserinfoEndpoint { get; init; }
        public string TokenEndpoint { get; init; }
        public string ClientId { get; init; }
        public string ClientSecret { get; init; }
        public string RedirectUri { get; init; }
        public string Audience { get; init; }
        public string Issuer { get; init; }
    };
}