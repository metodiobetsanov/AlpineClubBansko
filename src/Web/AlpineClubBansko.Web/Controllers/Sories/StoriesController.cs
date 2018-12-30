using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.StoryViewModels;
using AlpineClubBansko.Web.Models;
using MagicStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Stories
{
    public class StoriesController : BaseController
    {
        private readonly IStoryService storyService;
        private readonly ILogger<StoriesController> logger;

        public StoriesController(IStoryService storyService,
            UserManager<User> userManager,
            ILogger<StoriesController> logger,
            HtmlEncoder htmlEncoder)
            : base(userManager)
        {
            this.htmlEncoder = htmlEncoder;
            this.logger = logger;
            this.storyService = storyService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            try
            {
                List<StoryViewModel> list = this.storyService.GetAllStories().ToList();
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
        public async Task<IActionResult> Create(CreateStoryInputModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    string storyId = await storyService.CreateAsync(model.Title, CurrentUser);

                    logger.LogInformation(
                        string.Format(SetLog.CreatedSuccess,
                            CurrentUser.UserName,
                            CurrentController,
                            storyId
                            ));

                    AddWarningNotification(string.Format(Notifications.CreatedSuccess, model.Title));
                    return Redirect($"/Stories/Update/{storyId}");
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

                return Redirect("/Stories");
            }
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                StoryViewModel model = this.storyService.GetStoryById(id);

                return View(model);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));
                AddDangerNotification(string.Format(Notifications.Fail));

                return Redirect("/Stories");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Update(string id)
        {
            try
            {
                StoryViewModel model = this.storyService.GetStoryById(id);

                return View(model);
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(string.Format(Notifications.Fail));

                return Redirect("/Stories");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(StoryViewModel model)
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

                Redirect($"/Stories/Details/{model.Id}");
            }

            try
            {
                if (this.ModelState.IsValid)
                {
                    await storyService.UpdateAsync(model);

                    logger.LogInformation(
                        string.Format(SetLog.UpdateSuccess,
                            CurrentUser.UserName,
                            CurrentController,
                            model.Id
                            ));

                    AddSuccessNotification(string.Format(Notifications.UpdateSuccess, model.Title));

                    return Redirect($"/Stories/Details/{model.Id}");
                }
                else
                {
                    AddDangerNotification(string.Format(Notifications.Fail));
                    return Redirect($"/Stories");
                }
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(string.Format(Notifications.Fail));

                return Redirect("/Stories");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (!CurrentUser.Stories.Any(s => s.Id == id))
            {
                logger.LogInformation(
                        string.Format(SetLog.NotTheAuthor,
                            CurrentUser.UserName,
                            CurrentController,
                            id
                            ));

                AddDangerNotification(string.Format(Notifications.DeleteFail));

                Redirect($"/Stories/Details/{id}");
            }

            try
            {
                await storyService.DeleteAsync(id);

                logger.LogInformation(
                        string.Format(SetLog.Delete,
                            CurrentUser.UserName,
                            CurrentController,
                            id
                            ));

                AddSuccessNotification(Notifications.Delete);

                return Redirect($"/Stories");
            }
            catch (System.Exception e)
            {
                logger.LogError(string.Format(SetLog.Error,
                            CurrentUser.UserName,
                            CurrentController,
                            e.Message));

                AddDangerNotification(Notifications.Fail);

                return Redirect($"/Stories");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment(string id, string content)
        {
            try
            {
                await this.storyService.CreateComment(id, content, CurrentUser);

                logger.LogInformation(
                        string.Format(SetLog.CreatedSuccess,
                            CurrentUser.UserName,
                            CurrentController,
                            $"StoryCommentFor-{id}"
                            ));

                var model = storyService.GetStoryById(id);
                return PartialView("_Comments", model.StoryComments.ToList());
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
        public async Task<IActionResult> DeleteComment(string id, string storyId)
        {
            try
            {
                await this.storyService.DeleteComment(id);

                logger.LogInformation(
                        string.Format(SetLog.Delete,
                            CurrentUser.UserName,
                            CurrentController,
                            $"StoryCommentFor-{id}"
                            ));

                var model = storyService.GetStoryById(storyId);
                return PartialView("_Comments", model.StoryComments.ToList());
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
        public IActionResult FilterStories(string searchCriteria = null,
            string sortOrder = null,
            int page = 1)
        {
            try
            {
                List<StoryViewModel> list = this.storyService.GetAllStories().ToList();
                return ViewComponent("ViewStories",
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
        public async Task<bool> AddViewed(string id)
        {
            try
            {
                bool result = await this.storyService.AddViewed(id);

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
        public async Task<bool> Favorite(string id)
        {
            try
            {
                bool result = await this.storyService.Favorite(id, CurrentUser);

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
        public IActionResult RefreshStats(string id)
        {
            try
            {
                var model = this.storyService.GetStoryById(id);

                return ViewComponent("StoryOptions", model);
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