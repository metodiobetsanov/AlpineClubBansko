using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Web.Controllers.Club;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Security.Claims;
using Xunit;

namespace AlpineClubBansko.Web.Tests.ClubControllerT
{
    public class ClubControllerTests
    {
        private readonly User user;
        private readonly Mock<UserManager<User>> userManager;

        public ClubControllerTests()
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
        }

        [Fact]
        public void Index_ShouldReturnView()
        {
            ClubController controller = new ClubController(userManager.Object);

            var result = controller.Index();
            Assert.IsAssignableFrom<ViewResult>(result);
        }
    }
}