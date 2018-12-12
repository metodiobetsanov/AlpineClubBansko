using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IRepository<Album> albumRepository;

        public AlbumService(IRepository<Album> albumRepository)
        {
            this.albumRepository = albumRepository;
        }

        public IEnumerable<AlbumViewModel> GetAllAlbums()
        {
            return this.albumRepository.All().To<AlbumViewModel>();
        }

        public Album GetAlbum(string id)
        {
            return this.albumRepository.GetById(id);
        }

        public AlbumViewModel GetAlbumById(string id)
        {
            return this.GetAllAlbums().FirstOrDefault(a => a.Id == id);
        }

        public async Task<string> CreateAsync(AlbumViewModel model, User user)
        {
            var album = new Album
            {
                Title = model.Title,
                Content = model.Content,
                Author = user
            };

            await this.albumRepository.AddAsync(album);
            await this.albumRepository.SaveChangesAsync();

            return album.Id;
        }

        public async Task<string> UpdateAsync(AlbumViewModel model)
        {
            Album album = this.albumRepository.GetById(model.Id);

            album.Title = model.Title;
            album.Content = model.Content;
            album.ModifiedOn = DateTime.UtcNow;
        
            this.albumRepository.Update(album);
            await this.albumRepository.SaveChangesAsync();

            return album.Id;
        }

        public async Task<int> DeleteAsync(string id)
        {
            Album album = this.albumRepository.GetById(id);

            this.albumRepository.Delete(album);

            return await this.albumRepository.SaveChangesAsync();
        }

        //public async Task<bool> UploadImages(ICollection<IFormFile> files, string id)
        //{
        //    var isUploaded = false;
        //    var counter = GetById(id).Photos.Count();

        //    foreach (var formFile in files)
        //        if (_uploadService.IsImage(formFile))
        //            if (formFile.Length > 0)
        //            {
        //                var name = $"{id}-{++counter}.{formFile.FileName.Split(".")[1]}";

        //                using (var stream = formFile.OpenReadStream())
        //                {
        //                    isUploaded = await _uploadService.UploadFileToStorage(
        //                        stream,
        //                        name,
        //                        _storageConfig
        //                    );
        //                }

        //                var photo = new Photo
        //                {
        //                    AlbumId = id,
        //                    LocationUrl = $"https://alpineclubbanskofs.blob.core.windows.net/images/{name}"
        //                };

        //                await _context.Photos.AddAsync(photo);
        //                await _context.SaveChangesAsync();
        //            }

        //    return isUploaded;
        //}
    }
}