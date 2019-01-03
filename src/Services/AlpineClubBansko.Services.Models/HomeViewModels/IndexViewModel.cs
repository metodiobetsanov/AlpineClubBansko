using AlpineClubBansko.Services.Models.AlbumViewModels;
using AlpineClubBansko.Services.Models.RouteViewModels;
using AlpineClubBansko.Services.Models.StoryViewModels;
using AlpineClubBansko.Services.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Services.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public List<StoryViewModel> NewStories { get; set; }

        public List<RouteViewModel> NewRoutes { get; set; }

        public List<RouteViewModel> TopRoutes { get; set; }

        public List<PhotoViewModel> NewPhotos { get; set; }

        public List<UserProfileViewModel> NewUsers { get; set; }
    }
}
