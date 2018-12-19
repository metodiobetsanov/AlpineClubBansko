using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using AlpineClubBansko.Services.Models.UserViewModels;
using Microsoft.AspNetCore.Mvc;

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