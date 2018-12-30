using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Events
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}