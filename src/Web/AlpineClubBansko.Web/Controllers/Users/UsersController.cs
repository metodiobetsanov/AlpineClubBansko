using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Users
{
    public class UsersController : BaseController
    {
        private readonly IUsersService userService;

        public UsersController(IUsersService userService,
            UserManager<User> userManager)
            : base(userManager)
        {
            this.userService = userService;
        }

        public IActionResult Profile(string id)
        {
            UserProfileViewModel model = this.userService.GetUserById(id);

            return View(model);
        }
    }
}