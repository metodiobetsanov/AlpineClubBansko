using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IRepository<Photo> photoRepository;
        private readonly IAlbumService albumService;
        private readonly AzureStorageConfig storageConfig;

        public PhotoService(IRepository<Photo> photoRepository,
            IAlbumService albumService,
            IOptions<AzureStorageConfig> config)
        {
            this.photoRepository = photoRepository;
            this.albumService = albumService;
            this.storageConfig = config.Value;
        }

        public async Task<bool> UploadImages(IFormFile file, PhotoViewModel model)
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
                        LocationUrl = $"https://acbimagestorage.blob.core.windows.net/{albumId}/{name}",
                        ThumbnailUrl = $"https://acbimagestorage.blob.core.windows.net/{albumId}/thumbnail_{name}",
                    };

                    await this.photoRepository.AddAsync(photo);
                    await this.photoRepository.SaveChangesAsync();
                }
            }

            return isUploaded;
        }

        private bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image")) return true;

            string[] formats = { ".jpg", ".png", ".gif", ".jpeg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> UploadImageToStorage(Stream fileStream, string fileName, string albumName)
        {
            var blobClient = this.GetClient();

            var container = blobClient.GetContainerReference(albumName);

            if (!(await container.ExistsAsync()))
            {
                await container.CreateAsync();
                await container.SetPermissionsAsync(
                    new BlobContainerPermissions()
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
            }

            var imageBlob = container.GetBlockBlobReference(fileName);
            var thumbnailBlob = container.GetBlockBlobReference($"thumbnail_{fileName}");

            byte[] thumbnail;

            using (var image = new MagickImage(fileStream))
            {
                MagickGeometry size = new MagickGeometry(200);
                size.IgnoreAspectRatio = true;

                image.Resize(size);
                image.Quality = 75;
                thumbnail = image.ToByteArray();
            }

            fileStream.Position = 0;

            await imageBlob.UploadFromStreamAsync(fileStream);
            await thumbnailBlob.UploadFromByteArrayAsync(thumbnail, 0, thumbnail.Count());

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

