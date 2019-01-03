using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Data.Models
{
    public class LikedAlbums
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }
    }
}
