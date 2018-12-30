using AlpineClubBansko.Services.Models.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.ViewComponents.Users
{
    public class UserStories : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(UserProfileViewModel model)
        {
            var posts = model.Stories;

            return View(posts);
        }
    }
}