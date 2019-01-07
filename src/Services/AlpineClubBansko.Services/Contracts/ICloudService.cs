using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    public interface ICloudService
    {
        IEnumerable<PhotoViewModel> GetAllPhotosAsViewModels();

        Task<bool> UploadImage(IFormFile file, PhotoViewModel model);

        Task<string> UploadAvatar(IFormFile file, string id);

        Task<bool> DeleteImage(string photoId);

        Task<bool> CreateContainer(string name);

        Task<bool> DeleteContainer(string name);
    }
}