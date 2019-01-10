namespace AlpineClubBansko.Data.Models
{
    public class AlbumComment : BaseComment
    {
        public string AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}