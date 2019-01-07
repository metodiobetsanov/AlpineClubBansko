using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Routes.Components
{
    public class RouteToAlbum : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public RouteToAlbum(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string routeId)
        {
            ConnectRouteAndAlbumInputModel model = new ConnectRouteAndAlbumInputModel();
            model.RouteId = routeId;

            User user = await this.userManager.GetUserAsync(this.UserClaimsPrincipal);

            this.TempData["albumList"] = user.Albums.ToList();

            return View(model);
        }
    }
}