using AlpineClubBansko.Services.Models.RouteViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AlpineClubBansko.Web.Controllers.Users.Components
{
    public class UserRoutes : ViewComponent
    {
        public IViewComponentResult Invoke(List<RouteViewModel> model)
        {
            return View(model.OrderBy(s => s.Views).ToList());
        }
    }
}