using AlpineClubBansko.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly UserManager<User> userManager;

        public BaseController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public User CurrentUser => this.userManager.GetUserAsync(this.User).Result;

        public string CurrentController => RouteData.Values["controller"].ToString();

        public void AddUserNotification(string message)
        {
            this.TempData["notification"] = new string[2] { "primary", message };
        }

        public void AddSuccessNotification(string message)
        {
            this.TempData["notification"] = new string[2] { "success", message };
        }

        public void AddWarningNotification(string message)
        {
            this.TempData["notification"] = new string[2] { "warning", message };
        }

        public void AddDangerNotification(string message)
        {
            this.TempData["notification"] = new string[2] { "danger", message };
        }
    }
}