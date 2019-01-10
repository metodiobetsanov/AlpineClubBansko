using AlpineClubBansko.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AlpineClubBansko.Web.Areas.Manage.Controllers.Admins
{
    [Area("Manage")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IUsersService usersService;
        private readonly IRouteService routeService;
        private readonly IStoryService storyService;
        private readonly IAlbumService albumService;

        public AdminController(IAdminService adminService,
            IUsersService usersService,
             IRouteService routeService,
             IStoryService storyService,
             IAlbumService albumService)
        {
            this.storyService = storyService;
            this.albumService = albumService;
            this.routeService = routeService;
            this.usersService = usersService;
            this.adminService = adminService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = this.adminService.GetWebData();

            return View(model);
        }

        [HttpGet]
        public IActionResult Users()
        {
            var model = this.usersService
                .GetAllUsersAsViewModels()
                .Where(u => u.UserName != "Owner")
                .ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Routes()
        {
            var model = this.routeService
                .GetAllRoutesAsViewModels()
                .Where(r => string.IsNullOrEmpty(r.Content) ||
                r.Locations.Count == 0)
                .ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Stories()
        {
            var model = this.storyService
                .GetAllStoriesAsViewModels()
                .Where(r => string.IsNullOrEmpty(r.Content))
                .ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Albums()
        {
            var model = this.albumService
                .GetAllAlbumsAsViewModels()
                .Where(r => string.IsNullOrEmpty(r.Content) ||
                r.Photos.Count == 0)
                .ToList();

            return View(model);
        }
    }
}