using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Extensions;
using AlpineClubBansko.Services.Models.HomeViewModels;
using System.Linq;

namespace AlpineClubBansko.Services
{
    public class HomeService : IHomeService
    {
        private readonly IStoryService storyService;
        private readonly IRouteService routeService;
        private readonly ICloudService cloudService;
        private readonly IUsersService usersService;

        public HomeService(IStoryService storyService,
            IRouteService routeService,
            ICloudService cloudService,
            IUsersService usersService)
        {
            this.usersService = usersService;
            this.cloudService = cloudService;
            this.routeService = routeService;
            this.storyService = storyService;
        }

        public HomeViewModel GetHomeViewModel()
        {
            HomeViewModel model = new HomeViewModel();

            model.TopStories = this.storyService
                .GetAllStoriesAsViewModels()
                .Where(s => !string.IsNullOrEmpty(s.Content))
                .OrderBy(s => s.Favorite.Count)
                .Take(5)
                .ToList();
            if (model.TopStories != null && model.TopStories.Count > 0)
            {
                model.TopStories.ForEach(s => s.Content = s.Content.StorySubstring(300));
            }

            model.TopRoutes = this.routeService
                .GetAllRoutesAsViewModels()
                .OrderBy(r => r.Favorite.Count)
                .Take(5)
                .ToList();

            model.NewPhotos = this.cloudService
                .GetAllPhotosAsViewModels()
                .OrderBy(p => p.CreatedOn)
                .Take(15)
                .ToList();

            model.NewUsers = this.usersService
                .GetAllUsersAsViewModels()
                .OrderBy(p => p.CreatedOn)
                .Take(5)
                .ToList();

            return model;
        }
    }
}