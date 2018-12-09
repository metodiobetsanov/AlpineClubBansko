using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Data.Models
{
    public class Route : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public virtual List<Location> Locations { get; set; }

        public int Rating { get; set; }
    }
}
