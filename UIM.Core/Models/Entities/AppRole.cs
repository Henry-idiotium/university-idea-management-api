using Microsoft.AspNetCore.Identity;

namespace UIM.Core.Models.Entities
{
    public class AppRole : IdentityRole
    {
        public string Description { get; set; }
    }
}