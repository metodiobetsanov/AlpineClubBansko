using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.RouteViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums.ViewComponents
{
    public class RouteOptions : ViewComponent
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public RouteOptions(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(RouteViewModel model = null)
        {
            if (model != null)
            {
                if (signInManager.IsSignedIn(UserClaimsPrincipal))
                {
                    if (model.Author.Id == userManager.GetUserId(UserClaimsPrincipal))
                    {
                        return View("Author", model);
                    }

                    return View("UserRoutesDetails");
                }

                return View("GuestRoutesDetails");
            }

            if (signInManager.IsSignedIn(UserClaimsPrincipal))
            {
                return View("UserRoutesIndex");
            }

            return View("GuestRoutesIndex");
        }
    }
}