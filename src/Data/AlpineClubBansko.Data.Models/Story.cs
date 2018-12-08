using AlpineClubBansko.Services.Mapping.Contracts;

namespace AlpineClubBansko.Data.Models
{
    public class Story : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public int Rating { get; set; }
    }
}