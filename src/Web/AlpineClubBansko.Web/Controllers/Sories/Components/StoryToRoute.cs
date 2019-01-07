using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Routes.Components
{
    public class StoryToRoute : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public StoryToRoute(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string storyId)
        {
            ConnectRouteAndStoryInputModel model = new ConnectRouteAndStoryInputModel();
            model.StoryId = storyId;

            User user = await this.userManager.GetUserAsync(this.UserClaimsPrincipal);

            this.TempData["routeList"] = user.Routes.ToList();

            return View(model);
        }
    }
}