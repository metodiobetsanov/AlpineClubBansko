using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Routes.Components
{
    public class RouteToStory : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public RouteToStory(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string routeId)
        {
            ConnectRouteAndStoryInputModel model = new ConnectRouteAndStoryInputModel();
            model.RouteId = routeId;

            User user = await this.userManager.GetUserAsync(this.UserClaimsPrincipal);

            this.TempData["storyList"] = user.Stories.ToList();

            return View(model);
        }
    }
}