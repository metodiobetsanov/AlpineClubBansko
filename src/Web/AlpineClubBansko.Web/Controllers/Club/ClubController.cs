using AlpineClubBansko.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlpineClubBansko.Web.Controllers.Club
{
    public class ClubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
