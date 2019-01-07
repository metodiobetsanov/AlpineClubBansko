using AlpineClubBansko.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly UserManager<User> userManager;

        protected BaseController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public User CurrentUser => this.userManager.GetUserAsync(this.User).Result;

        public string CurrentController => RouteData.Values["controller"].ToString();

        public void AddUserNotification(string message)
        {
            this.TempData["notification"] = new string[] { "primary", message };
        }

        public void AddSuccessNotification(string message)
        {
            this.TempData["notification"] = new string[] { "success", message };
        }

        public void AddWarningNotification(string message)
        {
            this.TempData["notification"] = new string[] { "warning", message };
        }

        public void AddDangerNotification(string message)
        {
            this.TempData["notification"] = new string[] { "danger", message };
        }
    }
}