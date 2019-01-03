using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Data.Models
{
    public class Album : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public string Place { get; set; }

        [Required]
        public virtual ICollection<Photo> Photos { get; set; }

        public string StoryId { get; set; }
        public virtual Story Story { get; set; }

        public string RouteId { get; set; }
        public virtual Route Route { get; set; }

        [Required]
        public virtual ICollection<LikedAlbums> Favorite { get; set; }

        [Required]
        public virtual ICollection<AlbumComment> Comments { get; set; }

        public int Views { get; set; }
    }
}