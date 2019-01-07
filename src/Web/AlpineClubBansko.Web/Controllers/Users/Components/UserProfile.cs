using AlpineClubBansko.Services.Models.UserViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Users.Components
{
    public class UserProfile : ViewComponent
    {
        public IViewComponentResult Invoke(UserProfileViewModel model)
        {
            return View(model);
        }
    }
}