using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.RouteViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Routes.Components
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

        public IViewComponentResult Invoke(RouteViewModel model = null)
        {
            if (model != null)
            {
                if (signInManager.IsSignedIn(UserClaimsPrincipal))
                {
                    if (model.Author.Id == userManager.GetUserId(UserClaimsPrincipal)
                        || User.IsInRole("Administrator"))
                    {
                        return View("Author", model);
                    }

                    return View("UserRoutesDetails", model);
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