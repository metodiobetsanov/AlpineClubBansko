using AlpineClubBansko.Data;
using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models;
using AlpineClubBansko.Services.Models.HomeViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using Xunit;

namespace AlpineClubBansko.Services.Tests
{
    public class HomeServiceTests
    {
        private readonly ApplicationDbContext context;
        private readonly IHomeService service;
        private readonly IServiceProvider provider;
        private readonly IRepository<Route> routeRepository;
        private readonly IRepository<Story> storyRepository;
        private readonly IRepository<Photo> photoRepository;

        public HomeServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IStoryService, StoryService>();
            services.AddScoped<ICloudService, CloudService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).Assembly
            );

            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ApplicationDbContext>();
            this.service = provider.GetService<IHomeService>();
            this.routeRepository = provider.GetService<IRepository<Route>>();
            this.storyRepository = provider.GetService<IRepository<Story>>();
            this.photoRepository = provider.GetService<IRepository<Photo>>();
        }

        [Fact]
        public void GetHomeViewModel_ShouldWork()
        {
            for (int i = 0; i < 20; i++)
            {
                this.routeRepository.AddAsync(new Route()).GetAwaiter();
                this.storyRepository.AddAsync(new Story()).GetAwaiter();
                this.photoRepository.AddAsync(new Photo()).GetAwaiter();
            }

            this.context.SaveChangesAsync().GetAwaiter();

            var model = this.service.GetHomeViewModel();

            model.ShouldBeOfType<HomeViewModel>();
        }
    }
}