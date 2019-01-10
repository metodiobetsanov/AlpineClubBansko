using AlpineClubBansko.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Middleware
{
    public class SeedDataMiddleware
    {
        private readonly RequestDelegate next;

        public SeedDataMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (!(await roleManager.RoleExistsAsync("Owner")))
            {
                await roleManager.CreateAsync(
                    new IdentityRole("Owner")
                    );
            }

            if (!(await roleManager.RoleExistsAsync("Administrator")))
            {
                await roleManager.CreateAsync(
                    new IdentityRole("Administrator")
                    );
            }

            if (!(await roleManager.RoleExistsAsync("User")))
            {
                await roleManager.CreateAsync(
                    new IdentityRole("User")
                    );
            }

            if (await userManager.FindByEmailAsync("owner@this.web") == null)
            {
                User user = new User
                {
                    UserName = "Owner",
                    Email = "owner@this.web",
                    EmailConfirmed = true,
                    Avatar = "https://acbimagestorage.blob.core.windows.net/avatars/avatar.svg"
                };

                var result = await userManager.CreateAsync(user, "0wnerPassw0rd");

                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, new[] { "Owner", "Administrator" });
                }
            }

            await this.next(context);
        }
    }
}