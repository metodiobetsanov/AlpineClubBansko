using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Data.Models
{
    public class Route : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string TimeNeeded { get; set; }

        public virtual Album Album { get; set; }

        [Required]
        public virtual ICollection<LikedRoutes> Favorite { get; set; }

        [Required]
        public virtual ICollection<Location> Locations { get; set; }

        [Required]
        public virtual ICollection<RouteComment> Comments { get; set; }

        public int Views { get; set; }
    }
}