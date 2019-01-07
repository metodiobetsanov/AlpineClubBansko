using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums.Components
{
    public class AlbumToRoute : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public AlbumToRoute(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string albumId)
        {
            ConnectRouteAndAlbumInputModel model = new ConnectRouteAndAlbumInputModel();
            model.AlbumId = albumId;

            User user = await this.userManager.GetUserAsync(this.UserClaimsPrincipal);

            this.TempData["routeList"] = user.Routes.ToList();

            return View(model);
        }
    }
}