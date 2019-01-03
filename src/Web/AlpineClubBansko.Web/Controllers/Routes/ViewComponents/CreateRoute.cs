using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums.ViewComponents
{
    public class CreateRoute : ViewComponent
    {
        public CreateRoute()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new CreateRouteInputModel();
            return View(model);
        }
    }
}