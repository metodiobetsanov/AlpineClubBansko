using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.RouteViewModels;
using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Routes
{
    public class RoutesController : BaseController
    {
        private readonly IRouteService routeService;

        public RoutesController(IRouteService routeService,
            UserManager<User> userManager)
            : base(userManager)
        {
            this.routeService = routeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = this.routeService.GetAllRoutes().ToList();
            return View(model);
        }

        [HttpGet]
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateRouteInputModel model)
        {
            if (ModelState.IsValid)
            {
                string routeId = await this.routeService.CreateRouteAsync(model.Title, CurrentUser);

                return Redirect($"/Routes/Update/{routeId}");
            }

            return Redirect("/Routes");
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var model = this.routeService.GetRouteById(id);

            return View(model);
        }

        [HttpGet]
        public IActionResult Update(string id)
        {
            var model = this.routeService.GetRouteById(id);

            return View(model);
        }

        [HttpGet]
        public ViewComponentResult SearchInRoutes(List<RouteViewModel> list, string searchCriteria = null)
        {
            return ViewComponent("ViewRoutes", new { list, searchCriteria });
        }
    }
}