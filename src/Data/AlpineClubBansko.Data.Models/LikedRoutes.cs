using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Data.Models
{
    public class LikedRoutes
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string RouteId { get; set; }
        public virtual Route Route { get; set; }
    }
}
