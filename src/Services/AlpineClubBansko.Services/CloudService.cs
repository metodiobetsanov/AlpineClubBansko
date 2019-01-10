using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services
{
    public class CloudService : ICloudService
    {
        private readonly IRepository<Photo> photoRepository;
        private readonly AzureStorageConfig storageConfig;

        public CloudService(IRepository<Photo> photoRepository,
            IOptions<AzureStorageConfig> config)
        {
            this.photoRepository = photoRepository;
            this.storageConfig = config.Value;
        }

        public IQueryable<Photo> GetAllPhotos()
        {
            return this.photoRepository.All();
        }

        public IEnumerable<PhotoViewModel> GetAllPhotosAsViewModels()
        {
            return this.photoRepository.All().To<PhotoViewModel>();
        }

        public async Task<bool> UploadImage(IFormFile file, PhotoViewModel model)
        {
            bool isUploaded = false;
            int counter = model.Album.Photos == null ? 0 : model.Album.Photos.Count();
            string albumId = model.Album.Id;

            if (this.IsImage(file) && file.Length > 0)
            {
                var name = $"{albumId}-{++counter}.{file.FileName.Split(".").Last()}";

                using (var stream = file.OpenReadStream())
                {
                    isUploaded = await this.UploadImageToStorage(
                        stream,
                        name,
                        albumId
                    );
                }

                if (isUploaded)
                {
                    var photo = new Photo
                    {
                        Title = model.Title,
                        Content = model.Content,
                        Photographer = model.Photographer,
                        Album = model.Album,
                        Author = model.Author,
                        CreatedOn = DateTime.UtcNow,
                        LocationUrl = $"https://acbimagestorage.blob.core.windows.net/{albumId}/{name}",
                        ThumbnailUrl = $"https://acbimagestorage.blob.core.windows.net/{albumId}/thumbnail_{name}",
                    };

                    await this.photoRepository.AddAsync(photo);
                    await this.photoRepository.SaveChangesAsync();
                }
            }

            return isUploaded;
        }

        public async Task<string> UploadAvatar(IFormFile file, string id)
        {
            var blobClient = this.GetClient();
            var name = $"{id}.{file.FileName.Split(".").Last()}";
            var container = blobClient.GetContainerReference("avatars");

            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            if (this.IsImage(file) && file.Length > 0)
            {
                using (var fileStream = file.OpenReadStream())
                {
                    byte[] avatar;

                    using (var image = new MagickImage(fileStream))
                    {
                        MagickGeometry size = new MagickGeometry(500);
                        size.IgnoreAspectRatio = false;
                        image.Resize(size);
                        image.Quality = 100;
                        avatar = image.ToByteArray();
                    }

                    var imageBlob = container.GetBlockBlobReference(name);
                    await imageBlob.UploadFromByteArrayAsync(avatar, 0, avatar.Count());

                    imageBlob = container.GetBlockBlobReference(name);
                    if (await imageBlob.ExistsAsync())
                    {
                        return $"https://acbimagestorage.blob.core.windows.net/avatars/{name}";
                    }
                }
            }

            return null;
        }

        public async Task<bool> DeleteImage(string photoId)
        {
            bool isDeleted = false;

            Photo photo = this.photoRepository.GetById(photoId);

            isDeleted = await this.DeleteImageFromStorage(photo);

            if (isDeleted)
            {
                this.photoRepository.Delete(photo);
                await this.photoRepository.SaveChangesAsync();
            }

            return isDeleted;
        }

        public async Task<bool> CreateContainer(string name)
        {
            try
            {
                var blobClient = this.GetClient();

                var container = blobClient.GetContainerReference(name);

                await container.CreateAsync();
                await container.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteContainer(string name)
        {
            var blobClient = this.GetClient();

            var container = blobClient.GetContainerReference(name);

            return await container.DeleteIfExistsAsync();
        }

        private bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image")) return true;

            string[] formats = { ".jpg", ".png", ".gif", ".jpeg", ".svg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> UploadImageToStorage(Stream fileStream, string fileName, string albumName)
        {
            var blobClient = this.GetClient();

            var container = blobClient.GetContainerReference(albumName);

            var imageBlob = container.GetBlockBlobReference(fileName);
            var thumbnailBlob = container.GetBlockBlobReference($"thumbnail_{fileName}");

            byte[] thumbnail;

            using (var image = new MagickImage(fileStream))
            {
                MagickGeometry size = new MagickGeometry(500);
                size.IgnoreAspectRatio = false;
                image.Resize(size);
                image.Quality = 100;
                thumbnail = image.ToByteArray();
            }

            fileStream.Position = 0;

            await imageBlob.UploadFromStreamAsync(fileStream);

            await thumbnailBlob.UploadFromByteArrayAsync(thumbnail, 0, thumbnail.Count());

            return await Task.FromResult(true);
        }

        private async Task<bool> DeleteImageFromStorage(Photo photo)
        {
            var blobClient = this.GetClient();

            var container = blobClient.GetContainerReference(photo.Album.Id);

            var image = container.GetBlockBlobReference(photo.LocationUrl.Split("/").Last());

            await image.DeleteIfExistsAsync();

            var thumbnail = container.GetBlockBlobReference(photo.ThumbnailUrl.Split("/").Last());

            await thumbnail.DeleteIfExistsAsync();

            return await Task.FromResult(true);
        }

        private CloudBlobClient GetClient()
        {
            var storageCredentials = new StorageCredentials(this.storageConfig.AccountName, storageConfig.AccountKey);

            var storageAccount = new CloudStorageAccount(storageCredentials, true);

            var blobClient = storageAccount.CreateCloudBlobClient();

            return blobClient;
        }
    }
}