using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models;
using AlpineClubBansko.Services.Models.HomeViewModels;
using MagicStrings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AlpineClubBansko.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;
        private readonly ILogger<HomeController> logger;

        public HomeController(IHomeService homeService,
            UserManager<User> userManager,
            ILogger<HomeController> logger)
            : base(userManager)
        {
            this.logger = logger;
            this.homeService = homeService;
            this.CurrentController = this.GetType().Name;
        }

        public IActionResult Index()
        {
            try
            {
                HomeViewModel model = this.homeService.GetHomeViewModel();
                return View(model);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(string.Format(Notifications.Fail));
                return Redirect("/Error");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}