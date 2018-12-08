using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers
{
    public class AlbumsController : Controller
    {
        public IActionResult Index()
        {
           return View();
        }
    }
}