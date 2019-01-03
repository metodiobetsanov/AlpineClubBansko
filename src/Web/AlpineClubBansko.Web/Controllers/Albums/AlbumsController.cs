using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using AlpineClubBansko.Web.Models;
using MagicStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums
{
    public class AlbumsController : BaseController
    {
        private readonly IAlbumService albumService;
        private readonly ICloudService cloudService;
        private readonly ILogger<AlbumsController> logger;

        public AlbumsController(IAlbumService albumService,
            ICloudService cloudService,
            UserManager<User> userManager,
            ILogger<AlbumsController> logger)
            : base(userManager)
        {
            this.logger = logger;
            this.albumService = albumService;
            this.cloudService = cloudService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                List<AlbumViewModel> list = albumService.GetAllAlbumsAsViewModels().ToList();
                return View(list);
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateAlbumInputModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    string albumId = await this.albumService.CreateAsync(model.Title, CurrentUser);

                    if (string.IsNullOrEmpty(albumId))
                    {
                        AddDangerNotification(string.Format(Notifications.CreatedFail, model.Title));
                        return Redirect("/Albums");
                    }

                    logger.LogInformation(
                        string.Format(SetLog.CreatedSuccess,
                            CurrentUser.UserName,
                            CurrentController,
                            albumId
                            ));

                    AddWarningNotification(string.Format(Notifications.CreatedSuccess, model.Title));
                    return Redirect($"/Albums/Update/{albumId}");
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

                return Redirect("/Albums");
            }
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                AlbumViewModel model = this.albumService.GetAlbumByIdAsViewModel(id);

                if (model == null)
                {
                    AddWarningNotification(Notifications.NotFound);
                    return Redirect("/Albums");
                }

                return View(model);
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
            try
            {
                AlbumViewModel model = this.albumService.GetAlbumByIdAsViewModel(id);
                if (model == null)
                {
                    AddWarningNotification(Notifications.NotFound);
                    return Redirect("/Albums");
                }
                return View(model);
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
        public async Task<IActionResult> Update(AlbumViewModel model)
        {
            if (CurrentUser.Id != this.TempData["AuthorId"].ToString())
            {
                logger.LogInformation(
                        string.Format(SetLog.NotTheAuthor,
                            CurrentUser.UserName,
                            CurrentController,
                            model.Id
                            ));

                AddDangerNotification(string.Format(Notifications.NotTheAuthor, model.Title));

                Redirect($"/Albums/Details/{model.Id}");
            }

            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.albumService.UpdateAsync(model);

                    logger.LogInformation(
                        string.Format(SetLog.UpdateSuccess,
                            CurrentUser.UserName,
                            CurrentController,
                            model.Id
                            ));

                    AddSuccessNotification(string.Format(Notifications.UpdateSuccess, model.Title));

                    return Redirect($"/Albums/Details/{model.Id}");
                }

                return View(model);
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
        public async Task<IActionResult> Delete(string id)
        {
            if (!CurrentUser.Albums.Any(s => s.Id == id))
            {
                logger.LogInformation(
                        string.Format(SetLog.NotTheAuthor,
                            CurrentUser.UserName,
                            CurrentController,
                            id
                            ));

                AddDangerNotification(string.Format(Notifications.DeleteFail));

                Redirect($"/Albums/Details/{id}");
            }

            try
            { 
                await this.albumService.DeleteAsync(id);

                return Redirect($"/Albums");
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(Notifications.Fail);

                return Redirect($"/Albums");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ViewComponentResult> UploadPhoto(IFormFile file, PhotoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Author = CurrentUser;
                    model.Album = this.albumService.GetAlbumById(model.AlbumId);

                    var isUploaded = await this.cloudService.UploadImage(file, model);

                    if (!isUploaded) AddDangerNotification(Notifications.Fail);
                }

                AlbumViewModel album = this.albumService.GetAlbumByIdAsViewModel(model.AlbumId);

                return ViewComponent("ViewPhotos", new
                {
                    model = album.Photos,
                    page = 1
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
        [Authorize]
        public async Task<ViewComponentResult> DeletePhoto(string photoId, string albumId)
        {
            try
            {
                await this.cloudService.DeleteImage(photoId);

                AlbumViewModel album = this.albumService.GetAlbumByIdAsViewModel(albumId);

                return ViewComponent("ViewPhotos", new {
                    model = album.Photos,
                    page = 1});
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
        public async Task<IActionResult> CreateComment(string albumId, string content)
        {
            try
            {
                await this.albumService.CreateCommentAsync(albumId, content, CurrentUser);

                logger.LogInformation(
                        string.Format(SetLog.CreatedSuccess,
                            CurrentUser.UserName,
                            CurrentController,
                            $"AlbumCommentFor-{albumId}"
                            ));

                AlbumViewModel model = albumService.GetAlbumByIdAsViewModel(albumId);
                return PartialView("_AlbumComments", model.Comments);
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
        public async Task<IActionResult> DeleteComment(string commentId, string albumId)
        {
            try
            {
                await this.albumService.DeleteCommentAsync(commentId);

                logger.LogInformation(
                        string.Format(SetLog.Delete,
                            CurrentUser.UserName,
                            CurrentController,
                            $"AlbumCommentFor-{albumId}"
                            ));

                AlbumViewModel model = albumService.GetAlbumByIdAsViewModel(albumId);
                return PartialView("_AlbumComments", model.Comments);
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
        public IActionResult FilterAlbums(string searchCriteria,
            string sortOrder,
            int page)
        {
            try
            {
                List<AlbumViewModel> list = this.albumService.GetAllAlbumsAsViewModels().ToList();
                return ViewComponent("ViewAlbums",
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

        [HttpGet]
        public IActionResult FilterPhotos(int page, string albumId)
        {
            try
            {
                List<PhotoViewModel> list = this.albumService.GetAlbumByIdAsViewModel(albumId).Photos;
                return ViewComponent("UpdatePhotos",
                    new
                    {
                        model = list,
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
        public async Task<bool> AddViewed(string albumId)
        {
            try
            {
                bool result = await this.albumService.AddViewedAsync(albumId);

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
        [Authorize]
        public async Task<bool> Favorite(string albumId)
        {
            try
            {
                bool result = await this.albumService.FavoriteAsync(albumId, CurrentUser);

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
        public IActionResult RefreshStats(string albumId)
        {
            try
            {
                AlbumViewModel model = this.albumService.GetAlbumByIdAsViewModel(albumId);

                return ViewComponent("AlbumOptions", model);
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