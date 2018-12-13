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
    public interface IPhotoService
    {
        Task<bool> UploadImages(IFormFile file, PhotoViewModel model);
    }
}
