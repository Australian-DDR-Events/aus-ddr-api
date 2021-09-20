using System;
using Application.Core.Interfaces;

namespace Infrastructure.Identity
{
    public class LocalIdentity : IIdentity<string>
    {
        public bool IsAdmin(string source)
        {
            return source.Equals("admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}