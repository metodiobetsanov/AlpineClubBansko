using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    public interface ICloudService
    {
        Task<bool> UploadImage(IFormFile file, PhotoViewModel model);

        Task<bool> DeleteImage(string photoId);

        Task CreateContainer(string name);

        Task<bool> DeleteContainer(string name);
    }
}
