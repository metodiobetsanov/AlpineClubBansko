using System.Collections.Generic;

namespace AlpineClubBansko.Data.Models
{
    public class Album : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public virtual List<Photo> Photos { get; set; }

        public int Rating { get; set; }
    }
}