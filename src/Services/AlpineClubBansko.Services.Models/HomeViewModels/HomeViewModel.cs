using AlpineClubBansko.Services.Models.AlbumViewModels;
using AlpineClubBansko.Services.Models.RouteViewModels;
using AlpineClubBansko.Services.Models.StoryViewModels;
using AlpineClubBansko.Services.Models.UserViewModels;
using System.Collections.Generic;

namespace AlpineClubBansko.Services.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public List<StoryViewModel> TopStories { get; set; }

        public List<RouteViewModel> TopRoutes { get; set; }

        public List<PhotoViewModel> NewPhotos { get; set; }

        public List<UserProfileViewModel> NewUsers { get; set; }
    }
}