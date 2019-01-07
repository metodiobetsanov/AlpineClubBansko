using AlpineClubBansko.Services.Models.StoryViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AlpineClubBansko.Web.Controllers.Users.Components
{
    public class UserStories : ViewComponent
    {
        public IViewComponentResult Invoke(List<StoryViewModel> model)
        {
            return View(model.OrderBy(s => s.Views).ToList());
        }
    }
}