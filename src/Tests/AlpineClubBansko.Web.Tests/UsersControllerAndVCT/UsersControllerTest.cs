using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.UserViewModels;
using AlpineClubBansko.Web.Controllers.Users;
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

namespace AlpineClubBansko.Web.Tests.UsersControllerAndVCT
{
    public class UsersControllerTest
    {
        private readonly User user;
        private readonly Mock<UserManager<User>> userManager;
        private readonly Mock<ITempDataProvider> tempDataProvider;
        private readonly Mock<HttpContext> httpContext;
        private readonly ILogger<UsersController> logger;

        public UsersControllerTest()
        {
            this.userManager = new Mock<UserManager<User>>(
                    MockBehavior.Default,
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
            this.logger = new Mock<ILogger<UsersController>>().Object;
        }

        [Fact]
        public void Index_ShouldReturnView()
        {
            var service = new Mock<IUsersService>();
            service.Setup(s => s.GetUserByIdAsViewModel("id"))
                .Returns(() => new UserProfileViewModel());

            UsersController controller = new UsersController(service.Object, userManager.Object, logger);

            var result = controller.Profile("id");
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<UserProfileViewModel>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void Index_ShouldReturnRedirect_IfNotFound()
        {
            var service = new Mock<IUsersService>();
            service.Setup(s => s.GetUserByIdAsViewModel("id"))
                .Returns(() => null);

            UsersController controller = new UsersController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Profile("id");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("warning");
        }

        [Fact]
        public void Index_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IUsersService>();
            service.Setup(s => s.GetUserByIdAsViewModel("id"))
                .Throws(new Exception());

            UsersController controller = new UsersController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Profile("id");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }
    }
}