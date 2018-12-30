using AlpineClubBansko.Services.Models.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.ViewComponents.Users
{
    public class UserProfile : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(UserProfileViewModel model)
        {
            return View(model);
        }
    }
}