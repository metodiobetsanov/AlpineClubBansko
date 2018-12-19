using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AlpineClubBansko.Data.Models
{
    public class Route : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string StoryId { get; set; }
        public virtual Story Story { get; set; }

        public virtual Album Album { get; set; }

        public virtual ICollection<Location> Locations { get; set; }

        public int Rating { get; set; }
    }
}
