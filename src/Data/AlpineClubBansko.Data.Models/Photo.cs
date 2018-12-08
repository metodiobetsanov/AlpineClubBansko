namespace AlpineClubBansko.Data.Models
{
    public class Photo : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Photographer { get; set; }

        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public string LocationUrl { get; set; }

        public int Rating { get; set; }
    }
}