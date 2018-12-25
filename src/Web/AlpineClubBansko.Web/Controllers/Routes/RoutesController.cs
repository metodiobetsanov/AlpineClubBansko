using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.RouteViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Routes
{
    public class RoutesController : Controller
    {
        readonly IRouteService routeService;

        public RoutesController(IRouteService routeService)
        {
            this.routeService = routeService;
        }

        public IActionResult Index()
        {
            var model = this.routeService.GetAllRoutes().ToList();

            return View(model);
        }

        public JsonResult GetListCoords()
        {
            var result = new List<Location> {
                new Location()
                {
                    Latitude = 42.0100311m,
                    Longitude = 42.0100311m
                },
                new Location()
                {
                    Latitude = 41.0100311m,
                    Longitude = 41.0100311m
                },
            };

            return Json(result);
        }
    
        public ViewComponentResult SearchInRoutes(List<RouteViewModel> list, string searchCriteria = null)
        {
            return ViewComponent("ViewRoutes", new { list, searchCriteria });
        }
    }
}