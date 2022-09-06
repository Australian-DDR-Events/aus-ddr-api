using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Attributes;

public class UserAuthAttribute : TypeFilterAttribute
{
    public UserAuthAttribute() : base(typeof(UserAuthFilter)) {
    }
}