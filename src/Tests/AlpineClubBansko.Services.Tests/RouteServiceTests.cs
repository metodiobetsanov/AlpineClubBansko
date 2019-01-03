using AlpineClubBansko.Data;
using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models;
using AlpineClubBansko.Services.Models.RouteViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace AlpineClubBansko.Services.Tests
{
    public class RouteServiceTests
    {
        private readonly ApplicationDbContext context;
        private readonly IRouteService service;
        private readonly IServiceProvider provider;
        private readonly IRepository<Route> routeRepo;

        public RouteServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).Assembly
            );

            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ApplicationDbContext>();
            this.service = provider.GetService<IRouteService>();
            this.routeRepo = provider.GetService<IRepository<Route>>();
        }

        [Fact]
        public void All_WithExistingData_ShouldReturnNull()
        {
            var routes = service.GetAllRoutes().ToList();

            routes.ShouldBeEmpty();

            var storiesAsViewModel = service.GetAllRoutesAsViewModels().ToList();

            storiesAsViewModel.ShouldBeEmpty();
        }

        [Fact]
        public void All_WithExistingData_ShouldReturnOneExistingData()
        {
            User user = new User
            {
                UserName = "TestUser"
            };

            Route route = new Route()
            {
                Title = "Title",
                Author = user,
            };

            this.routeRepo.AddAsync(route);
            this.routeRepo.SaveChangesAsync();

            var count = this.routeRepo.All().Count();

            Assert.Equal(1, count);

            var routes = service.GetAllRoutes().ToList();

            routes.ShouldNotBeEmpty();

            routes.ShouldHaveSingleItem();

            var routesAsViewModel = service.GetAllRoutesAsViewModels().ToList();

            routesAsViewModel.ShouldNotBeEmpty();

            routesAsViewModel.ShouldHaveSingleItem();
        }

        [Fact]
        public void CreateAsync_ShouldReturnCreateRoute_AndShouldReturnString()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            test.ShouldNotBeNullOrEmpty();

            test.ShouldBeOfType<string>();

            user.Routes.ShouldNotBeEmpty();

            user.Routes.ShouldHaveSingleItem();
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

            var repoCount = this.routeRepo.All().Count();

            Assert.Equal(count, repoCount);

            int allRoutesCount = this.service.GetAllRoutes().Count();

            Assert.Equal(count, allRoutesCount);

            int allRoutesAsViewModelsCount = this.service.GetAllRoutesAsViewModels().Count();

            Assert.Equal(count, allRoutesAsViewModelsCount);
        }

        [Fact]
        public void GetRouteById_ShouldReturnCorrectElement()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            var route = this.service.GetRouteById(test);

            route.ShouldNotBeNull();

            Assert.Equal(route.Id, test);

            var routeAsViewModel = this.service.GetRouteByIdAsViewModel(test);

            routeAsViewModel.ShouldNotBeNull();

            Assert.Equal(routeAsViewModel.Id, test);
        }

        [Fact]
        public void GetRouteById_ShouldReturnNull()
        {
            string modelTitle = "TestCreate";

            User user = new User
            {
                UserName = "TestUser"
            };

            var test = this.service.CreateAsync(modelTitle, user).GetAwaiter().GetResult();

            var route = this.service.GetRouteById(new Guid().ToString());

            route.ShouldBeNull();

            var routeAsViewModel = this.service.GetRouteByIdAsViewModel(new Guid().ToString());

            routeAsViewModel.ShouldBeNull();
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

            RouteViewModel modelUpdateOne = new RouteViewModel
            {
                Id = test,
                Title = "One",
                Content = modelContent,
            };

            RouteViewModel modelUpdateTwo = new RouteViewModel
            {
                Id = test,
                Title = "Two",
                Content = modelUpdateContent,
            };

            bool updateOne = this.service.UpdateAsync(modelUpdateOne).GetAwaiter().GetResult();

            updateOne.ShouldBeTrue();

            string updateOneTitle = this.service.GetRouteById(test).Title;
            string updateOneContent = this.service.GetRouteById(test).Content;

            Assert.Equal("One", updateOneTitle);
            Assert.Equal(modelContent, updateOneContent);

            bool updateTwo = this.service.UpdateAsync(modelUpdateTwo).GetAwaiter().GetResult();

            updateTwo.ShouldBeTrue();

            string updateTwoTitle = this.service.GetRouteById(test).Title;
            string updateTwoContent = this.service.GetRouteById(test).Content;

            Assert.Equal("Two", updateTwoTitle);
            Assert.Equal(modelUpdateContent, updateTwoContent);
        }

        [Fact]
        public void Delete_ShouldWork()
        {
            string modelTitle = "TestCreate";
            string modelContent = "TestCreateContent";

            RouteViewModel model = new RouteViewModel
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

            var result = service.GetAllRoutes();
            Assert.Equal(2, result.Count());

            this.service.DeleteAsync(testOne);

            result = service.GetAllRoutes();
            Assert.Equal(1, result.Count());

            Assert.Equal(testTwo, result.First().Id);

            this.service.DeleteAsync(testTwo);
            result = service.GetAllRoutes();

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

            int commentUserRouteCount = user.Routes.FirstOrDefault(s => s.Id == test).Comments.Count();

            commentUserRouteCount.ShouldBe(1);
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

            int count = this.service.GetRouteById(test).Comments.Count();

            count.ShouldBe(1);

            string commentId = this.service.GetRouteById(test).Comments.First().Id;

            bool deleted = this.service.DeleteCommentAsync(commentId).GetAwaiter().GetResult();

            deleted.ShouldBeTrue();

            count = this.service.GetRouteById(test).Comments.Count();

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

            Route route = this.service.GetRouteById(test);

            int routeViews = route.Views;

            routeViews.ShouldBe(0);

            bool result = this.service.AddViewedAsync(test).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            route = this.service.GetRouteById(test);

            routeViews = route.Views;

            routeViews.ShouldBe(1);

            Random random = new Random();

            int count = random.Next(10, 100);

            for (int i = 0; i < count; i++)
            {
                this.service.AddViewedAsync(test).GetAwaiter().GetResult();
            }

            route = this.service.GetRouteById(test);

            routeViews = route.Views;

            routeViews.ShouldBe(count+1);
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

            int favCount = this.service.GetRouteById(test).Favorite.Count();
            favCount.ShouldBe(1);

            result = this.service.FavoriteAsync(test, user).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            favCount = this.service.GetRouteById(test).Favorite.Count();
            favCount.ShouldBe(0);
        }

        [Fact]
        public void GetRouteById_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => this.service.GetRouteById(null));
            Assert.Throws<ArgumentException>(() => this.service.GetRouteById(""));
        }

        [Fact]
        public void GetRouteByIdAsViewModel_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => this.service.GetRouteByIdAsViewModel(null));
            Assert.Throws<ArgumentException>(() => this.service.GetRouteByIdAsViewModel(""));
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
        public void CreateLocationAsync_ShouldThrowException()
        {
            User user = new User();
            LocationViewModel location = new LocationViewModel();

            Assert.Throws<ArgumentNullException>(() =>
                this.service.CreateLocationAsync(location, null).GetAwaiter().GetResult());

            Assert.Throws<ArgumentNullException>(() =>
                this.service.CreateLocationAsync( null, user).GetAwaiter().GetResult());

        }

        [Fact]
        public void DeleteLocationAsync_ShouldThrowException()
        {
            string test = "";

            Assert.Throws<ArgumentException>(() =>
                this.service.DeleteLocationAsync(null).GetAwaiter().GetResult());

            Assert.Throws<ArgumentException>(() =>
                this.service.DeleteLocationAsync(test).GetAwaiter().GetResult());

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