using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;

namespace AlpineClubBansko.Services.Models.AlbumViewModels
{
    public class AlbumViewModel : IMapTo<Album>, IMapFrom<Album>
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public IEnumerable<Photo> Photos { get; set; }

        public User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int Rating { get; set; }
    }
}