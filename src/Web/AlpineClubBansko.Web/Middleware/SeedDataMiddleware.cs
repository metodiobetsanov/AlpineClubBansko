﻿using AlpineClubBansko.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Middleware
{
    public class SeedDataMiddleware
    {
        private readonly RequestDelegate next;
        private bool seedDone = false;

        public SeedDataMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (!seedDone)
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
                    User user = new User()
                    {
                        UserName = "Owner",
                        Email = "owner@this.web"
                    };

                    var result = await userManager.CreateAsync(user, "0wnerPassw0rd");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRolesAsync(user, new[] { "Owner", "Administrator" });
                        seedDone = true;
                    }
                }
            }

            await this.next(context);
        }
    }
}
