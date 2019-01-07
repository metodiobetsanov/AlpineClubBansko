using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums.Components
{
    public class AlbumToStory : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public AlbumToStory(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string albumId)
        {
            ConnectAlbumAndStoryInputModel model = new ConnectAlbumAndStoryInputModel();
            model.AlbumId = albumId;

            User user = await this.userManager.GetUserAsync(this.UserClaimsPrincipal);

            this.TempData["storyList"] = user.Stories.ToList();

            return View(model);
        }
    }
}