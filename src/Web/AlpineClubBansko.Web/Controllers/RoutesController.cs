using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers
{
    public class RoutesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}