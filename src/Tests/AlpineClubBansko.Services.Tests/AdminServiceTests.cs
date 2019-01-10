using AlpineClubBansko.Data;
using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models;
using AlpineClubBansko.Services.Models.WebDataViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using Xunit;

namespace AlpineClubBansko.Services.Tests
{
    public class AdminServiceTests
    {
        private readonly ApplicationDbContext context;
        private readonly IAdminService service;
        private readonly IServiceProvider provider;
        private readonly IRepository<Album> albumRepository;
        private readonly IRepository<Route> routeRepository;
        private readonly IRepository<Story> storyRepository;
        private readonly IRepository<Photo> photoRepository;

        public AdminServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IStoryService, StoryService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<ICloudService, CloudService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).Assembly
            );

            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ApplicationDbContext>();
            this.service = provider.GetService<IAdminService>();
            this.albumRepository = provider.GetService<IRepository<Album>>();
            this.routeRepository = provider.GetService<IRepository<Route>>();
            this.storyRepository = provider.GetService<IRepository<Story>>();
            this.photoRepository = provider.GetService<IRepository<Photo>>();
        }

        [Fact]
        public void GetWebData_ShouldWork()
        {
            for (int i = 0; i < 20; i++)
            {
                this.albumRepository.AddAsync(new Album());
                this.routeRepository.AddAsync(new Route());
                this.storyRepository.AddAsync(new Story());
                this.photoRepository.AddAsync(new Photo());
            }

            this.context.SaveChangesAsync();

            var model = this.service.GetWebData();

            model.ShouldBeOfType<WebDataViewModel>();

            Assert.InRange(model.TotalUser, 0, 20);

            model.TotalUser.Equals(20);
            model.NewUsersLastMonth.Equals(20);
            model.NewUsersLastWeek.Equals(20);
            model.TotalRoutes.Equals(20);
            model.NewRoutesLastMonth.Equals(20);
            model.NewRoutesLastWeek.Equals(20);
            model.TotalStories.Equals(20);
            model.NewStoriesLastMonth.Equals(20);
            model.NewStoriesLastWeek.Equals(20);
            model.TotalAlbums.Equals(20);
            model.NewAlbumsLastMonth.Equals(20);
            model.NewAlbumsLastWeek.Equals(20);
            model.TotalPhotos.Equals(20);
            model.NewPhotosLastMonth.Equals(20);
            model.NewPhotosLastWeek.Equals(20);
        }
    }
}