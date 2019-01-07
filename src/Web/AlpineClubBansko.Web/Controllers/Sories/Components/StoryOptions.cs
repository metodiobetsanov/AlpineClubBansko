using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.StoryViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Stories.Components
{
    public class StoryOptions : ViewComponent
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public StoryOptions(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke(StoryViewModel model = null)
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

                    return View("UserStoriesDetails", model);
                }

                return View("GuestStoriesDetails", model);
            }

            if (signInManager.IsSignedIn(UserClaimsPrincipal))
            {
                return View("UserStoriesIndex");
            }

            return View("GuestStoriesIndex");
        }
    }
}