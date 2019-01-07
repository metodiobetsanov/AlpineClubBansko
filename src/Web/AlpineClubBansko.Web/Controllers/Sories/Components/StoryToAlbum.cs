using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Routes.Components
{
    public class StoryToAlbum : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public StoryToAlbum(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string storyId)
        {
            ConnectAlbumAndStoryInputModel model = new ConnectAlbumAndStoryInputModel();
            model.StoryId = storyId;

            User user = await this.userManager.GetUserAsync(this.UserClaimsPrincipal);

            this.TempData["albumList"] = user.Albums.ToList();

            return View(model);
        }
    }
}