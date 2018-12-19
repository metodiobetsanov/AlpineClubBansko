using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.UserViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Users
{
    public class UsersController : Controller
    {
        private readonly IUsersService userService;

        public UsersController(IUsersService userService)
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