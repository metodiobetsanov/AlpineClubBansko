using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Web.Controllers.Albums
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumService albumService;
        private readonly IPhotoService photoService;
        private readonly UserManager<User> userManager;

        public AlbumsController(IAlbumService albumService,
            IPhotoService photoService,
            UserManager<User> userManager)
        {
            this.albumService = albumService;
            this.photoService = photoService;
            this.userManager = userManager;
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
                User user = await this.userManager.GetUserAsync(User);
                string albumId = await this.albumService.CreateAsync(model, user);

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

        [HttpGet]
        [Authorize]
        public IActionResult Upload(string id) {

            var model = this.albumService.GetAlbumById(id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files,  string id)
        {
            bool isUploaded = false;

            User user = await this.userManager.GetUserAsync(User);
            Album album = this.albumService.GetAlbum(id);
            try
            {
                foreach (var file in files)
                {
                    isUploaded = await this.photoService.UploadImages(file, album, user);
                }

                if (!isUploaded)
                {
                    return this.View();
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}