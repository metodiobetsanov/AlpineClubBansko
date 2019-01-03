using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Routes.Components
{
    public class CreateRouteComment : ViewComponent
    {
        public CreateRouteComment()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new CreateCommentInputModel();

            return View(model);
        }
    }
}