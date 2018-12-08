using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}