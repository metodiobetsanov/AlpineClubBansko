using System.Collections.Generic;

namespace AlpineClubBansko.Data.Models
{
    public class Album : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public string StoryId { get; set; }
        public virtual Story Story { get; set; }

        public string EventId { get; set; }
        public virtual Event Event { get; set; }

        public virtual ICollection<RoutesAlbums> Routes { get; set; }

        public int Rating { get; set; }
    }
}