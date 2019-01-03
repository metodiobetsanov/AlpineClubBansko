using AlpineClubBansko.Data;
using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace AlpineClubBansko.Services.Tests
{
    public class AlbumServiceTests
    {
        private readonly ApplicationDbContext context;
        private readonly IAlbumService service;
        private readonly IServiceProvider provider;
        private readonly IRepository<Album> albumRepo;

        public AlbumServiceTests()
        {
            var services = new ServiceCollection();
            var mockService = new Mock<ICloudService>();
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<ICloudService, CloudService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).Assembly
            );

            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ApplicationDbContext>();
            this.service = provider.GetService<IAlbumService>();
            this.albumRepo = provider.GetService<IRepository<Album>>();
        }
        [Fact]
        public void All_WithExistingData_ShouldReturnNull()
        {
            var albums = service.GetAllAlbums().ToList();

            albums.ShouldBeEmpty();

            var albumsAsViewModel = service.GetAllAlbumsAsViewModels().ToList();

            albumsAsViewModel.ShouldBeEmpty();
        }

        [Fact]
        public void All_WithExistingData_ShouldReturnOneExistingData()
        {
            User user = new User
            {
                UserName = "TestUser"
            };

            Album album = new Album()
            {
                Title = "Title",
                Author = user,
            };

            this.albumRepo.AddAsync(album);
            this.albumRepo.SaveChangesAsync();

            var count = this.albumRepo.All().Count();

            Assert.Equal(1, count);

            var albums = service.GetAllAlbums().ToList();

            albums.ShouldNotBeEmpty();

            albums.ShouldHaveSingleItem();

            var albumsAsViewModel = service.GetAllAlbumsAsViewModels().ToList();

            albumsAsViewModel.ShouldNotBeEmpty();

            albumsAsViewModel.ShouldHaveSingleItem();
        }

        [Fact]
        public void CreateAsync_ShouldReturnCreateAlbum_AndShouldReturnString()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            test.ShouldNotBeNullOrEmpty();

            test.ShouldBeOfType<string>();

            user.Albums.ShouldNotBeEmpty();

            user.Albums.ShouldHaveSingleItem();
        }

        [Fact]
        public void GetAll_ShouldReturnCorrectCount()
        {
            Random random = new Random();

            int count = random.Next(10, 100);

            User user = new User
            {
                UserName = "TestUser"
            };

            for (int i = 0; i < count; i++)
            {
                this.service.CreateAsync($"{i}", user).GetAwaiter().GetResult();
            }

            var repoCount = this.albumRepo.All().Count();

            Assert.Equal(count, repoCount);

            int allAlbumsCount = this.service.GetAllAlbums().Count();

            Assert.Equal(count, allAlbumsCount);

            int allAlbumsAsViewModelsCount = this.service.GetAllAlbumsAsViewModels().Count();

            Assert.Equal(count, allAlbumsAsViewModelsCount);
        }

        [Fact]
        public void GetAlbumById_ShouldReturnCorrectElement()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            var album = this.service.GetAlbumById(test);

            album.ShouldNotBeNull();

            Assert.Equal(album.Id, test);

            var albumAsViewModel = this.service.GetAlbumByIdAsViewModel(test);

            albumAsViewModel.ShouldNotBeNull();

            Assert.Equal(albumAsViewModel.Id, test);
        }

        [Fact]
        public void GetAlbumById_ShouldReturnNull()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            var story = this.service.GetAlbumById(new Guid().ToString());

            story.ShouldBeNull();

            var storyAsViewModel = this.service.GetAlbumByIdAsViewModel(new Guid().ToString());

            storyAsViewModel.ShouldBeNull();
        }

        [Fact]
        public void Update_ShouldWork()
        {
            string modelTitle = "TestCreate";
            string modelContent = "TestUpdateContentOne";
            string modelUpdateContent = "TestUpdateContentTwo";

            User user = new User
            {
                UserName = "TestUser"
            };

            string test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            AlbumViewModel modelUpdateOne = new AlbumViewModel
            {
                Id = test,
                Title = "One",
                Content = modelContent,
            };

            AlbumViewModel modelUpdateTwo = new AlbumViewModel
            {
                Id = test,
                Title = "Two",
                Content = modelUpdateContent,
            };

            bool updateOne = this.service.UpdateAsync(modelUpdateOne).GetAwaiter().GetResult();

            updateOne.ShouldBeTrue();

            string updateOneTitle = this.service.GetAlbumById(test).Title;
            string updateOneContent = this.service.GetAlbumById(test).Content;

            Assert.Equal("One", updateOneTitle);
            Assert.Equal(modelContent, updateOneContent);

            bool updateTwo = this.service.UpdateAsync(modelUpdateTwo).GetAwaiter().GetResult();

            updateTwo.ShouldBeTrue();

            string updateTwoTitle = this.service.GetAlbumById(test).Title;
            string updateTwoContent = this.service.GetAlbumById(test).Content;

            Assert.Equal("Two", updateTwoTitle);
            Assert.Equal(modelUpdateContent, updateTwoContent);
        }

        [Fact]
        public void Delete_ShouldWork()
        {
            string modelTitle = "TestCreate";
            string modelContent = "TestCreateContent";

            AlbumViewModel model = new AlbumViewModel
            {
                Title = modelTitle,
                Content = modelContent,
            };

            User user = new User
            {
                UserName = "TestUser"
            };

            var testOne = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();
            var testTwo = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            var result = service.GetAllAlbums();
            Assert.Equal(2, result.Count());

            this.service.DeleteAsync(testOne);

            result = service.GetAllAlbums();
            Assert.Equal(1, result.Count());

            Assert.Equal(testTwo, result.First().Id);

            this.service.DeleteAsync(testTwo);
            result = service.GetAllAlbums();

            result.ShouldBeEmpty();
        }

        [Fact]
        public void CreateComment_ShouldWork()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            string test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            string commentContent = "Comment";

            bool result = this.service.CreateCommentAsync(test, commentContent, user).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            int commentUserAlbumCount = user.Albums.FirstOrDefault(s => s.Id == test).Comments.Count();

            commentUserAlbumCount.ShouldBe(1);
        }

        [Fact]
        public void DeleteComment_ShouldWork()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            string test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            string commentContent = "Comment";

            bool result = this.service.CreateCommentAsync(test, commentContent, user).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            int count = this.service.GetAlbumById(test).Comments.Count();

            count.ShouldBe(1);

            string commentId = this.service.GetAlbumById(test).Comments.First().Id;

            bool deleted = this.service.DeleteCommentAsync(commentId).GetAwaiter().GetResult();

            deleted.ShouldBeTrue();

            count = this.service.GetAlbumById(test).Comments.Count();

            count.ShouldBe(0);
        }

        [Fact]
        public void AddViews_ShouldWork()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            string test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            Album album = this.service.GetAlbumById(test);

            int albumViews = album.Views;

            albumViews.ShouldBe(0);

            bool result = this.service.AddViewedAsync(test).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            album = this.service.GetAlbumById(test);

            albumViews = album.Views;

            albumViews.ShouldBe(1);

            Random random = new Random();

            int count = random.Next(10, 100);

            for (int i = 0; i < count; i++)
            {
                this.service.AddViewedAsync(test).GetAwaiter().GetResult();
            }

            album = this.service.GetAlbumById(test);

            albumViews = album.Views;

            albumViews.ShouldBe(count + 1);
        }

        [Fact]
        public void AddFavorite_ShouldWork()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            string test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            bool result = this.service.FavoriteAsync(test, user).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            int favCount = this.service.GetAlbumById(test).Favorite.Count();
            favCount.ShouldBe(1);

            result = this.service.FavoriteAsync(test, user).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            favCount = this.service.GetAlbumById(test).Favorite.Count();
            favCount.ShouldBe(0);
        }

        [Fact]
        public void GetAlbumById_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => this.service.GetAlbumById(null));
            Assert.Throws<ArgumentException>(() => this.service.GetAlbumById(""));
        }

        [Fact]
        public void GetAlbumByIdAsViewModel_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => this.service.GetAlbumByIdAsViewModel(null));
            Assert.Throws<ArgumentException>(() => this.service.GetAlbumByIdAsViewModel(""));
        }

        [Fact]
        public void CreateAsync_ShouldThrowException()
        {
            User user = new User();
            string test = "test";

            Assert.Throws<ArgumentException>(() =>
                this.service.CreateAsync(null, user).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.CreateAsync("", user).GetAwaiter().GetResult());

            Assert.Throws<ArgumentNullException>(() =>
                this.service.CreateAsync(test, null).GetAwaiter().GetResult());
        }

        [Fact]
        public void UpdateAsync_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                this.service.UpdateAsync(null).GetAwaiter().GetResult());
        }

        [Fact]
        public void DeleteAsync_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                this.service.DeleteAsync(null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.DeleteAsync("").GetAwaiter().GetResult());
        }

        [Fact]
        public void CreateCommentAsync_ShouldThrowException()
        {
            User user = new User();
            string test = "test";

            Assert.Throws<ArgumentException>(() =>
                this.service.CreateCommentAsync(null, test, user).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.CreateCommentAsync("", test, user).GetAwaiter().GetResult());

            Assert.Throws<ArgumentException>(() =>
                this.service.CreateCommentAsync(test, null, user).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.CreateCommentAsync(test, "", user).GetAwaiter().GetResult());

            Assert.Throws<ArgumentNullException>(() =>
                this.service.CreateCommentAsync(test, test, null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentNullException>(() =>
                this.service.CreateCommentAsync(test, test, null).GetAwaiter().GetResult());
        }

        [Fact]
        public void DeleteCommentAsync_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                this.service.DeleteCommentAsync(null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.DeleteCommentAsync("").GetAwaiter().GetResult());
        }

        [Fact]
        public void AddViewedAsync_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                this.service.AddViewedAsync(null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.AddViewedAsync("").GetAwaiter().GetResult());
        }

        [Fact]
        public void FavoriteAsync_ShouldThrowException()
        {
            User user = new User();
            string test = "test";

            Assert.Throws<ArgumentException>(() =>
                this.service.FavoriteAsync(null, user).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.FavoriteAsync("", user).GetAwaiter().GetResult());

            Assert.Throws<ArgumentNullException>(() =>
                this.service.FavoriteAsync(test, null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentNullException>(() =>
                this.service.FavoriteAsync(test, null).GetAwaiter().GetResult());
        }
    }
}