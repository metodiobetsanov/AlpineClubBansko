using AlpineClubBansko.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Areas.Manage.Controllers.Admins
{
    [Area("Manage")]
    public class AdminController : BaseController
    {
        public AdminController(UserManager<User> userManager)
               : base(userManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}