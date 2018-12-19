using System.Linq;
using System.Threading.Tasks;
using AlpineClubBansko.Services.Models.UserViewModels;
using Microsoft.AspNetCore.Mvc;

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