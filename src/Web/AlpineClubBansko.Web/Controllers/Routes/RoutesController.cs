using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.RouteViewModels;
using AlpineClubBansko.Web.Models;
using MagicStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Routes
{
    public class RoutesController : BaseController
    {
        private readonly IRouteService routeService;
        private readonly ILogger<RoutesController> logger;

        public RoutesController(IRouteService routeService,
            UserManager<User> userManager,
            ILogger<RoutesController> logger)
            : base(userManager)
        {
            this.logger = logger;
            this.routeService = routeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                List<RouteViewModel> routeList = this.routeService.GetAllRoutesAsViewModels().ToList();
                return View(routeList);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(string.Format(Notifications.Fail));
                return Redirect("/");
            }
        }

        [HttpGet]
        public JsonResult GetListCoords()
        {
            try
            {
                var result = this.routeService
                    .GetAllRoutesAsViewModels()
                    .Where(l => l.Locations != null && l.Locations.Count > 0)
                    .Select(r => r.Locations.OrderBy(l => l.CreatedOn).Last())
                    .ToList();

                return Json(result);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                           CurrentUser.UserName,
                           CurrentController,
                           e.Message));

                return null;
            }
        }

        [HttpGet]
        public JsonResult GetListRouteCoords(string routeId)
        {
            try
            {
                var result = this.routeService
                    .GetRouteByIdAsViewModel(routeId)
                    .Locations
                    .OrderBy(l => l.CreatedOn)
                    .ToList();

                return Json(result);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                           CurrentUser.UserName,
                           CurrentController,
                           e.Message));

                return null;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateRouteInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string routeId = await this.routeService.CreateAsync(model.Title, CurrentUser);
                    if (string.IsNullOrEmpty(routeId))
                    {
                        AddDangerNotification(string.Format(Notifications.CreatedFail, model.Title));
                        return Redirect("/Routes");
                    }

                    logger.LogInformation(
                       string.Format(SetLog.CreatedSuccess,
                           CurrentUser.UserName,
                           CurrentController,
                           routeId
                           ));

                    AddWarningNotification(string.Format(Notifications.CreatedSuccess, model.Title));

                    return Redirect($"/Routes/Update/{routeId}");
                }
                else
                {
                    logger.LogInformation(string.Format(SetLog.CreatedFail,
                        CurrentUser.UserName,
                        CurrentController));

                    AddDangerNotification(string.Format(Notifications.CreatedFail, model.Title));
                    return View(model);
                }
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                        CurrentUser.UserName,
                        CurrentController,
                        e.Message));

                AddDangerNotification(string.Format(Notifications.Fail));

                return Redirect("/Routes");
            }
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                RouteViewModel route = this.routeService.GetRouteByIdAsViewModel(id);

                if (route == null)
                {
                    AddWarningNotification(Notifications.NotFound);
                    return Redirect("/Home/NotFound");
                }

                return View(route);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));
                AddDangerNotification(string.Format(Notifications.Fail));

                return Redirect("/Albums");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Update(string id)
        {
            if (!CurrentUser.Routes.Any(r => r.Id == id)
                && !User.IsInRole("Administrator"))
            {
                logger.LogInformation(
                        string.Format(SetLog.NotTheAuthor,
                            CurrentUser.UserName,
                            CurrentController,
                            id
                            ));

                AddDangerNotification(string.Format(Notifications.OnlyAuthor));

                Redirect($"/Routes/Details/{id}");
            }

            try
            {
                RouteViewModel route = this.routeService.GetRouteByIdAsViewModel(id);

                if (route == null)
                {
                    AddWarningNotification(Notifications.NotFound);
                    return Redirect("/Routes");
                }

                return View(route);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));
                AddDangerNotification(string.Format(Notifications.Fail));

                return Redirect("/Albums");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(RouteViewModel model)
        {
            if (!CurrentUser.Routes.Any(r => r.Id == model.Id)
                || !User.IsInRole("Administrator"))
            {
                logger.LogInformation(
                        string.Format(SetLog.NotTheAuthor,
                            CurrentUser.UserName,
                            CurrentController,
                            model.Id
                            ));

                AddDangerNotification(string.Format(Notifications.NotTheAuthor, model.Title));

                Redirect($"/Routes/Details/{model.Id}");
            }
            try
            {
                if (this.ModelState.IsValid)
                {
                    model.Content = model.Content.Replace("script", "");

                    await routeService.UpdateAsync(model);

                    logger.LogInformation(
                        string.Format(SetLog.UpdateSuccess,
                            CurrentUser.UserName,
                            CurrentController,
                            model.Id
                            ));

                    AddSuccessNotification(string.Format(Notifications.UpdateSuccess, model.Title));

                    return Redirect($"/Routes/Details/{model.Id}");
                }
                else
                {
                    AddDangerNotification(string.Format(Notifications.Fail));
                    return Redirect($"/Routes");
                }
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));
                AddDangerNotification(string.Format(Notifications.Fail));

                return Redirect("/Routes");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateLocation(LocationViewModel model)
        {
            try
            {
                model.RouteId = this.TempData.Peek("RouteId").ToString();
                await this.routeService.CreateLocationAsync(model, CurrentUser);

                RouteViewModel route = this.routeService.GetRouteByIdAsViewModel(model.RouteId);

                return PartialView("_Locations", route.Locations);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));
                AddDangerNotification(string.Format(Notifications.Fail));

                return Redirect("/Routes");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteLocation(string locationId, string routeId)
        {
            try
            {
                await this.routeService.DeleteLocationAsync(locationId);

                RouteViewModel route = this.routeService.GetRouteByIdAsViewModel(routeId);

                return PartialView("_Locations", route.Locations);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));
                AddDangerNotification(string.Format(Notifications.Fail));

                return Redirect($"/Routes/Details{routeId}");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (!CurrentUser.Routes.Any(s => s.Id == id))
            {
                logger.LogInformation(
                        string.Format(SetLog.NotTheAuthor,
                            CurrentUser.UserName,
                            CurrentController,
                            id
                            ));

                AddDangerNotification(string.Format(Notifications.DeleteFail));

                Redirect($"/Routes/Details/{id}");
            }

            try
            {
                var result = await routeService.DeleteAsync(id);

                if (!result)
                {
                    AddDangerNotification(Notifications.Fail);
                    return Redirect("/Routes");
                }

                logger.LogInformation(
                        string.Format(SetLog.Delete,
                            CurrentUser.UserName,
                            CurrentController,
                            id
                            ));

                AddSuccessNotification(Notifications.Delete);

                return Redirect($"/Routes");
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(Notifications.Fail);

                return Redirect($"/Routes");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment(string routeId, string content)
        {
            try
            {
                await this.routeService.CreateCommentAsync(routeId, content, CurrentUser);

                logger.LogInformation(
                        string.Format(SetLog.CreatedSuccess,
                            CurrentUser.UserName,
                            CurrentController,
                            $"StoryCommentFor-{routeId}"
                            ));

                RouteViewModel route = routeService.GetRouteByIdAsViewModel(routeId);
                return PartialView("_RouteComments", route.Comments);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(Notifications.Fail);

                return null;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteComment(string commentId, string routeId)
        {
            try
            {
                await this.routeService.DeleteCommentAsync(commentId);

                logger.LogInformation(
                        string.Format(SetLog.Delete,
                            CurrentUser.UserName,
                            CurrentController,
                            $"StoryCommentFor-{routeId}"
                            ));

                RouteViewModel route = routeService.GetRouteByIdAsViewModel(routeId);
                return PartialView("_RouteComments", route.Comments);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(Notifications.Fail);

                return null;
            }
        }

        [HttpGet]
        public IActionResult FilterRoutes(string searchCriteria,
            string sortOrder,
            int page)
        {
            try
            {
                List<RouteViewModel> list = this.routeService.GetAllRoutesAsViewModels().ToList();
                return ViewComponent("ViewRoutes",
                    new
                    {
                        model = list,
                        searchCriteria,
                        sortOrder,
                        page
                    });
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(Notifications.Fail);

                return null;
            }
        }

        [HttpPost]
        public async Task<bool> AddViewed(string routeId)
        {
            try
            {
                bool result = await this.routeService.AddViewedAsync(routeId);

                return result;
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(Notifications.Fail);

                return false;
            }
        }

        [HttpPost]
        public async Task<bool> Favorite(string routeId)
        {
            try
            {
                bool result = await this.routeService.FavoriteAsync(routeId, CurrentUser);

                return result;
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(Notifications.Fail);

                return false;
            }
        }

        [HttpGet]
        public IActionResult RefreshStats(string routeId)
        {
            try
            {
                RouteViewModel route = this.routeService.GetRouteByIdAsViewModel(routeId);

                return ViewComponent("RouteOptions", route);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(Notifications.Fail);

                return null;
            }
        }
    }
}