using AlpineClubBansko.Data;
using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models;
using AlpineClubBansko.Services.Models.UserViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace AlpineClubBansko.Services.Tests
{
    public class UsersServiceTests
    {
        private readonly ApplicationDbContext context;
        private readonly IUsersService service;
        private readonly IRepository<User> repository;
        private readonly IServiceProvider provider;

        public UsersServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).Assembly
            );

            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ApplicationDbContext>();
            this.service = provider.GetService<IUsersService>();
            this.repository = provider.GetService<IRepository<User>>();
        }

        [Fact]
        public void GetAllUsers_ShouldWork()
        {
            for (int i = 0; i < 20; i++)
            {
                this.repository.AddAsync(new User());
            }
            this.repository.SaveChangesAsync();

            var result = this.service.GetAllUsers();

            result.ShouldNotBeNull();

            result.Count().ShouldBe(20);
        }

        [Fact]
        public void GetAllUsersAsViewModels_ShouldWork()
        {
            for (int i = 0; i < 20; i++)
            {
                this.repository.AddAsync(new User());
            }
            this.repository.SaveChangesAsync();

            var result = this.service.GetAllUsersAsViewModels();

            result.ShouldNotBeNull();

            result.Count().ShouldBe(20);
        }

        [Fact]
        public void GetUserById_ShouldWork()
        {
            User user = new User
            {
                Id = "test"
            };

            this.repository.AddAsync(user);
            this.repository.SaveChangesAsync();

            var result = this.service.GetUserById("test");

            result.ShouldNotBeNull();
            result.ShouldBeOfType<User>();
        }

        [Fact]
        public void GetUserByIdAsViewModel_ShouldWork()
        {
            User user = new User
            {
                Id = "test"
            };

            this.repository.AddAsync(user);
            this.repository.SaveChangesAsync();

            var result = this.service.GetUserByIdAsViewModel("test");

            result.ShouldNotBeNull();
            result.ShouldBeOfType<UserProfileViewModel>();
        }

        [Fact]
        public void GetUserById_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                this.service.GetUserById(null));
            Assert.Throws<ArgumentException>(() =>
                this.service.GetUserById(""));
        }

        [Fact]
        public void GetUserByIdAsViewModel_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                this.service.GetUserByIdAsViewModel(null));
            Assert.Throws<ArgumentException>(() =>
                this.service.GetUserByIdAsViewModel(""));
        }
    }
}