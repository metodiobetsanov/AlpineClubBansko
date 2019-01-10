using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.HomeViewModels;
using AlpineClubBansko.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System;
using System.Security.Claims;
using Xunit;

namespace AlpineClubBansko.Web.Tests
{
    public class HomeControllerTests
    {
        private readonly User user;
        private readonly Mock<UserManager<User>> userManager;
        private readonly Mock<ITempDataProvider> tempDataProvider;
        private readonly Mock<HttpContext> httpContext;
        private readonly ILogger<HomeController> logger;

        public HomeControllerTests()
        {
            this.userManager = new Mock<UserManager<User>>(
                    new Mock<IUserStore<User>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<User>>().Object,
                    new IUserValidator<User>[0],
                    new IPasswordValidator<User>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<User>>>().Object);

            this.user = new User
            {
                Id = "testId",
                UserName = "testUser",
                Email = "test@test.test"
            };
            this.userManager.Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(() => this.user);
            this.tempDataProvider = new Mock<ITempDataProvider>();
            this.httpContext = new Mock<HttpContext>();
            this.logger = new Mock<ILogger<HomeController>>().Object;
        }

        [Fact]
        public void Index_ShouldReturn_View()
        {
            var service = new Mock<IHomeService>();
            service.Setup(s => s.GetHomeViewModel())
                .Returns(() => new HomeViewModel());

            HomeController controller = new HomeController(
                service.Object, userManager.Object, logger);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HomeViewModel>(
                viewResult.ViewData.Model);
        }

        [Fact]
        public void Index_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IHomeService>();
            service.Setup(s => s.GetHomeViewModel())
                .Throws(new Exception());

            HomeController controller = new HomeController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Index();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Error");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void Privacy_ShouldReturn_View()
        {
            var service = new Mock<IHomeService>();

            HomeController controller = new HomeController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Privacy();

            Assert.IsType<ViewResult>(result);
        }
    }
}