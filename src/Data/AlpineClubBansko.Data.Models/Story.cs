using System.Collections.Generic;

namespace AlpineClubBansko.Data.Models
{
    public class Story : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public virtual Album Album { get; set; }

        public virtual ICollection<RoutesStories> Routes { get; set; }

        public virtual ICollection<LikedStories> Favorite { get; set; }

        public virtual ICollection<StoryComment> StoryComments { get; set; }

        public int Views { get; set; }

    }
}