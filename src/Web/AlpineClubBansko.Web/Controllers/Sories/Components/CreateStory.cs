﻿using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlpineClubBansko.Web.Controllers.Stories.Components
{
    public class CreateStory : ViewComponent
    {
        public CreateStory()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new CreateStoryInputModel();

            return View(model);
        }
    }
}