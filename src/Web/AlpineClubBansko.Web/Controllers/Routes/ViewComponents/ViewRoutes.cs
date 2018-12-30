using AlpineClubBansko.Services.Models.RouteViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums.ViewComponents
{
    public class ViewRoutes : ViewComponent
    {
        public ViewRoutes()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(List<RouteViewModel> list, string searchCriteria = null)
        {
            if (!string.IsNullOrEmpty(searchCriteria)
                && !string.IsNullOrWhiteSpace(searchCriteria))
            {
                list = list.Where(r =>
                r.Title.Contains(searchCriteria)
                || r.Content.Contains(searchCriteria)
                || r.Locations.Any(l => l.Name == searchCriteria)
                ).ToList();

                return View(list);
            }

            return View(list.OrderByDescending(r => r.CreatedOn).ToList());
        }
    }
}