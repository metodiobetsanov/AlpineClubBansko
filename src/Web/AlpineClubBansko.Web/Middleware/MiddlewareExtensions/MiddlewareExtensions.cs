using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Middleware.MiddlewareExtensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSeedDataMiddleware(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedDataMiddleware>();
        }
    }
}
