using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Web.Controllers.Connect;
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
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace AlpineClubBansko.Web.Tests.ConnectControllerT
{
    public class ConnectControllerTests
    {
        private readonly User user;
        private readonly Mock<UserManager<User>> userManager;
        private readonly Mock<ITempDataProvider> tempDataProvider;
        private readonly Mock<HttpContext> httpContext;
        private readonly ILogger<ConnectController> logger;

        public ConnectControllerTests()
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
            this.logger = new Mock<ILogger<ConnectController>>().Object;
        }

        [Fact]
        public async Task ConnectRouteAndAlbum_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndRoute("aId", "rId"))
                .ReturnsAsync(() => true);

            ConnectRouteAndAlbumInputModel model = new ConnectRouteAndAlbumInputModel
            {
                AlbumId = "aId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectRouteAndAlbum(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/rId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task ConnectRouteAndAlbum_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndRoute("aId", "rId"))
                .ReturnsAsync(() => false);

            ConnectRouteAndAlbumInputModel model = new ConnectRouteAndAlbumInputModel
            {
                AlbumId = "aId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectRouteAndAlbum(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/rId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectRouteAndAlbum_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndRoute("aId", "rId"))
                .Throws(new Exception());

            ConnectRouteAndAlbumInputModel model = new ConnectRouteAndAlbumInputModel
            {
                AlbumId = "aId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectRouteAndAlbum(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectRouteAndAlbum_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndRoute("aId"))
                .ReturnsAsync(() => true);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectRouteAndAlbum("aId", "rId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/rId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task DisconnectRouteAndAlbum_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndRoute("aId"))
                .ReturnsAsync(() => false);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectRouteAndAlbum("aId", "rId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/rId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectRouteAndAlbum_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndRoute("aId"))
                .Throws(new Exception());

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectRouteAndAlbum("aId", "rId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectRouteAndStory_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectStoryAndRoute("sId", "rId"))
                .ReturnsAsync(() => true);

            ConnectRouteAndStoryInputModel model = new ConnectRouteAndStoryInputModel
            {
                StoryId = "sId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectRouteAndStory(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/rId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task ConnectRouteAndStory_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectStoryAndRoute("sId", "rId"))
                .ReturnsAsync(() => false);

            ConnectRouteAndStoryInputModel model = new ConnectRouteAndStoryInputModel
            {
                StoryId = "sId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectRouteAndStory(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/rId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectRouteAndStory_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectStoryAndRoute("sId", "rId"))
                .Throws(new Exception());

            ConnectRouteAndStoryInputModel model = new ConnectRouteAndStoryInputModel
            {
                StoryId = "sId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectRouteAndStory(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectRouteAndStory_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectStoryAndRoute("sId"))
                .ReturnsAsync(() => true);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectRouteAndStory("sId", "rId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/rId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task DisconnectRouteAndStory_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndRoute("aId"))
                .ReturnsAsync(() => false);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectRouteAndStory("aId", "rId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/rId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectRouteAndStory_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectStoryAndRoute("sId"))
                .Throws(new Exception());

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectRouteAndStory("sId", "rId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectAlbumAndRoute_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndRoute("aId", "rId"))
                .ReturnsAsync(() => true);

            ConnectRouteAndAlbumInputModel model = new ConnectRouteAndAlbumInputModel
            {
                AlbumId = "aId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectAlbumAndRoute(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums/Details/aId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task ConnectAlbumAndRoute_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndRoute("aId", "rId"))
                .ReturnsAsync(() => false);

            ConnectRouteAndAlbumInputModel model = new ConnectRouteAndAlbumInputModel
            {
                AlbumId = "aId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectAlbumAndRoute(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums/Details/aId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectAlbumAndRoute_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndRoute("aId", "rId"))
                .Throws(new Exception());

            ConnectRouteAndAlbumInputModel model = new ConnectRouteAndAlbumInputModel
            {
                AlbumId = "aId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectAlbumAndRoute(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectAlbumAndRoute_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndRoute("aId"))
                .ReturnsAsync(() => true);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectAlbumAndRoute("aId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums/Details/aId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task DisconnectAlbumAndRoute_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndRoute("aId"))
                .ReturnsAsync(() => false);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectAlbumAndRoute("aId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums/Details/aId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectAlbumAndRoute_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndRoute("aId"))
                .Throws(new Exception());

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectAlbumAndRoute("aId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectAlbumAndStory_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndStory("aId", "sId"))
                .ReturnsAsync(() => true);

            ConnectAlbumAndStoryInputModel model = new ConnectAlbumAndStoryInputModel
            {
                StoryId = "sId",
                AlbumId = "aId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectAlbumAndStory(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums/Details/aId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task ConnectAlbumAndStory_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndStory("aId", "sId"))
                .ReturnsAsync(() => false);

            ConnectAlbumAndStoryInputModel model = new ConnectAlbumAndStoryInputModel
            {
                StoryId = "sId",
                AlbumId = "aId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectAlbumAndStory(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums/Details/aId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectAlbumAndStory_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndStory("aId", "sId"))
                .Throws(new Exception());

            ConnectAlbumAndStoryInputModel model = new ConnectAlbumAndStoryInputModel
            {
                StoryId = "sId",
                AlbumId = "aId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectAlbumAndStory(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectAlbumAndStory_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndStory("aId"))
                .ReturnsAsync(() => true);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectAlbumAndStory("aId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums/Details/aId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task DisconnectAlbumAndStory_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndStory("aId"))
                .ReturnsAsync(() => false);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectAlbumAndStory("aId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums/Details/aId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectAlbumAndStory_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndStory("aId"))
                .Throws(new Exception());

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectAlbumAndStory("aId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectStoryAndRoute_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectStoryAndRoute("sId", "rId"))
                .ReturnsAsync(() => true);

            ConnectRouteAndStoryInputModel model = new ConnectRouteAndStoryInputModel
            {
                StoryId = "sId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectStoryAndRoute(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories/Details/sId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task ConnectStoryAndRoute_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectStoryAndRoute("sId", "rId"))
                .ReturnsAsync(() => false);

            ConnectRouteAndStoryInputModel model = new ConnectRouteAndStoryInputModel
            {
                StoryId = "sId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectStoryAndRoute(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories/Details/sId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectStoryAndRoute_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectStoryAndRoute("sId", "rId"))
                .Throws(new Exception());

            ConnectRouteAndStoryInputModel model = new ConnectRouteAndStoryInputModel
            {
                StoryId = "sId",
                RouteId = "rId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectStoryAndRoute(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectStoryAndRoute_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectStoryAndRoute("sId"))
                .ReturnsAsync(() => true);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectStoryAndRoute("sId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories/Details/sId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task DisconnectStoryAndRoute_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectStoryAndRoute("sId"))
                .ReturnsAsync(() => false);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectStoryAndRoute("sId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories/Details/sId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectStoryAndRoute_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectStoryAndRoute("sId"))
                .Throws(new Exception());

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectStoryAndRoute("sId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectStoryAndAlbum_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndStory("aId", "sId"))
                .ReturnsAsync(() => true);

            ConnectAlbumAndStoryInputModel model = new ConnectAlbumAndStoryInputModel
            {
                StoryId = "sId",
                AlbumId = "aId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectStoryAndAlbum(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories/Details/sId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task ConnectStoryAndAlbum_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndStory("aId", "sId"))
                .ReturnsAsync(() => false);

            ConnectAlbumAndStoryInputModel model = new ConnectAlbumAndStoryInputModel
            {
                StoryId = "sId",
                AlbumId = "aId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectStoryAndAlbum(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories/Details/sId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task ConnectStoryAndAlbum_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.ConnectAlbumAndStory("aId", "sId"))
                .Throws(new Exception());

            ConnectAlbumAndStoryInputModel model = new ConnectAlbumAndStoryInputModel
            {
                StoryId = "sId",
                AlbumId = "aId"
            };

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.ConnectStoryAndAlbum(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectStoryAndAlbum_OnConnectTrue()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndStory("aId"))
                .ReturnsAsync(() => true);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectStoryAndAlbum("aId", "sId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories/Details/sId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task DisconnectStoryAndAlbum_OnConnectFalse()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndStory("aId"))
                .ReturnsAsync(() => false);

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectStoryAndAlbum("aId", "sId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories/Details/sId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DisconnectStoryAndAlbum_OnException()
        {
            var service = new Mock<IConnectService>();
            service.Setup(s => s.DisconnectAlbumAndStory("aId"))
                .Throws(new Exception());

            ConnectController controller = new ConnectController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.DisconnectStoryAndAlbum("aId", "sId");
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