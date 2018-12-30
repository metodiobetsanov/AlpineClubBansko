namespace AlpineClubBansko.Data.Models
{
    public class UsersEvents
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}