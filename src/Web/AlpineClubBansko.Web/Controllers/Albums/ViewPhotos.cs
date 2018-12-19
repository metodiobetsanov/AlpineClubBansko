using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums
{
    public class ViewPhotos : ViewComponent
    {
        public ViewPhotos()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(List<PhotoViewModel> list)
        {
            return View(list);
        }
    }
}