using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums.Components
{
    public class UploadPhoto : ViewComponent
    {
        public UploadPhoto(IAlbumService albumService)
        {

        }

        public IViewComponentResult Invoke(string albumId)
        {
            PhotoViewModel model = new PhotoViewModel()
            {
                AlbumId = albumId
            };

            return View(model);
        }
    }
}