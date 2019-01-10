using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Models.WebDataViewModels;
using System;
using System.Linq;

namespace AlpineClubBansko.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUsersService usersService;
        private readonly ICloudService cloudService;
        private readonly IRouteService routeService;
        private readonly IAlbumService albumService;
        private readonly IStoryService storyService;

        public AdminService(IUsersService usersService,
            ICloudService cloudService,
            IRouteService routeService,
            IAlbumService albumService,
            IStoryService storyService)
        {
            this.storyService = storyService;
            this.albumService = albumService;
            this.routeService = routeService;
            this.cloudService = cloudService;
            this.usersService = usersService;
        }

        public WebDataViewModel GetWebData()
        {
            var curMonth = DateTime.UtcNow.Month;
            var curWeek = DateTime.UtcNow.AddDays(-7);

            WebDataViewModel model = new WebDataViewModel();

            model.TotalUser = this.usersService
                .GetAllUsers()
                .Count();

            model.NewUsersLastMonth = this.usersService
                .GetAllUsers()
                .Where(u => u.CreatedOn.Month == curMonth)
                .Count();

            model.NewUsersLastWeek = this.usersService
                .GetAllUsers()
                .Where(u => u.CreatedOn >= curWeek)
                .Count();

            model.TotalRoutes = this.routeService
                .GetAllRoutes()
                .Count();

            model.NewRoutesLastMonth = this.routeService
                .GetAllRoutes()
                .Where(u => u.CreatedOn.Month == curMonth)
                .Count();

            model.NewRoutesLastWeek = this.routeService
                .GetAllRoutes()
                .Where(u => u.CreatedOn >= curWeek)
                .Count();

            model.TotalStories = this.storyService
                .GetAllStories()
                .Count();

            model.NewStoriesLastMonth = this.storyService
                .GetAllStories()
                .Where(u => u.CreatedOn.Month == curMonth)
                .Count();

            model.NewStoriesLastWeek = this.storyService
                .GetAllStories()
                .Where(u => u.CreatedOn >= curWeek)
                .Count();

            model.TotalAlbums = this.albumService
                .GetAllAlbums()
                .Count();

            model.NewAlbumsLastMonth = this.albumService
                .GetAllAlbums()
                .Where(u => u.CreatedOn.Month == curMonth)
                .Count();

            model.NewAlbumsLastWeek = this.albumService
                .GetAllAlbums()
                .Where(u => u.CreatedOn >= curWeek)
                .Count();

            model.TotalPhotos = this.cloudService
                .GetAllPhotos()
                .Count();

            model.NewPhotosLastMonth = this.cloudService
                .GetAllPhotos()
                .Where(u => u.CreatedOn.Month == curMonth)
                .Count();

            model.NewPhotosLastWeek = this.cloudService
                .GetAllPhotos()
                .Where(u => u.CreatedOn >= curWeek)
                .Count();

            return model;
        }
    }
}