using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AlpineClubBansko.Services.Models.AlbumViewModels
{
    public class AlbumViewModel : IMapTo<Album>, IMapFrom<Album>
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string Cover
        {
            get
            {
                if (Photos == null || Photos.Count == 0)
                {
                    return string.Empty;
                }

                return Photos.First().ThumbnailUrl;
            }
        }

        public List<PhotoViewModel> Photos { get; set; }

        public User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int Rating { get; set; }
    }
}