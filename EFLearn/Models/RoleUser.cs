using Microsoft.AspNetCore.Identity;

namespace EFLearn.Models
{
    public class RoleUser
    {
        public List<IdentityRole>? Roles { get; set; }
        public List<IdentityUser>? Users { get; set; }

    }
}
