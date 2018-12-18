using AlpineClubBansko.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums
{
    public class UpdatePhoto : ViewComponent
    {
        public UpdatePhoto()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(List<Photo> list)
        {
            return View(list);
        }
    }
}