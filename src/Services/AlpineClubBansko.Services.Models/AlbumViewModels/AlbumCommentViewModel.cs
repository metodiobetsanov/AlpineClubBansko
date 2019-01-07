using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using System;

namespace AlpineClubBansko.Services.Models.AlbumViewModels
{
    public class AlbumCommentViewModel : IMapTo<AlbumComment>, IMapFrom<AlbumComment>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public User Author { get; set; }

        public string Comment { get; set; }

        public Album Album { get; set; }
    }
}