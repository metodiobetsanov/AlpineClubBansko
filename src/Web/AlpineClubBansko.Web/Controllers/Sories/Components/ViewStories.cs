﻿using AlpineClubBansko.Services.Extensions;
using AlpineClubBansko.Services.Models.StoryViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Stories.ViewComponents
{
    public class ViewStories : ViewComponent
    {
        public ViewStories()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(List<StoryViewModel> model,
            string searchCriteria,
            string sortOrder,
            int page)
        {
            if (model != null)
            {
                if (!string.IsNullOrEmpty(searchCriteria))
                {
                    model = model.Where(s =>
                        s.Title.ToLower().Contains(searchCriteria.ToLower()) ||
                        s.Content.ToLower().Contains(searchCriteria.ToLower())
                        ).ToList();
                }

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    this.TempData["CurrentSortParam"] = sortOrder;
                }
                else if (string.IsNullOrEmpty(sortOrder) && this.TempData.ContainsKey("CurrentSortParam"))
                {
                    sortOrder = this.TempData.Peek("CurrentSortParam").ToString();
                }

                switch (sortOrder)
                {
                    case "title_Asc":
                        model = model.OrderBy(s => s.Title).ToList();
                        break;

                    case "title_Desc":
                        model = model.OrderByDescending(s => s.Title).ToList();
                        break;

                    case "date_Asc":
                        model = model.OrderBy(s => s.CreatedOn).ToList();
                        break;

                    case "rate_Asc":
                        model = model.OrderBy(s => s.Favorite.Count).ToList();
                        break;

                    case "rate_Desc":
                        model = model.OrderByDescending(s => s.Favorite.Count).ToList();
                        break;

                    default:
                        model = model.OrderByDescending(s => s.CreatedOn).ToList();
                        break;
                }

                int size = 6;
                int firstPage = 1;
                int lastPage = (int)Math.Ceiling(model.Count / (double)size);
                this.ViewData["totalPages"] = lastPage == 0 ? 1 : lastPage;
                if (page <= 0)
                {
                    page = firstPage;
                }

                if (page > lastPage)
                {
                    page = lastPage;
                }

                model = model.Skip((page - 1) * (size)).Take(size).ToList();
                this.ViewData["page"] = page;
                model.ForEach(i => i.Content = i.Content?.StorySubstring(300));

                return View(model);
            }

            return View();
        }
    }
}