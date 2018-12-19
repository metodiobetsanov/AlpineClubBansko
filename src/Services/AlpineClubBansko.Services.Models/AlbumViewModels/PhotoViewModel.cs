using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AlpineClubBansko.Services.Models.AlbumViewModels
{
    public class PhotoViewModel : IMapTo<Photo>, IMapFrom<Photo>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public User Author { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Photographer { get; set; }

        public Album Album { get; set; }

        public string LocationUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public int Rating { get; set; }

    }
}
