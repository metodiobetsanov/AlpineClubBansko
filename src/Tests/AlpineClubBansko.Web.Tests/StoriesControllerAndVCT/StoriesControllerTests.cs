using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.StoryViewModels;
using AlpineClubBansko.Web.Controllers.Stories;
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

namespace AlpineClubBansko.Web.Tests.StoriesControllerAndVCT
{
    public class StoriesControllerTests
    {
        private readonly User user;
        private readonly Mock<UserManager<User>> userManager;
        private readonly Mock<ITempDataProvider> tempDataProvider;
        private readonly Mock<HttpContext> httpContext;
        private readonly ILogger<StoriesController> logger;

        public StoriesControllerTests()
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
            this.logger = new Mock<ILogger<StoriesController>>().Object;
        }

        [Fact]
        public void Index_ShouldReturnView()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetAllStoriesAsViewModels())
                .Returns(() => new List<StoryViewModel>());

            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger);

            var result = controller.Index();
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<List<StoryViewModel>>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void Index_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetAllStoriesAsViewModels())
                .Throws(new Exception());

            StoriesController controller = new StoriesController(
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
        public async Task Create_ShouldReturnRedirect()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.CreateAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", user))
                .ReturnsAsync(() => "testId");

            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            CreateStoryInputModel model = new CreateStoryInputModel
            {
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories/Update/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("warning");
        }

        [Fact]
        public async Task Create_ShouldReturnView_OnInValidModel()
        {
            var service = new Mock<IStoryService>();

            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };
            controller.ModelState.AddModelError("k", "e");
            CreateStoryInputModel model = new CreateStoryInputModel
            {
                Title = ""
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<CreateStoryInputModel>();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Create_ShouldReturnRedirect_OnCreationFail()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.CreateAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", user))
                .ReturnsAsync(() => null);

            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            CreateStoryInputModel model = new CreateStoryInputModel
            {
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Create_ShouldReturnRedirect_OnException()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.CreateAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", user))
                .Throws(new Exception());

            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            CreateStoryInputModel model = new CreateStoryInputModel
            {
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            var result = await controller.Create(model);
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void Details_ShouldReturnView()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetStoryByIdAsViewModel("testId"))
                .Returns(() => new StoryViewModel
                {
                    Id = "testId"
                });
            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Details("testId");
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            viewResult.Model.ShouldBeOfType<StoryViewModel>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void Details_ShouldReturnRedirect_IfItemDontExists()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetStoryByIdAsViewModel("testId"))
                .Returns(() => null);
            StoriesController controller = new StoriesController(
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
            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetStoryByIdAsViewModel("testId"))
                .Throws(new Exception());
            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.Details("testId");
            var viewResult = Assert.IsAssignableFrom<RedirectResult>(result);
            viewResult.Url.ShouldBe("/Stories");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public void UpdateGet_ShouldReturnView_IfAllIsOkAndItemExists()
        {
            user.Stories = new List<Story>{
                new Story
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetStoryByIdAsViewModel("testId"))
                .Returns(() => new StoryViewModel
                {
                    Author = user,
                    Id = "testId"
                });

            StoriesController controller = new StoriesController(
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
            viewResult.Model.ShouldBeOfType<StoryViewModel>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_IfAllIsOkButItemDontExists()
        {
            user.Stories = new List<Story>{
                new Story
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetStoryByIdAsViewModel("testId"))
                .Returns(() => null);

            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("warning");
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_IfNotAuthorButAdmin()
        {
            user.Stories = new List<Story>();

            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetStoryByIdAsViewModel("testId"))
                .Returns(() => new StoryViewModel
                {
                    Id = "testId"
                });

            StoriesController controller = new StoriesController(
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
            viewResult.Model.ShouldBeOfType<StoryViewModel>();
            viewResult.Model.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_IfNotAuthorAndNotAdmin()
        {
            user.Stories = new List<Story>();

            var service = new Mock<IStoryService>();

            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories/Details/testId");
        }

        [Fact]
        public void UpdateGet_ShouldReturnRedirect_OnException()
        {
            user.Stories = new List<Story>{
                new Story
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetStoryByIdAsViewModel("testId"))
                .Throws(new Exception());

            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnView_IfAllIsOkAndServiceTrue()
        {
            user.Stories = new List<Story>{
                new Story
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var model = new StoryViewModel
            {
                Id = "testId",
                Author = user,
                Content = "Lorem ipsum dolor sit amet,consectetur adipiscing eli"
            };

            var service = new Mock<IStoryService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnView_OnInValidModel()
        {
            user.Stories = new List<Story>{
                new Story
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var model = new StoryViewModel
            {
                Id = "testId",
                Author = user
            };

            var service = new Mock<IStoryService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            StoriesController controller = new StoriesController(
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
            viewResult.Model.ShouldBeOfType<StoryViewModel>();
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnRedirect_IfNotAuthorButAdmin()
        {
            user.Stories = new List<Story>();

            var model = new StoryViewModel
            {
                Id = "testId",
                Content = "Lorem ipsum dolor sit amet,consectetur adipiscing eli"
            };

            var service = new Mock<IStoryService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnRedirect_IfNotAuthorAndNotAdmin()
        {
            user.Stories = new List<Story>();

            var model = new StoryViewModel
            {
                Id = "testId",
            };

            var service = new Mock<IStoryService>();
            service.Setup(s => s.UpdateAsync(model))
                .ReturnsAsync(() => true);

            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnRedirect_OnException()
        {
            user.Stories = new List<Story>{
                new Story
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };

            var model = new StoryViewModel
            {
                Id = "testId",
            };

            var service = new Mock<IStoryService>();
            service.Setup(s => s.UpdateAsync(model))
                .Throws(new Exception());
            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfAllIsOkAndServiceTrue()
        {
            user.Stories = new List<Story>{
                new Story
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };
            var service = new Mock<IStoryService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .ReturnsAsync(() => true);

            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfAllIsOkAndServiceFalse()
        {
            user.Stories = new List<Story>{
                new Story
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };
            var service = new Mock<IStoryService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .ReturnsAsync(() => false);

            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfNotAuthorButAdmin()
        {
            user.Stories = new List<Story>();
            var service = new Mock<IStoryService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .ReturnsAsync(() => true);

            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("success");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_IfNotAuthorAndNotAdmin()
        {
            user.Stories = new List<Story>();
            var service = new Mock<IStoryService>();

            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories/Details/testId");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task Delete_ShouldReturnRedirect_OnException()
        {
            user.Stories = new List<Story>{
                new Story
                {
                    Id = "testId",
                    AuthorId = "testId"
                }
            };
            var service = new Mock<IStoryService>();
            service.Setup(s => s.DeleteAsync("testId"))
                .Throws(new Exception());
            StoriesController controller = new StoriesController(
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
            viewResult.Url.ShouldBe("/Stories");
            controller.TempData.ContainsKey("notification").ShouldBeTrue();
            controller.TempData["notification"].ShouldNotBeNull();
            controller.TempData["notification"].ShouldBeOfType<string[]>();
            string[] arr = controller.TempData["notification"] as string[];
            arr[0].ShouldBe("danger");
        }

        [Fact]
        public async Task CreateComment_ShouldReturnView()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.CreateCommentAsync("testId", "text", user))
                .ReturnsAsync(() => true);

            service.Setup(s => s.GetStoryByIdAsViewModel("testId"))
                .Returns(() => new StoryViewModel());

            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.CreateComment("testId", "text");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<PartialViewResult>(result);
            viewResult.ViewName.ShouldBe("_StoryComments");
        }

        [Fact]
        public async Task CreateComment_ShouldReturnNull()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.CreateCommentAsync("id", "text", user))
                .Throws(new Exception());

            StoriesController controller = new StoriesController(
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
            var service = new Mock<IStoryService>();
            service.Setup(s => s.DeleteCommentAsync("id"))
                .ReturnsAsync(() => true);

            service.Setup(s => s.GetStoryByIdAsViewModel("testId"))
                .Returns(() => new StoryViewModel());

            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = await controller.DeleteComment("id", "testId");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<PartialViewResult>(result);
            viewResult.ViewName.ShouldBe("_StoryComments");
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnNull()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.DeleteCommentAsync("id"))
                .Throws(new Exception());

            StoriesController controller = new StoriesController(
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
            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetAllStoriesAsViewModels())
                .Returns(() => new List<StoryViewModel>());

            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.FilterStories(null, null, 0);
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("ViewStories");
        }

        [Fact]
        public void FilterRoutes_ShouldReturnNull()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetAllStoriesAsViewModels())
                .Throws(new Exception());

            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.FilterStories(null, null, 0);
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
            var service = new Mock<IStoryService>();
            service.Setup(s => s.AddViewedAsync("id"))
                .ReturnsAsync(() => true);

            StoriesController controller = new StoriesController(
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
            var service = new Mock<IStoryService>();
            service.Setup(s => s.AddViewedAsync("id"))
                .Throws(new Exception());

            StoriesController controller = new StoriesController(
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
            var service = new Mock<IStoryService>();
            service.Setup(s => s.FavoriteAsync("id", user))
                .ReturnsAsync(() => true);

            StoriesController controller = new StoriesController(
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
            var service = new Mock<IStoryService>();
            service.Setup(s => s.FavoriteAsync("id", user))
                .Throws(new Exception());

            StoriesController controller = new StoriesController(
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
            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetStoryByIdAsViewModel("id"))
                .Returns(() => new StoryViewModel());

            StoriesController controller = new StoriesController(
                service.Object, userManager.Object, logger)
            {
                TempData = new TempDataDictionary(httpContext.Object, tempDataProvider.Object)
            };

            var result = controller.RefreshStats("id");
            result.ShouldNotBeNull();
            var viewResult = Assert.IsAssignableFrom<ViewComponentResult>(result);
            viewResult.ViewComponentName.ShouldBe("StoryOptions");
        }

        [Fact]
        public void RefreshStats_ShouldReturnNull()
        {
            var service = new Mock<IStoryService>();
            service.Setup(s => s.GetStoryByIdAsViewModel("id"))
                .Throws(new Exception());

            StoriesController controller = new StoriesController(service.Object, userManager.Object, logger)
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