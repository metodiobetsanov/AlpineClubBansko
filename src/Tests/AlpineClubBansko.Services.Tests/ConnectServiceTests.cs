using AlpineClubBansko.Data;
using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using Xunit;

namespace AlpineClubBansko.Services.Tests
{
    public class ConnectServiceTests
    {
        private readonly ApplicationDbContext context;
        private readonly IConnectService service;
        private readonly IServiceProvider provider;
        private readonly IRepository<Album> albumRepository;
        private readonly IRepository<Route> routeRepository;
        private readonly IRepository<Story> storyRepository;

        public ConnectServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IConnectService, ConnectService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).Assembly
            );

            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ApplicationDbContext>();
            this.service = provider.GetService<IConnectService>();
            this.albumRepository = provider.GetService<IRepository<Album>>();
            this.routeRepository = provider.GetService<IRepository<Route>>();
            this.storyRepository = provider.GetService<IRepository<Story>>();
        }

        [Fact]
        public void ConnectAlbumAndRoute_ShouldWork()
        {
            Album album = new Album();
            this.albumRepository.AddAsync(album).GetAwaiter();

            Route route = new Route();
            this.routeRepository.AddAsync(route).GetAwaiter();

            this.context.SaveChanges();

            bool result = this.service.ConnectAlbumAndRoute(album.Id, route.Id).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            album.RouteId.ShouldBe(route.Id);
        }

        [Fact]
        public void DisconnectAlbumAndRoute_ShouldWork()
        {
            Album album = new Album();
            this.albumRepository.AddAsync(album).GetAwaiter();

            Route route = new Route();
            this.routeRepository.AddAsync(route).GetAwaiter();

            this.context.SaveChanges();

            bool result = this.service.ConnectAlbumAndRoute(album.Id, route.Id).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            album.RouteId.ShouldBe(route.Id);

            result = this.service.DisconnectAlbumAndRoute(album.Id).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            album.RouteId.ShouldBeNull();
        }

        [Fact]
        public void ConnectAlbumAndStory_ShouldWork()
        {
            Album album = new Album();
            this.albumRepository.AddAsync(album).GetAwaiter();

            Story story = new Story();
            this.storyRepository.AddAsync(story).GetAwaiter();

            this.context.SaveChanges();

            bool result = this.service.ConnectAlbumAndStory(album.Id, story.Id).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            album.StoryId.ShouldBe(story.Id);
        }

        [Fact]
        public void DisconnectAlbumAndStory_ShouldWork()
        {
            Album album = new Album();
            this.albumRepository.AddAsync(album).GetAwaiter();

            Story story = new Story();
            this.storyRepository.AddAsync(story).GetAwaiter();

            this.context.SaveChanges();

            bool result = this.service.ConnectAlbumAndStory(album.Id, story.Id).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            album.StoryId.ShouldBe(story.Id);

            result = this.service.DisconnectAlbumAndStory(album.Id).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            album.StoryId.ShouldBeNull();
        }

        [Fact]
        public void ConnectStoryAndRoute_ShouldWork()
        {
            Story story = new Story();
            this.storyRepository.AddAsync(story).GetAwaiter();

            Route route = new Route();
            this.routeRepository.AddAsync(route).GetAwaiter();

            this.context.SaveChanges();

            bool result = this.service.ConnectStoryAndRoute(story.Id, route.Id).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            story.RouteId.ShouldBe(route.Id);
        }

        [Fact]
        public void DisconnectStoryAndRoute_ShouldWork()
        {
            Story story = new Story();
            this.storyRepository.AddAsync(story).GetAwaiter();

            Route route = new Route();
            this.routeRepository.AddAsync(route).GetAwaiter();

            this.context.SaveChanges();

            bool result = this.service.ConnectStoryAndRoute(story.Id, route.Id).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            story.RouteId.ShouldBe(route.Id);

            result = this.service.DisconnectStoryAndRoute(story.Id).GetAwaiter().GetResult();

            result.ShouldBeTrue();

            story.RouteId.ShouldBeNull();
        }

        [Fact]
        public void ConnectAlbumAndRoute_ShouldThrowException()
        {
            string test = "test";

            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectAlbumAndRoute(test, null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectAlbumAndRoute(test, "").GetAwaiter().GetResult());

            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectAlbumAndRoute(null, test).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectAlbumAndRoute("", test).GetAwaiter().GetResult());
        }

        [Fact]
        public void DisconnectAlbumAndRoute_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                this.service.DisconnectAlbumAndRoute(null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.DisconnectAlbumAndRoute("").GetAwaiter().GetResult());
        }

        [Fact]
        public void ConnectAlbumAndStory_ShouldThrowException()
        {
            string test = "test";

            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectAlbumAndStory(test, null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectAlbumAndStory(test, "").GetAwaiter().GetResult());

            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectAlbumAndStory(null, test).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectAlbumAndStory("", test).GetAwaiter().GetResult());
        }

        [Fact]
        public void DisconnectAlbumAndStory_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                this.service.DisconnectAlbumAndStory(null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.DisconnectAlbumAndStory("").GetAwaiter().GetResult());
        }

        [Fact]
        public void ConnectStoryAndRoute_ShouldThrowException()
        {
            string test = "test";

            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectStoryAndRoute(test, null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectStoryAndRoute(test, "").GetAwaiter().GetResult());

            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectStoryAndRoute(null, test).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.ConnectStoryAndRoute("", test).GetAwaiter().GetResult());
        }

        [Fact]
        public void DisconnectStoryAndRoute_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                this.service.DisconnectStoryAndRoute(null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() =>
                this.service.DisconnectStoryAndRoute("").GetAwaiter().GetResult());
        }
    }
}