using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EFLearn.Data
{
    public static class SeedData
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            if (!await roleManager.RoleExistsAsync("Manager"))
                await roleManager.CreateAsync(new IdentityRole("Manager"));
        }

        public static async Task SeedAdmin(UserManager<IdentityUser> userManager)
        {
            var adminEmail = "admin@test.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin@123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            //AddUserManager
            var EmailManager = "manager@Gmail.com";
            var ManagerExist = await userManager.FindByEmailAsync(EmailManager);
            if (ManagerExist == null)
            {
                var managerUser = new IdentityUser();
                managerUser.UserName = EmailManager;
                managerUser.Email = EmailManager;
                managerUser.EmailConfirmed = true;
                await userManager.CreateAsync(managerUser, "Manager@123");
                await userManager.AddToRoleAsync(managerUser, "Manager");
            }
        }
    }
}