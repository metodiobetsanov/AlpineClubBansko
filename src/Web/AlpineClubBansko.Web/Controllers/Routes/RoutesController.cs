using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Routes
{
    public class RoutesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}