using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Common;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IRepository<Album> albumRepository;
        private readonly IRepository<AlbumComment> albumCommentRepository;
        private readonly IRepository<LikedAlbums> likedAlbumsRepository;
        private readonly ICloudService cloudService;

        public AlbumService(IRepository<Album> albumRepository,
            IRepository<AlbumComment> albumCommentRepository,
            IRepository<LikedAlbums> likedAlbumsRepository,
            ICloudService cloudService)
        {
            this.albumRepository = albumRepository;
            this.albumCommentRepository = albumCommentRepository;
            this.likedAlbumsRepository = likedAlbumsRepository;
            this.cloudService = cloudService;
        }

        public IQueryable<Album> GetAllAlbums()
        {
            return this.albumRepository.All();
        }

        public IEnumerable<AlbumViewModel> GetAllAlbumsAsViewModels()
        {
            return this.albumRepository.All().To<AlbumViewModel>();
        }

        public Album GetAlbumById(string albumId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(albumId, nameof(albumId));

            return this.albumRepository.GetById(albumId);
        }

        public AlbumViewModel GetAlbumByIdAsViewModel(string albumId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(albumId, nameof(albumId));

            return this.GetAllAlbumsAsViewModels().FirstOrDefault(s => s.Id == albumId);
        }

        public async Task<string> CreateAsync(string title, User user)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(title, nameof(title));

            ArgumentValidator.ThrowIfNull(user, nameof(user));

            Album album = new Album
            {
                Title = title,
                Author = user,
                CreatedOn = DateTime.UtcNow
            };

            await this.albumRepository.AddAsync(album);
            var result = await this.albumRepository.SaveChangesAsync();

            if (result != 0)
            {
                await this.cloudService.CreateContainer(album.Id);
            }

            return album.Id;
        }

        public async Task<bool> UpdateAsync(AlbumViewModel model)
        {
            ArgumentValidator.ThrowIfNull(model, nameof(model));

            Album album = this.albumRepository.GetById(model.Id);

            album.Title = model.Title;
            album.Content = model.Content;
            album.Place = model.Place;
            album.ModifiedOn = DateTime.UtcNow;

            this.albumRepository.Update(album);
            var result = await this.albumRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> DeleteAsync(string albumId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(albumId, nameof(albumId));

            Album album = this.albumRepository.GetById(albumId);

            if (album.Photos != null)
            {
                foreach (var item in album.Photos)
                {
                    await this.cloudService.DeleteImage(item.Id);
                }
            }

            if (album.Comments != null)
            {
                foreach (var item in album.Comments)
                {
                    this.albumCommentRepository.Delete(item);
                }
            }

            if (album.Favorite != null)
            {
                foreach (var item in album.Favorite)
                {
                    this.likedAlbumsRepository.Delete(item);
                }
            }

            this.albumRepository.Delete(album);
            var result = await this.albumRepository.SaveChangesAsync();

            if (result != 0)
            {
                await this.cloudService.DeleteContainer(albumId);
            }

            return result != 0;
        }

        public async Task<bool> CreateCommentAsync(string albumId, string content, User user)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(albumId, nameof(albumId));
            ArgumentValidator.ThrowIfNullOrEmpty(content, nameof(content));
            ArgumentValidator.ThrowIfNull(user, nameof(user));

            var comment = new AlbumComment
            {
                AuthorId = user.Id,
                AlbumId = albumId,
                Comment = content,
                CreatedOn = DateTime.UtcNow
            };

            await this.albumCommentRepository.AddAsync(comment);
            var result = await this.albumCommentRepository.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> DeleteCommentAsync(string commentId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(commentId, nameof(commentId));

            var item = this.albumCommentRepository.GetById(commentId);

            this.albumCommentRepository.Delete(item);
            var result = await this.albumCommentRepository.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> AddViewedAsync(string albumId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(albumId, nameof(albumId));

            Album album = this.albumRepository.GetById(albumId);

            album.Views += 1;

            this.albumRepository.Update(album);
            var changed = await this.albumRepository.SaveChangesAsync();

            return changed != 0;
        }

        public async Task<bool> FavoriteAsync(string albumId, User user)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(albumId, nameof(albumId));
            ArgumentValidator.ThrowIfNull(user, nameof(user));

            if (this.likedAlbumsRepository.All()
                .Any(f => f.UserId == user.Id && f.AlbumId == albumId))
            {
                var item = this.likedAlbumsRepository.All()
                    .FirstOrDefault(f => f.UserId == user.Id && f.AlbumId == albumId);

                this.likedAlbumsRepository.Delete(item);
            }
            else
            {
                await this.likedAlbumsRepository.AddAsync(new LikedAlbums
                {
                    UserId = user.Id,
                    AlbumId = albumId,
                });
            }
            var changed = await this.likedAlbumsRepository.SaveChangesAsync();

            return changed != 0;
        }
    }
}