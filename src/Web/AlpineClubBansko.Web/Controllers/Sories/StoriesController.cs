using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Extensions;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.StoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Stories
{
    public class StoriesController : Controller
    {
        private readonly IStoryService storyService;
        private readonly UserManager<User> userManager;

        public StoriesController(IStoryService storyService,
            UserManager<User> userManager)
        {
            this.storyService = storyService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            List<StoryViewModel> list = this.storyService.GetAllStories().ToList();

            list.ForEach(i => i.Content = i.Content.StorySubstring(300));

            return View(list);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(StoryViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                User user = await this.userManager.GetUserAsync(User);
                string storyId = await storyService.CreateAsync(model, user);

                return Redirect($"/Stories/Read/{storyId}");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Read(string id)
        {
            StoryViewModel model = this.storyService.GetStoryById(id);

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Update(string id)
        {
            StoryViewModel model = this.storyService.GetStoryById(id);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(StoryViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                string storyId = await storyService.UpdateAsync(model);

                return Redirect($"/Stories/Read/{storyId}");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await storyService.DeleteAsync(id);

            return Redirect($"/Stories");
        }
    }
}