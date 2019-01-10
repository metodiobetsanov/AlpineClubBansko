namespace AlpineClubBansko.Services.Models.WebDataViewModels
{
    public class WebDataViewModel
    {
        public int TotalUser { get; set; }
        public int NewUsersLastMonth { get; set; }
        public int NewUsersLastWeek { get; set; }

        public int TotalRoutes { get; set; }
        public int NewRoutesLastMonth { get; set; }
        public int NewRoutesLastWeek { get; set; }

        public int TotalStories { get; set; }
        public int NewStoriesLastMonth { get; set; }
        public int NewStoriesLastWeek { get; set; }

        public int TotalAlbums { get; set; }
        public int NewAlbumsLastMonth { get; set; }
        public int NewAlbumsLastWeek { get; set; }

        public int TotalPhotos { get; set; }
        public int NewPhotosLastMonth { get; set; }
        public int NewPhotosLastWeek { get; set; }
    }
}