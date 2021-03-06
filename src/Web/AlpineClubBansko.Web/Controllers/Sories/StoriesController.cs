﻿using AlpineClubBansko.Data.Models;
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
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Stories
{
    public class StoriesController : BaseController
    {
        private readonly IStoryService storyService;
        private readonly ILogger<StoriesController> logger;

        public StoriesController(IStoryService storyService,
            UserManager<User> userManager,
            ILogger<StoriesController> logger)
            : base(userManager)
        {
            this.logger = logger;
            this.storyService = storyService;
            this.CurrentController = this.GetType().Name;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            try
            {
                List<StoryViewModel> list = this.storyService.GetAllStoriesAsViewModels().ToList();
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
                    if (string.IsNullOrEmpty(storyId))
                    {
                        AddDangerNotification(string.Format(Notifications.CreatedFail, model.Title));
                        return Redirect("/Stories");
                    }

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
                StoryViewModel model = this.storyService.GetStoryByIdAsViewModel(id);

                if (model == null)
                {
                    AddWarningNotification(Notifications.NotFound);
                    return Redirect("/");
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

                return Redirect("/Stories");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Update(string id)
        {
            if (!CurrentUser.Stories.Any(r => r.Id == id))
            {
                if (!User.IsInRole("Administrator"))
                {
                    logger.LogInformation(
                        string.Format(SetLog.NotTheAuthor,
                            CurrentUser.UserName,
                            CurrentController,
                            id
                            ));

                    AddDangerNotification(string.Format(Notifications.OnlyAuthor));

                    return Redirect($"/Stories/Details/{id}");
                }
            }
            try
            {
                StoryViewModel model = this.storyService.GetStoryByIdAsViewModel(id);

                if (model == null)
                {
                    AddWarningNotification(Notifications.NotFound);
                    return Redirect("/Stories");
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

                return Redirect("/Stories");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(StoryViewModel model)
        {
            if (!CurrentUser.Stories.Any(r => r.Id == model.Id))
            {
                if (!User.IsInRole("Administrator"))
                {
                    logger.LogInformation(
                        string.Format(SetLog.NotTheAuthor,
                            CurrentUser.UserName,
                            CurrentController,
                            model.Id
                            ));

                    AddDangerNotification(string.Format(Notifications.NotTheAuthor, model.Title));

                    return Redirect($"/Stories/Details/{model.Id}");
                }
            }
            try
            {
                if (this.ModelState.IsValid)
                {
                    model.Content = model.Content.Replace("script", "");

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
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (!CurrentUser.Stories.Any(r => r.Id == id))
            {
                if (!User.IsInRole("Administrator"))
                {
                    logger.LogInformation(
                        string.Format(SetLog.NotTheAuthor,
                            CurrentUser.UserName,
                            CurrentController,
                            id
                            ));

                    AddDangerNotification(string.Format(Notifications.DeleteFail));

                    return Redirect($"/Stories/Details/{id}");
                }
            }

            try
            {
                var result = await storyService.DeleteAsync(id);

                if (!result)
                {
                    AddDangerNotification(Notifications.Fail);
                    return Redirect("/Stories");
                }

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
        public async Task<IActionResult> CreateComment(string storyId, string content)
        {
            try
            {
                await this.storyService.CreateCommentAsync(storyId, content, CurrentUser);

                logger.LogInformation(
                        string.Format(SetLog.CreatedSuccess,
                            CurrentUser.UserName,
                            CurrentController,
                            $"StoryCommentFor-{storyId}"
                            ));

                StoryViewModel model = storyService.GetStoryByIdAsViewModel(storyId);
                return PartialView("_StoryComments", model.Comments);
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
        public async Task<IActionResult> DeleteComment(string commentId, string storyId)
        {
            try
            {
                await this.storyService.DeleteCommentAsync(commentId);

                logger.LogInformation(
                        string.Format(SetLog.Delete,
                            CurrentUser.UserName,
                            CurrentController,
                            $"StoryCommentFor-{storyId}"
                            ));

                StoryViewModel model = storyService.GetStoryByIdAsViewModel(storyId);
                return PartialView("_StoryComments", model.Comments);
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
        public IActionResult FilterStories(string searchCriteria,
            string sortOrder,
            int page)
        {
            try
            {
                List<StoryViewModel> list = this.storyService.GetAllStoriesAsViewModels().ToList();
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
        public async Task<bool> AddViewed(string storyId)
        {
            try
            {
                bool result = await this.storyService.AddViewedAsync(storyId);

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
        public async Task<bool> Favorite(string storyId)
        {
            try
            {
                bool result = await this.storyService.FavoriteAsync(storyId, CurrentUser);

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
        public IActionResult RefreshStats(string storyId)
        {
            try
            {
                StoryViewModel model = this.storyService.GetStoryByIdAsViewModel(storyId);

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