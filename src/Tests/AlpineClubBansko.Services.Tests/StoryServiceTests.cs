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
        private readonly IRepository<Story> storyRepo;

        public StoryServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IStoryService, StoryService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).Assembly
            );

            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ApplicationDbContext>();
            this.service = provider.GetService<IStoryService>();
            this.storyRepo = provider.GetService<IRepository<Story>>();
        }

        [Fact]
        public void All_WithExistingData_ShouldReturnNull()
        {
            var stories = service.GetAllStories().ToList();

            stories.ShouldBeEmpty();

            var storiesAsViewModel = service.GetAllStoriesAsViewModels().ToList();

            storiesAsViewModel.ShouldBeEmpty();
        }

        [Fact]
        public void All_WithExistingData_ShouldReturnOneExistingData()
        {
            User user = new User
            {
                UserName = "TestUser"
            };

            Story story = new Story()
            {
                Title = "Title",
                Author = user,
            };

            this.storyRepo.AddAsync(story);
            this.storyRepo.SaveChangesAsync();

            var count = this.storyRepo.All().Count();

            Assert.Equal(1, count);

            var stories = service.GetAllStories().ToList();

            stories.ShouldNotBeEmpty();

            stories.ShouldHaveSingleItem();

            var storiesAsViewModel = service.GetAllStoriesAsViewModels().ToList();

            storiesAsViewModel.ShouldNotBeEmpty();

            storiesAsViewModel.ShouldHaveSingleItem();
        }

        [Fact]
        public void CreateAsync_ShouldReturnCreateStory_AndShouldReturnString()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            test.ShouldNotBeNullOrEmpty();

            test.ShouldBeOfType<string>();

            user.Stories.ShouldNotBeEmpty();

            user.Stories.ShouldHaveSingleItem();
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

            var repoCount = this.storyRepo.All().Count();

            Assert.Equal(count, repoCount);

            int allStoryCount = this.service.GetAllStories().Count();

            Assert.Equal(count, allStoryCount);

            int allStoryAsViewModelsCount = this.service.GetAllStoriesAsViewModels().Count();

            Assert.Equal(count, allStoryAsViewModelsCount);
        }

        [Fact]
        public void GetStoryById_ShouldReturnCorrectElement()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            var story = this.service.GetStoryById(test);

            story.ShouldNotBeNull();

            Assert.Equal(story.Id, test);

            var storyAsViewModel = this.service.GetStoryByIdAsViewModel(test);

            storyAsViewModel.ShouldNotBeNull();

            Assert.Equal(storyAsViewModel.Id, test);
        }

        [Fact]
        public void GetStoryById_ShouldReturnNull()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            var story = this.service.GetStoryById(new Guid().ToString());

            story.ShouldBeNull();

            var storyAsViewModel = this.service.GetStoryByIdAsViewModel(new Guid().ToString());

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

            StoryViewModel modelUpdateOne = new StoryViewModel
            {
                Id = test,
                Title = "One",
                Content = modelContent,
            };

            StoryViewModel modelUpdateTwo = new StoryViewModel
            {
                Id = test,
                Title = "Two",
                Content = modelUpdateContent,
            };

            bool updateOne = this.service.UpdateAsync(modelUpdateOne).GetAwaiter().GetResult();

            updateOne.ShouldBeTrue();

            string updateOneTitle = this.service.GetStoryById(test).Title;
            string updateOneContent = this.service.GetStoryById(test).Content;

            Assert.Equal("One", updateOneTitle);
            Assert.Equal(modelContent, updateOneContent);

            bool updateTwo = this.service.UpdateAsync(modelUpdateTwo).GetAwaiter().GetResult();

            updateTwo.ShouldBeTrue();

            string updateTwoTitle = this.service.GetStoryById(test).Title;
            string updateTwoContent = this.service.GetStoryById(test).Content;

            Assert.Equal("Two", updateTwoTitle);
            Assert.Equal(modelUpdateContent, updateTwoContent);
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

            var testOne = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();
            var testTwo = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

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

            int commentUserStoryCount = user.Stories.FirstOrDefault(s => s.Id == test).Comments.Count();

            commentUserStoryCount.ShouldBe(1);
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

            int count = this.service.GetStoryById(test).Comments.Count();

            count.ShouldBe(1);

            string commentId = this.service.GetStoryById(test).Comments.First().Id;

            bool deleted = this.service.DeleteCommentAsync(commentId).GetAwaiter().GetResult();

            deleted.ShouldBeTrue();

            count = this.service.GetStoryById(test).Comments.Count();

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

            Story story = this.service.GetStoryById(test);

            int storyViews = story.Views;

            storyViews.ShouldBe(0);

            bool result = this.service.AddViewedAsync(test).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            story = this.service.GetStoryById(test);

            storyViews = story.Views;

            storyViews.ShouldBe(1);

            Random random = new Random();

            int count = random.Next(10, 100);

            for (int i = 0; i < count; i++)
            {
                this.service.AddViewedAsync(test).GetAwaiter().GetResult();
            }

            story = this.service.GetStoryById(test);

            storyViews = story.Views;

            storyViews.ShouldBe(count + 1);
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

            int favCount = this.service.GetStoryById(test).Favorite.Count();
            favCount.ShouldBe(1);

            result = this.service.FavoriteAsync(test, user).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            favCount = this.service.GetStoryById(test).Favorite.Count();
            favCount.ShouldBe(0);
        }

        [Fact]
        public void GetStoryById_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => this.service.GetStoryById(null));
            Assert.Throws<ArgumentException>(() => this.service.GetStoryById(""));
        }

        [Fact]
        public void GetStoryByIdAsViewModel_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => this.service.GetStoryByIdAsViewModel(null));
            Assert.Throws<ArgumentException>(() => this.service.GetStoryByIdAsViewModel(""));
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