using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.RouteViewModels;
using AlpineClubBansko.Web.Controllers.Routes;
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

namespace AlpineClubBansko.Web.Tests.RoutesControllerAndVCT
{
    public class RouteControllerTests
    {
        private readonly User user;
        private readonly Mock<UserManager<User>> userManager;
        private readonly Mock<ITempDataProvider> tempDataProvider;
        private readonly Mock<HttpContext> httpContext;
        private readonly ILogger<RoutesController> logger;

        public RouteControllerTests()
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
            this.logger = new Mock<ILogger<RoutesController>>().Object;
        }

        [Fact]
        public void Index_ShouldReturnView()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetAllRoutesAsViewModels())
                .Returns(() => new List<RouteViewModel>());

            RoutesController controller = new RoutesController(service.Object, userManager.Object, logger);

            var result = controller.Index();
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<List<RouteViewModel>>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void Index_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetAllRoutesAsViewModels())
                .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Index();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void GetListCoords_ShouldReturnJson()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetAllRoutesAsViewModels())
                .Returns(() => new List<RouteViewModel>());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.GetListCoords();

            Assert.IsAssignableFrom<JsonResult>(result);
        }

        [Fact]
        public void GetListCoords_ShouldReturnNull_OnException()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetAllRoutesAsViewModels())
               .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.GetListCoords();
            result.ShouldBeNull();
        }

        [Fact]
        public void GetListRouteCoords_ShouldReturnJson()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
                .Returns(() => new RouteViewModel
                {
                    Id = "testId",
                    Locations = new List<LocationViewModel>()
                });

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.GetListRouteCoords("testId");

            var viewResult = Assert.IsAssignableFrom<JsonResult>(result);
        }

        [Fact]
        public void GetListRouteCoords_ShouldReturnNull_OnException()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
               .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.GetListRouteCoords("testId");
            result.ShouldBeNull();
        }

        [Fact]
        public async Task Create_ShouldReturnRedirect()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.CreateAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", user))
                .ReturnsAsync(() => "testId");

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            CreateRouteInputModel model = new CreateRouteInputModel
            {
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Update/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("warning");
        }

        [Fact]
        public async Task Create_ShouldReturnView_OnInValidModel()
        {
            var service = new Mock<IRouteService>();

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            controller.ModelState.AddModelError("k", "e");
            CreateRouteInputModel model = new CreateRouteInputModel
            {
                Title = ""
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<CreateRouteInputModel>();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Create_ShouldReturnRedirect_OnCreationFail()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.CreateAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", user))
                .ReturnsAsync(() => null);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            CreateRouteInputModel model = new CreateRouteInputModel
            {
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Create_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.CreateAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", user))
                .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            CreateRouteInputModel model = new CreateRouteInputModel
            {
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void Details_ShouldReturnView()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
                .Returns(() => new RouteViewModel
                {
                    Id = "testId",
                    Locations = new List<LocationViewModel>()
                });
            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Details("testId");
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<RouteViewModel>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void Details_ShouldReturnRedirect_IfItemDontExists()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
                .Returns(() => null);
            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Details("testId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("warning");
        }

        [Fact]
        public void Details_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
                .Throws(new Exception());
            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Details("testId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void UpdateGet_ShouldReturnView_IfAllIsOkAndItemExists()
        {
            user.Routes = new List<Route>{
                new Route
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
                .Returns(() => new RouteViewModel
                {
                    Author = user,
                    Id = "testId"
                });

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Update("testId");
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<RouteViewModel>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_IfAllIsOkButItemDontExists()
        {
            user.Routes = new List<Route>{
                new Route
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
                .Returns(() => null);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Update("testId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("warning");
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_IfNotAuthorButAdmin()
        {
            user.Routes = new List<Route>();

            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
                .Returns(() => new RouteViewModel
                {
                    Id = "testId"
                });

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Update("testId");
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<RouteViewModel>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_IfNotAuthorAndNotAdmin()
        {
            user.Routes = new List<Route>();

            var service = new Mock<IRouteService>();

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "User")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Update("testId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/testId");
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_OnException()
        {
            user.Routes = new List<Route>{
                new Route
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
                .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Update("testId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnView_IfAllIsOkAndServiceTrue()
        {
            user.Routes = new List<Route>{
                new Route
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var model = new RouteViewModel
            {
                Id = "testId",
                Author = user,
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Locations = new List<LocationViewModel>()
            };

            var service = new Mock<IRouteService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.Update(model);
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnView_OnInValidModel()
        {
            user.Routes = new List<Route>{
                new Route
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var model = new RouteViewModel
            {
                Id = "testId",
                Author = user,
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Locations = new List<LocationViewModel>()
            };

            var service = new Mock<IRouteService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            controller.ModelState.AddModelError("K", "E");

            var result = await controller.Update(model);
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<RouteViewModel>();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnRedirect_IfNotAuthorButAdmin()
        {
            user.Routes = new List<Route>();

            var model = new RouteViewModel
            {
                Id = "testId",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Locations = new List<LocationViewModel>()
            };

            var service = new Mock<IRouteService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.Update(model);
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnRedirect_IfNotAuthorAndNotAdmin()
        {
            user.Routes = new List<Route>();

            var model = new RouteViewModel
            {
                Id = "testId",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Locations = new List<LocationViewModel>()
            };

            var service = new Mock<IRouteService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "User")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.Update(model);
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnRedirect_OnException()
        {
            user.Routes = new List<Route>{
                new Route
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var model = new RouteViewModel
            {
                Id = "testId",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Locations = new List<LocationViewModel>()
            };

            var service = new Mock<IRouteService>();
            service.Setup(s => s.UpdateAsync(model))
                .Throws(new Exception());
            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.Update(model);
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task CreateLocation_ShouldReturnView()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.CreateLocationAsync(new LocationViewModel(), user))
                .ReturnsAsync(() => true);
            service.Setup(s => s.GetRouteByIdAsViewModel("id"))
                .Returns(() => new RouteViewModel());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            controller.TempData["RouteId"] = "id";

            var result = await controller.CreateLocation(new LocationViewModel());
            var viewResult = Assert.IsAssignableFrom<PartialViewResult>(result);
            viewResult.ViewName.ShouldBe("_Locations");
        }

        [Fact]
        public async Task CreateLocation_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.CreateLocationAsync(new LocationViewModel(), user))
                .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            controller.TempData["RouteId"] = "id";

            var result = await controller.CreateLocation(new LocationViewModel());
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DeleteLocation_ShouldReturnView()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.DeleteLocationAsync("id"))
                .ReturnsAsync(() => true);
            service.Setup(s => s.GetRouteByIdAsViewModel("rId"))
                .Returns(() => new RouteViewModel());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.DeleteLocation("id", "rId");
            var viewResult = Assert.IsAssignableFrom<PartialViewResult>(result);
            viewResult.ViewName.ShouldBe("_Locations");
        }

        [Fact]
        public async Task DeleteLocation_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.DeleteLocationAsync("id"))
                .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.DeleteLocation("id", "rId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/rId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfAllIsOkAndServiceTrue()
        {
            user.Routes = new List<Route>{
                new Route
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };
            var service = new Mock<IRouteService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .ReturnsAsync(() => true);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.Delete("testId");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfAllIsOkAndServiceFalse()
        {
            user.Routes = new List<Route>{
                new Route
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };
            var service = new Mock<IRouteService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .ReturnsAsync(() => false);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.Delete("testId");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfNotAuthorButAdmin()
        {
            user.Routes = new List<Route>();
            var service = new Mock<IRouteService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .ReturnsAsync(() => true);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.Delete("testId");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfNotAuthorAndNotAdmin()
        {
            user.Routes = new List<Route>();
            var service = new Mock<IRouteService>();

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "User")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.Delete("testId");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_OnException()
        {
            user.Routes = new List<Route>{
                new Route
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };
            var service = new Mock<IRouteService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .Throws(new Exception());
            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "Administrator")
                        })),
                    },
                },
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            var result = await controller.Delete("testId");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Routes");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task CreateComment_ShouldReturnView()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.CreateCommentAsync("testId", "text", user))
                .ReturnsAsync(() => true);

            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
                .Returns(() => new RouteViewModel());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.CreateComment("testId", "text");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<PartialViewResult>(result);
            viewResult.ViewName.ShouldBe("_RouteComments");
        }

        [Fact]
        public async Task CreateComment_ShouldReturnNull()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.CreateCommentAsync("id", "text", user))
                .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.CreateComment("id", "text");
            result.ShouldBeNull();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnView()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.DeleteCommentAsync("id"))
                .ReturnsAsync(() => true);

            service.Setup(s => s.GetRouteByIdAsViewModel("testId"))
                .Returns(() => new RouteViewModel());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.DeleteComment("id", "testId");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<PartialViewResult>(result);
            viewResult.ViewName.ShouldBe("_RouteComments");
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnNull()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.DeleteCommentAsync("id"))
                .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.DeleteComment("id", "rId");
            result.ShouldBeNull();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void FilterRoutes_ShouldReturnView()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetAllRoutesAsViewModels())
                .Returns(() => new List<RouteViewModel>());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.FilterRoutes(null, null, 0);
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("ViewRoutes");
        }

        [Fact]
        public void FilterRoutes_ShouldReturnNull()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetAllRoutesAsViewModels())
                .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.FilterRoutes(null, null, 0);
            result.ShouldBeNull();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task AddView_ShouldReturnTrue()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.AddViewedAsync("id"))
                .ReturnsAsync(() => true);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.AddViewed("id");
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AddView_ShouldReturnFalse()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.AddViewedAsync("id"))
                .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.AddViewed("id");
            result.ShouldBeFalse();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Favorite_ShouldReturnTrue()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.FavoriteAsync("id", user))
                .ReturnsAsync(() => true);

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.Favorite("id");
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Favorite_ShouldReturnFalse()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.FavoriteAsync("id", user))
                .Throws(new Exception());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.Favorite("id");
            result.ShouldBeFalse();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void RefreshStats_ShouldReturnView()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("id"))
                .Returns(() => new RouteViewModel());

            RoutesController controller = new RoutesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.RefreshStats("id");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("RouteOptions");
        }

        [Fact]
        public void RefreshStats_ShouldReturnNull()
        {
            var service = new Mock<IRouteService>();
            service.Setup(s => s.GetRouteByIdAsViewModel("id"))
                .Throws(new Exception());

            RoutesController controller = new RoutesController(service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.RefreshStats("id");
            result.ShouldBeNull();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }
    }
}