using System;
using System.Collections.Generic;

namespace AlpineClubBansko.Data.Models
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<RoutesEvents> Routes { get; set; }

        public virtual ICollection<UsersEvents> Participants { get; set; }

        public int Rating { get; set; }
    }
}