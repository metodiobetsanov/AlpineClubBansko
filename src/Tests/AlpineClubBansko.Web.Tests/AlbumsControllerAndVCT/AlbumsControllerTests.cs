using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using AlpineClubBansko.Web.Controllers.Albums;
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

namespace AlpineClubBansko.Web.Tests.AlbumsControllerAndVCT
{
    public class AlbumsControllerTests
    {
        private readonly Mock<ICloudService> cloudService;
        private readonly Mock<HttpContext> httpContext;
        private readonly ILogger<AlbumsController> logger;
        private readonly Mock<ITempDataProvider> tempDataProvider;
        private readonly User user;
        private readonly Mock<UserManager<User>> userManager;

        public AlbumsControllerTests()
        {
            this.userManager = new Mock<UserManager<User>>();

            this.user = new User
            {
                Id = "testId",
                UserName = "testUser",
                Email = "test@test.test"
            };
            this.userManager.Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(() => this.user);
            this.cloudService = new Mock<ICloudService>();
            this.tempDataProvider = new Mock<ITempDataProvider>();
            this.httpContext = new Mock<HttpContext>();
            this.logger = new Mock<ILogger<AlbumsController>>().Object;
        }

        [Fact]
        public void Index_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAllAlbumsAsViewModels())
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
        public void Index_ShouldReturnView()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAllAlbumsAsViewModels())
                 .Returns(() => new List<AlbumViewModel>());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger);

            var result = controller.Index();
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<List<AlbumViewModel>>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public async Task Create_ShouldReturnRedirect()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.CreateAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", user))
                .ReturnsAsync(() => "testId");

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            CreateAlbumInputModel model = new CreateAlbumInputModel
            {
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums/Update/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("warning");
        }

        [Fact]
        public async Task Create_ShouldReturnRedirect_OnCreationFail()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.CreateAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", user))
                .ReturnsAsync(() => null);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            CreateAlbumInputModel model = new CreateAlbumInputModel
            {
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Create_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.CreateAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", user))
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            CreateAlbumInputModel model = new CreateAlbumInputModel
            {
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Create_ShouldReturnView_OnInValidModel()
        {
            var service = new Mock<IAlbumService>();

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            controller.ModelState.AddModelError("k", "e");
            CreateAlbumInputModel model = new CreateAlbumInputModel
            {
                Title = ""
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<CreateAlbumInputModel>();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void Details_ShouldReturnView()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("testId"))
                .Returns(() => new AlbumViewModel
                {
                    Id = "testId"
                });
            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Details("testId");
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<AlbumViewModel>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void Details_ShouldReturnRedirect_IfItemDontExists()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("testId"))
                .Returns(() => null);
            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("testId"))
                .Throws(new Exception());
            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Details("testId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Albums");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void UpdateGet_ShouldReturnView_IfAllIsOkAndItemExists()
        {
            user.Albums = new List<Album>{
                new Album
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("testId"))
                .Returns(() => new AlbumViewModel
                {
                    Author = user,
                    Id = "testId"
                });

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Model.ShouldBeOfType<AlbumViewModel>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_IfAllIsOkButItemDontExists()
        {
            user.Albums = new List<Album>{
                new Album
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("testId"))
                .Returns(() => null);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("warning");
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_IfNotAuthorButAdmin()
        {
            user.Albums = new List<Album>();

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("testId"))
                .Returns(() => new AlbumViewModel
                {
                    Id = "testId"
                });

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Model.ShouldBeOfType<AlbumViewModel>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_IfNotAuthorAndNotAdmin()
        {
            user.Albums = new List<Album>();

            var service = new Mock<IAlbumService>();

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums/Details/testId");
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_OnException()
        {
            user.Albums = new List<Album>{
                new Album
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("testId"))
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnView_IfAllIsOkAndServiceTrue()
        {
            user.Albums = new List<Album>{
                new Album
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var model = new AlbumViewModel
            {
                Id = "testId",
                Author = user,
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnView_OnInValidModel()
        {
            user.Albums = new List<Album>{
                new Album
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var model = new AlbumViewModel
            {
                Id = "testId",
                Author = user,
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Model.ShouldBeOfType<AlbumViewModel>();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnRedirect_IfNotAuthorButAdmin()
        {
            user.Albums = new List<Album>();

            var model = new AlbumViewModel
            {
                Id = "testId",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnRedirect_IfNotAuthorAndNotAdmin()
        {
            user.Albums = new List<Album>();

            var model = new AlbumViewModel
            {
                Id = "testId",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnRedirect_OnException()
        {
            user.Albums = new List<Album>{
                new Album
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var model = new AlbumViewModel
            {
                Id = "testId",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.UpdateAsync(model))
                .Throws(new Exception());
            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfAllIsOkAndServiceTrue()
        {
            user.Albums = new List<Album>{
                new Album
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .ReturnsAsync(() => true);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfAllIsOkAndServiceFalse()
        {
            user.Albums = new List<Album>{
                new Album
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .ReturnsAsync(() => false);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfNotAuthorButAdmin()
        {
            user.Albums = new List<Album>();
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .ReturnsAsync(() => true);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfNotAuthorAndNotAdmin()
        {
            user.Albums = new List<Album>();
            var service = new Mock<IAlbumService>();

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_OnException()
        {
            user.Albums = new List<Album>{
                new Album
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .Throws(new Exception());
            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            viewResult.Url.ShouldBe("/Albums");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task UploadPhoto_ShouldReturnView()
        {
            PhotoViewModel model = new PhotoViewModel
            {
                AlbumId = "id"
            };
            IFormFile file = null;

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("id"))
                .Returns(() => new AlbumViewModel());
            service.Setup(s => s.GetAlbumById("id"))
               .Returns(() => new Album());
            this.cloudService.Setup(s => s.UploadImage(file, model))
                .ReturnsAsync(() => true);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.UploadPhoto(file, model);

            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("ViewPhotos");
        }

        [Fact]
        public async Task UploadPhoto_ShouldReturnView_InValidModel()
        {
            PhotoViewModel model = new PhotoViewModel
            {
                AlbumId = "id"
            };
            IFormFile file = null;

            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("id"))
                .Returns(() => new AlbumViewModel
                {
                    Photos = new List<PhotoViewModel>()
                });

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            controller.ModelState.AddModelError("K", "E");

            var result = await controller.UploadPhoto(file, model);
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("ViewPhotos");
        }

        [Fact]
        public async Task UploadPhoto_ShouldReturnNull()
        {
            PhotoViewModel model = new PhotoViewModel();
            IFormFile file = null;

            var service = new Mock<IAlbumService>();
            this.cloudService.Setup(s => s.UploadImage(file, model))
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.UploadPhoto(file, model);
            result.ShouldBeNull();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task DeletePhoto_ShouldReturnView()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("aId"))
                .Returns(() => new AlbumViewModel());
            this.cloudService.Setup(s => s.DeleteImage("id"))
                .ReturnsAsync(() => true);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.DeletePhoto("id", "aId");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("ViewPhotos");
        }

        [Fact]
        public async Task DeletePhoto_ShouldReturnNull()
        {
            var service = new Mock<IAlbumService>();
            this.cloudService.Setup(s => s.DeleteImage("id"))
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.DeletePhoto("id", "aId");
            result.ShouldBeNull();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task CreateComment_ShouldReturnView()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.CreateCommentAsync("testId", "text", user))
                .ReturnsAsync(() => true);

            service.Setup(s => s.GetAlbumByIdAsViewModel("testId"))
                .Returns(() => new AlbumViewModel());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.CreateComment("testId", "text");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<PartialViewResult>(result);
            viewResult.ViewName.ShouldBe("_AlbumComments");
        }

        [Fact]
        public async Task CreateComment_ShouldReturnNull()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.CreateCommentAsync("id", "text", user))
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.DeleteCommentAsync("id"))
                .ReturnsAsync(() => true);

            service.Setup(s => s.GetAlbumByIdAsViewModel("testId"))
                .Returns(() => new AlbumViewModel());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.DeleteComment("id", "testId");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<PartialViewResult>(result);
            viewResult.ViewName.ShouldBe("_AlbumComments");
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnNull()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.DeleteCommentAsync("id"))
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.DeleteComment("id", "aId");
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
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAllAlbumsAsViewModels())
                .Returns(() => new List<AlbumViewModel>());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.FilterAlbums(null, null, 0);
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("ViewAlbums");
        }

        [Fact]
        public void FilterRoutes_ShouldReturnNull()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAllAlbumsAsViewModels())
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.FilterAlbums(null, null, 0);
            result.ShouldBeNull();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void FilterPhotos_ShouldReturnView()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("id"))
                .Returns(() => new AlbumViewModel());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.FilterPhotos(0, "id");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("UpdatePhotos");
        }

        [Fact]
        public void FilterPhotos_ShouldReturnNull()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("id"))
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.FilterPhotos(0, "id");
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
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.AddViewedAsync("id"))
                .ReturnsAsync(() => true);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.AddViewed("id");
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AddView_ShouldReturnFalse()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.AddViewedAsync("id"))
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.FavoriteAsync("id", user))
                .ReturnsAsync(() => true);

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.Favorite("id");
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Favorite_ShouldReturnFalse()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.FavoriteAsync("id", user))
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("id"))
                .Returns(() => new AlbumViewModel());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.RefreshStats("id");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("AlbumOptions");
        }

        [Fact]
        public void RefreshStats_ShouldReturnNull()
        {
            var service = new Mock<IAlbumService>();
            service.Setup(s => s.GetAlbumByIdAsViewModel("id"))
                .Throws(new Exception());

            AlbumsController controller = new AlbumsController(
                service.Object, cloudService.Object, userManager.Object, logger)
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