using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums
{
    public class UploadPhoto : ViewComponent
    {
        private readonly IAlbumService albumService;

        public UploadPhoto(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string albumId)
        {
            Album album = this.albumService.GetAlbum(albumId);

            PhotoViewModel model = new PhotoViewModel()
            {
                Album = album
            };

            return View(model);
        }
    }
}
