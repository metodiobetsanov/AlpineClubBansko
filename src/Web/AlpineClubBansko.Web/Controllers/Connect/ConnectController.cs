using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Web.Models;
using MagicStrings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Connect
{
    public class ConnectController : BaseController
    {
        private readonly IConnectService connectService;
        private readonly ILogger<ConnectController> logger;

        public ConnectController(IConnectService connectService,
            ILogger<ConnectController> logger,
            UserManager<User> userManager)
            : base(userManager)
        {
            this.logger = logger;
            this.connectService = connectService;
        }

        //Connect and Disconnect for Route
        public async Task<IActionResult> ConnectRouteAndAlbum(ConnectRouteAndAlbumInputModel model)
        {
            try
            {
                bool result = await this.connectService.ConnectAlbumAndRoute(model.AlbumId, model.RouteId);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Routes/Details/{model.RouteId}");
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

        public async Task<IActionResult> DisconnectRouteAndAlbum(string id, string routeId)
        {
            try
            {
                bool result = await this.connectService.DisconnectAlbumAndRoute(id);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Routes/Details/{routeId}");
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

        public async Task<IActionResult> ConnectRouteAndStory(ConnectRouteAndStoryInputModel model)
        {
            try
            {
                bool result = await this.connectService.ConnectStoryAndRoute(model.StoryId, model.RouteId);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Routes/Details/{model.RouteId}");
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

        public async Task<IActionResult> DisconnectRouteAndStory(string id, string routeId)
        {
            try
            {
                bool result = await this.connectService.DisconnectStoryAndRoute(id);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Routes/Details/{routeId}");
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

        //Connect and Disconnect for Album
        public async Task<IActionResult> ConnectAlbumAndRoute(ConnectRouteAndAlbumInputModel model)
        {
            try
            {
                bool result = await this.connectService.ConnectAlbumAndRoute(model.AlbumId, model.RouteId);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Albums/Details/{model.AlbumId}");
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

        public async Task<IActionResult> DisconnectAlbumAndRoute(string id)
        {
            try
            {
                bool result = await this.connectService.DisconnectAlbumAndRoute(id);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Albums/Details/{id}");
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

        public async Task<IActionResult> ConnectAlbumAndStory(ConnectAlbumAndStoryInputModel model)
        {
            try
            {
                bool result = await this.connectService.ConnectAlbumAndStory(model.AlbumId, model.StoryId);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Albums/Details/{model.AlbumId}");
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

        public async Task<IActionResult> DisconnectAlbumAndStory(string id)
        {
            try
            {
                bool result = await this.connectService.DisconnectAlbumAndStory(id);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Albums/Details/{id}");
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

        //Connect and Disconnect for Story

        public async Task<IActionResult> ConnectStoryAndRoute(ConnectRouteAndStoryInputModel model)
        {
            try
            {
                bool result = await this.connectService.ConnectStoryAndRoute(model.StoryId, model.RouteId);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Stories/Details/{model.StoryId}");
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

        public async Task<IActionResult> DisconnectStoryAndRoute(string id)
        {
            try
            {
                bool result = await this.connectService.DisconnectStoryAndRoute(id);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Stories/Details/{id}");
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

        public async Task<IActionResult> ConnectStoryAndAlbum(ConnectAlbumAndStoryInputModel model)
        {
            try
            {
                bool result = await this.connectService.ConnectAlbumAndStory(model.AlbumId, model.StoryId);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Stories/Details/{model.StoryId}");
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

        public async Task<IActionResult> DisconnectStoryAndAlbum(string id, string storyId)
        {
            try
            {
                bool result = await this.connectService.DisconnectAlbumAndStory(id);

                if (result)
                {
                    AddSuccessNotification(Notifications.Success);
                }
                else
                {
                    AddDangerNotification(Notifications.Fail);
                }

                return Redirect($"/Stories/Details/{storyId}");
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
    }
}