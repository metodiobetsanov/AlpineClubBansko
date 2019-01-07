using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Common;
using AlpineClubBansko.Services.Contracts;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services
{
    public class ConnectService : IConnectService
    {
        private readonly IRepository<Album> albumRepository;
        private readonly IRepository<Route> routeRepository;
        private readonly IRepository<Story> storyRepository;

        public ConnectService(IRepository<Album> albumRepository,
            IRepository<Route> routeRepository,
            IRepository<Story> storyRepository)
        {
            this.storyRepository = storyRepository;
            this.routeRepository = routeRepository;
            this.albumRepository = albumRepository;
        }

        public async Task<bool> ConnectAlbumAndRoute(string albumId, string routeId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(albumId, nameof(albumId));
            ArgumentValidator.ThrowIfNullOrEmpty(routeId, nameof(routeId));

            Album album = this.albumRepository.GetById(albumId);
            album.RouteId = routeId;

            this.albumRepository.Update(album);
            int result = await this.albumRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> DisconnectAlbumAndRoute(string albumId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(albumId, nameof(albumId));

            Album album = this.albumRepository.GetById(albumId);
            album.RouteId = null;

            this.albumRepository.Update(album);
            int result = await this.albumRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> ConnectAlbumAndStory(string albumId, string storyId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(albumId, nameof(albumId));
            ArgumentValidator.ThrowIfNullOrEmpty(storyId, nameof(storyId));

            Album album = this.albumRepository.GetById(albumId);
            album.StoryId = storyId;

            this.albumRepository.Update(album);
            int result = await this.albumRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> DisconnectAlbumAndStory(string albumId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(albumId, nameof(albumId));

            Album album = this.albumRepository.GetById(albumId);
            album.StoryId = null;

            this.albumRepository.Update(album);
            int result = await this.albumRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> ConnectStoryAndRoute(string storyId, string routeId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(storyId, nameof(storyId));
            ArgumentValidator.ThrowIfNullOrEmpty(routeId, nameof(routeId));

            Story story = this.storyRepository.GetById(storyId);
            story.RouteId = routeId;

            this.storyRepository.Update(story);
            int result = await this.storyRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> DisconnectStoryAndRoute(string storyId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(storyId, nameof(storyId));

            Story story = this.storyRepository.GetById(storyId);
            story.RouteId = null;

            this.storyRepository.Update(story);
            int result = await this.storyRepository.SaveChangesAsync();

            return result != 0;
        }
    }
}