using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AlpineClubBansko.Web.Controllers.Users.Components
{
    public class UserAlbums : ViewComponent
    {
        public IViewComponentResult Invoke(List<AlbumViewModel> model)
        {
            return View(model.OrderBy(a => a.Views).ToList());
        }
    }
}