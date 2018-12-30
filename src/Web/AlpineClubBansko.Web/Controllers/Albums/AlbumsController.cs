using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums
{
    public class AlbumsController : BaseController
    {
        private readonly IAlbumService albumService;
        private readonly ICloudService cloudService;

        public AlbumsController(IAlbumService albumService,
            ICloudService cloudService,
            UserManager<User> userManager)
            : base(userManager)
        {
            this.albumService = albumService;
            this.cloudService = cloudService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<AlbumViewModel> list = albumService.GetAllAlbums().ToList();

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
        public async Task<IActionResult> Create(AlbumViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                string albumId = await this.albumService.CreateAsync(model, CurrentUser);

                return Redirect($"/Albums/Details/{albumId}");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            AlbumViewModel model = this.albumService.GetAlbumById(id);

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Update(string id)
        {
            AlbumViewModel model = this.albumService.GetAlbumById(id);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(AlbumViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                string albumId = await this.albumService.UpdateAsync(model);

                return Redirect($"/Albums/Read/{albumId}");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await this.albumService.DeleteAsync(id);

            return Redirect($"/Albums");
        }

        [HttpPost]
        [Authorize]
        public async Task<ViewComponentResult> UploadPhoto(IFormFile file, PhotoViewModel model, string albumId)
        {
            bool isUploaded = false;

            if (ModelState.IsValid)
            {
                model.Author = CurrentUser;
                model.Album = this.albumService.GetAlbum(albumId);

                isUploaded = await this.cloudService.UploadImage(file, model);
            }

            AlbumViewModel album = this.albumService.GetAlbumById(albumId);

            return ViewComponent("ViewPhotos", album.Photos);
        }

        [HttpGet]
        [Authorize]
        public async Task<ViewComponentResult> DeletePhoto(string photoId, string albumId)
        {
            bool isDeleted = false;

            isDeleted = await this.cloudService.DeleteImage(photoId);

            AlbumViewModel album = this.albumService.GetAlbumById(albumId);

            return ViewComponent("UpdatePhoto", album.Photos);
        }
    }
}