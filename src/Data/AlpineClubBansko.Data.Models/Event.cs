using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Data.Models
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public virtual List<User> Participants { get; set; }

        public int Rating { get; set; }
    }
}
