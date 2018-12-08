using AlpineClubBansko.Data;
using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Shouldly;
using Xunit;
using AlpineClubBansko.Services.Models.StoryViewModels;
using AlpineClubBansko.Services.Models.AlbumViewModels;

namespace AlpineClubBansko.Services.Tests
{
    public class AlbumServiceTests
    {
        private readonly ApplicationDbContext context;
        private readonly IAlbumService service;
        private readonly IServiceProvider provider;
        private readonly IRepository<Album> repo;

        public AlbumServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IRepository<Album>, Repository<Album>>();
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).Assembly
            );

            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ApplicationDbContext>();
            this.service = provider.GetService<IAlbumService>();
            this.repo = provider.GetService<IRepository<Album>>();


        }

        [Fact]
        public void All_WithExistingsData_ShouldReturnNull()
        {
            var result = service.GetAllAlbums().ToList();

            result.ShouldBeEmpty();
        }

        [Fact]
        public void All_WithExistingsData_ShouldReturnOneExistingData()
        {
            var album = new Album();
            repo.AddAsync(album);
            repo.SaveChangesAsync();

            var result = service.GetAllAlbums().ToList();

            result.ShouldNotBeEmpty();

            result.ShouldHaveSingleItem();
        }

        [Fact]
        public void CreateAsync_ShouldReturnCreateStory_AndShouldReturnString()
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

            var test = this.service.CreateAsync(model, user).GetAwaiter().GetResult();

            test.ShouldNotBeNullOrEmpty();

            test.ShouldBeOfType<string>();

            user.Albums.ShouldNotBeEmpty();

            user.Albums.ShouldHaveSingleItem();
        }

        [Fact]
        public void GetAlbumById_ShouldReturnCorrectElement()
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

            var test = this.service.CreateAsync(model, user).GetAwaiter().GetResult();

            var result = this.service.GetAlbumById(test);

            result.ShouldNotBeNull();

            Assert.Equal(result.Id, test);
        }

        [Fact]
        public void GetStoryById_ShouldReturnNull()
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

            var test = this.service.CreateAsync(model, user).GetAwaiter().GetResult();

            var result = this.service.GetAlbumById(new Guid().ToString());

            result.ShouldBeNull();
        }

        [Fact]
        public void Update_ShouldWork()
        {
            string modelTitle = "TestCreate";
            string modelContent = "TestCreateContent";
            string modelUpdateContent = "TestUpdateContent";

            AlbumViewModel model = new AlbumViewModel
            {
                Title = modelTitle,
                Content = modelContent,
            };

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(model, user).GetAwaiter().GetResult();

            var updateModel = this.service.GetAlbumById(test);

            updateModel.Content = modelUpdateContent;

            this.service.UpdateAsync(updateModel);

            var result = this.service.GetAlbumById(test);

            Assert.Equal(modelUpdateContent, result.Content);
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

            var testOne = this.service.CreateAsync(model, user).GetAwaiter().GetResult();
            var testTwo = this.service.CreateAsync(model, user).GetAwaiter().GetResult();

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
    }
}
