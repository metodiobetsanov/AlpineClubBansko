using AlpineClubBansko.Services.Models.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.ViewComponents.Users
{
    public class UserAlbums : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(UserProfileViewModel model)
        {
            var albums = model.Albums;

            return View(albums);
        }
    }
}