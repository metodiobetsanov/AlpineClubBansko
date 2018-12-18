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
        private readonly ICloudService cloudService;

        public AlbumService(IRepository<Album> albumRepository,
            ICloudService cloudService)
        {
            this.cloudService = cloudService;
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

            await this.cloudService.CreateContainer(album.Id);

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

            if (album.Photos != null || album.Photos.Count > 0)
            {
                foreach (var photo in album.Photos.ToList())
                {
                    await this.cloudService.DeleteImage(photo.Id);
                }
            }

            await this.cloudService.DeleteContainer(id);

            this.albumRepository.Delete(album);

            return await this.albumRepository.SaveChangesAsync();
        }
    }
}