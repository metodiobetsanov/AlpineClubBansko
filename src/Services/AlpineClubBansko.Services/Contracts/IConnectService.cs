using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IConnectService
    {
        Task<bool> ConnectAlbumAndRoute(string albumId, string routeId);

        Task<bool> DisconnectAlbumAndRoute(string albumId);

        Task<bool> ConnectAlbumAndStory(string albumId, string storyId);

        Task<bool> DisconnectAlbumAndStory(string albumId);

        Task<bool> ConnectStoryAndRoute(string storyId, string routeId);

        Task<bool> DisconnectStoryAndRoute(string storyId);
    }
}