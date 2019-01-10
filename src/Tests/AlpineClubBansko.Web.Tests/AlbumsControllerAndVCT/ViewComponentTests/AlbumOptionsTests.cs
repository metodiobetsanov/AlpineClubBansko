using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using AlpineClubBansko.Web.Controllers.Albums;
using AlpineClubBansko.Web.Controllers.Albums.Components;
using AlpineClubBansko.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace AlpineClubBansko.Web.Tests.AlbumsControllerAndVCT.ViewComponentTests
{
    public class AlbumOptionsTests
    {
        private readonly Mock<HttpContext> httpContext;
        private readonly ILogger<AlbumsController> logger;
        private readonly Mock<ITempDataProvider> tempDataProvider;
        private readonly User user;
        private readonly Mock<SignInManager<User>> signInManager;
        private readonly Mock<UserManager<User>> userManager;

        public AlbumOptionsTests()
        {
            this.signInManager = new Mock<SignInManager<User>>();
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
            this.logger = new Mock<ILogger<AlbumsController>>().Object;
        }

        [Fact]
        public void Invoke_NoModel_NoUser()
        {
            signInManager.Setup(s => s.IsSignedIn(It.IsAny<ClaimsPrincipal>()))
                .Returns(false);

            AlbumOptions viewComponent = new AlbumOptions(userManager.Object, signInManager.Object);

            var result = viewComponent.Invoke();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("GuestAlbumsIndex");
        }

        [Fact]
        public void Invoke_NoModel_User()
        {
            signInManager.Setup(s => s.IsSignedIn(It.IsAny<ClaimsPrincipal>()))
                .Returns(true);

            AlbumViewModel model = new AlbumViewModel();

            AlbumOptions viewComponent = new AlbumOptions(userManager.Object, signInManager.Object);

            var result = viewComponent.Invoke();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("UserAlbumsIndex");
        }
    }
}