using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Albums.Components
{
    public class AlbumOptions : ViewComponent
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AlbumOptions(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke(AlbumViewModel model = null)
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

                    return View("UserAlbumsDetails", model);
                }

                return View("GuestAlbumsDetails", model);
            }

            if (signInManager.IsSignedIn(UserClaimsPrincipal))
            {
                return View("UserAlbumsIndex");
            }

            return View("GuestAlbumsIndex");
        }
    }
}