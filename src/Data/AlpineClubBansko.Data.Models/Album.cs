using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlpineClubBansko.Data.Models
{
    public class Album : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public string StoryId { get; set; }
        public virtual Story Story { get; set; }

        public string RouteId { get; set; }
        public virtual Route Route { get; set; }

        public int Rating { get; set; }
    }
}