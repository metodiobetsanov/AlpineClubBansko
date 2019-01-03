using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums.Components
{
    public class CreateAlbum : ViewComponent
    {
        public CreateAlbum()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new CreateAlbumInputModel();

            return View(model);
        }
    }
}