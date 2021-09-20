using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Attributes
{
    public class AdminAttribute : TypeFilterAttribute
    {
        public AdminAttribute() : base(typeof(AdminFilter))
        {
            
        }
    }
}