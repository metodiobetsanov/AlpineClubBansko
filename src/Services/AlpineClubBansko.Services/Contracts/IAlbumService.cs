using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IAlbumService
    {
        IQueryable<Album> GetAllAlbums();

        IEnumerable<AlbumViewModel> GetAllAlbumsAsViewModels();

        Album GetAlbumById(string albumId);

        AlbumViewModel GetAlbumByIdAsViewModel(string albumId);

        Task<string> CreateAsync(string title, User user);

        Task<bool> UpdateAsync(AlbumViewModel model);

        Task<bool> DeleteAsync(string albumId);

        Task<bool> AddViewedAsync(string albumId);

        Task<bool> FavoriteAsync(string albumId, User user);

        Task<bool> CreateCommentAsync(string albumId, string content, User user);

        Task<bool> DeleteCommentAsync(string commentId);
    }
}