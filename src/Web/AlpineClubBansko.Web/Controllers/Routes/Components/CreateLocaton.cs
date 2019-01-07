using AlpineClubBansko.Services.Models.RouteViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Routes.Components
{
    public class CreateLocation : ViewComponent
    {
        public CreateLocation()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new LocationViewModel();
            return View(model);
        }
    }
}