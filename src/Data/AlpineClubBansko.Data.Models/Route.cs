using System.Collections.Generic;

namespace AlpineClubBansko.Data.Models
{
    public class Route : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string TimeNeeded { get; set; }

        public virtual ICollection<RoutesStories> Stories { get; set; }

        public virtual ICollection<RoutesAlbums> Albums { get; set; }

        public virtual ICollection<RoutesEvents> Events { get; set; }

        public virtual ICollection<Location> Locations { get; set; }

        public int Rating { get; set; }
    }
}