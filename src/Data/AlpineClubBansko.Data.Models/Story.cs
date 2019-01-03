using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Data.Models
{
    public class Story : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public virtual Album Album { get; set; }

        [Required]
        public virtual ICollection<LikedStories> Favorite { get; set; }

        [Required]
        public virtual ICollection<StoryComment> Comments { get; set; }

        public int Views { get; set; }

    }
}