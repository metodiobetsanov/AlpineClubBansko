using System.Collections.Generic;

namespace AlpineClubBansko.Data.Models
{
    public class Route : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string TimeNeeded { get; set; }

        public virtual Album Album { get; set; }

        public virtual Story Story { get; set; }

        public virtual ICollection<LikedRoutes> Favorite { get; set; }

        public virtual ICollection<Location> Locations { get; set; }

        public virtual ICollection<RouteComment> Comments { get; set; }

        public int Views { get; set; }
    }
}