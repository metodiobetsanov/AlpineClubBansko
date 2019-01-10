namespace AlpineClubBansko.Data.Models
{
    public class LikedStories
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
        public string StoryId { get; set; }

        public virtual Story Story { get; set; }
    }
}