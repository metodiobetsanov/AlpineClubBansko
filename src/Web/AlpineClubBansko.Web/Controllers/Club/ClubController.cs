using AlpineClubBansko.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Club
{
    public class ClubController : BaseController
    {
        public ClubController(UserManager<User> userManager)
            : base(userManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}