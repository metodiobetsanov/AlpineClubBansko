using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Activities
{
    public class ActivitiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}