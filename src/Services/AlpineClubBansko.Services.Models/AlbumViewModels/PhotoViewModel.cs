using AlpineClubBansko.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Services.Models.AlbumViewModels
{
    public class PhotoViewModel
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public User Author { get; set; }

        public string Content { get; set; }

        public string Photographer { get; set; }

        public Album Album { get; set; }

        public string LocationUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public int Rating { get; set; }
    }
}
