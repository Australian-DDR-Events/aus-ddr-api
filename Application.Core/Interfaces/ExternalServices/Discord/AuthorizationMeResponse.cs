using JetBrains.Annotations;

namespace Application.Core.Interfaces.ExternalServices.Discord;

public class AuthorizationMeResponse
{
    public UserPortion? User { get; set; }

    public class UserPortion
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
    }
}