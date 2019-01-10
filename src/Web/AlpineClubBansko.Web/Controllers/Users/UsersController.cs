using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.UserViewModels;
using AlpineClubBansko.Web.Controllers;
using MagicStrings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AlpineClubBansko.Web.Controllers.Users
{
    public class UsersController : BaseController
    {
        private readonly IUsersService userService;
        private readonly ILogger<UsersController> logger;

        public UsersController(IUsersService userService,
            UserManager<User> userManager,
            ILogger<UsersController> logger)
            : base(userManager)
        {
            this.logger = logger;
            this.userService = userService;
            this.CurrentController = this.GetType().Name;
        }

        public IActionResult Profile(string id)
        {
            try
            {
                UserProfileViewModel model = this.userService.GetUserByIdAsViewModel(id);
                if (model == null)
                {
                    AddWarningNotification(Notifications.NotFound);
                    return Redirect("/");
                }
                return View(model);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(string.Format(Notifications.Fail));
                return Redirect("/");
            }
        }
    }
}