using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AusDdrApi.Services.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>();
            String username;
            try
            {
                if (AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]).Scheme != "Basic")
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header");
                }

                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]).Parameter;
                var credentialBytes = Convert.FromBase64String(authHeader);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] {':'}, 2);
                username = credentials[0];

                var rand = new Random(username.GetHashCode());
                var guid = new byte[16];
                rand.NextBytes(guid);
                claims.Add(new Claim("sub", new Guid(guid).ToString()));
                claims.Add(new Claim("name", username));
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            switch (username)
            {
                case "admin":
                    claims.Add(new Claim("admin", "true"));
                    break;
                default:
                    break;
            }
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}