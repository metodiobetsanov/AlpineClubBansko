using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums.Components
{
    public class CreateAlbumComment : ViewComponent
    {
        public CreateAlbumComment()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new CreateCommentInputModel();

            return View(model);
        }
    }
}