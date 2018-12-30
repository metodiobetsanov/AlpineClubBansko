using AlpineClubBansko.Data;
using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models;
using AlpineClubBansko.Services.Models.StoryViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace AlpineClubBansko.Services.Tests
{
    public class StoryServiceTests
    {
        private readonly ApplicationDbContext context;
        private readonly IStoryService service;
        private readonly IServiceProvider provider;
        private readonly IRepository<Story> repo;

        public StoryServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IStoryService, StoryService>();
            services.AddScoped<IRepository<Story>, Repository<Story>>();
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).Assembly
            );

            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ApplicationDbContext>();
            this.service = provider.GetService<IStoryService>();
            this.repo = provider.GetService<IRepository<Story>>();
        }

        [Fact]
        public void All_WithExistingsData_ShouldReturnNull()
        {
            var result = service.GetAllStories().ToList();

            result.ShouldBeEmpty();
        }

        [Fact]
        public void All_WithExistingsData_ShouldReturnOneExistingData()
        {
            var story = new Story();
            repo.AddAsync(story);
            repo.SaveChangesAsync();

            var result = service.GetAllStories().ToList();

            result.ShouldNotBeEmpty();

            result.ShouldHaveSingleItem();
        }

        [Fact]
        public void CreateAsync_ShouldReturnCreateStory_AndShouldReturnString()
        {
            string modelTitle = "TestCreate";
            string modelContent = "TestCreateContent";

            StoryViewModel model = new StoryViewModel
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

            user.Stories.ShouldNotBeEmpty();

            user.Stories.ShouldHaveSingleItem();
        }

        [Fact]
        public void GetStoryById_ShouldReturnCorrectElement()
        {
            string modelTitle = "TestCreate";
            string modelContent = "TestCreateContent";

            StoryViewModel model = new StoryViewModel
            {
                Title = modelTitle,
                Content = modelContent,
            };

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(model, user).GetAwaiter().GetResult();

            var result = this.service.GetStoryById(test);

            result.ShouldNotBeNull();

            Assert.Equal(result.Id, test);
        }

        [Fact]
        public void GetStoryById_ShouldReturnNull()
        {
            string modelTitle = "TestCreate";
            string modelContent = "TestCreateContent";

            StoryViewModel model = new StoryViewModel
            {
                Title = modelTitle,
                Content = modelContent,
            };

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(model, user).GetAwaiter().GetResult();

            var result = this.service.GetStoryById(new Guid().ToString());

            result.ShouldBeNull();
        }

        [Fact]
        public void Update_ShouldWork()
        {
            string modelTitle = "TestCreate";
            string modelContent = "TestCreateContent";
            string modelUpdateContent = "TestUpdateContent";

            StoryViewModel model = new StoryViewModel
            {
                Title = modelTitle,
                Content = modelContent,
            };

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(model, user).GetAwaiter().GetResult();

            StoryViewModel updateModel = this.service.GetStoryById(test);

            updateModel.Content = modelUpdateContent;

            this.service.UpdateAsync(updateModel);

            StoryViewModel result = this.service.GetStoryById(test);

            Assert.Equal(modelUpdateContent, result.Content);
        }

        [Fact]
        public void Delete_ShouldWork()
        {
            string modelTitle = "TestCreate";
            string modelContent = "TestCreateContent";

            StoryViewModel model = new StoryViewModel
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

            var result = service.GetAllStories();
            Assert.Equal(2, result.Count());

            this.service.DeleteAsync(testOne);

            result = service.GetAllStories();
            Assert.Equal(1, result.Count());

            Assert.Equal(testTwo, result.First().Id);

            this.service.DeleteAsync(testTwo);
            result = service.GetAllStories();

            result.ShouldBeEmpty();
        }
    }
}