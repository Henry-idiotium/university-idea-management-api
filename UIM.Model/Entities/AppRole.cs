using Microsoft.AspNetCore.Identity;

namespace UIM.Model.Entities
{
    public class AppRole : IdentityRole
    {
        public string Description { get; set; }
    }
}