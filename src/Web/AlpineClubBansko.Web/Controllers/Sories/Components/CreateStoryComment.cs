using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Stories.Components
{
    public class CreateStoryComment : ViewComponent
    {
        public CreateStoryComment()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new CreateCommentInputModel();

            return View(model);
        }
    }
}