using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlpineClubBansko.Web.Controllers.Albums.Components
{
    public class ViewPhotos : ViewComponent
    {
        public ViewPhotos()
        {
        }

        public IViewComponentResult Invoke(List<PhotoViewModel> model, int page)
        {
            if (model != null)
            {
                model = model.OrderByDescending(p => p.CreatedOn).ToList();
                int size = 10;
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

                return View(model);
            }

            return View();
        }
    }
}