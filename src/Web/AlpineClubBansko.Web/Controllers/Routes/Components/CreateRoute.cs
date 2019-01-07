using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Routes.Components
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