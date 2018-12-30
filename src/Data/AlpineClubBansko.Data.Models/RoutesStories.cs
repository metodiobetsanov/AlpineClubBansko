namespace AlpineClubBansko.Data.Models
{
    public class RoutesStories
    {
        public string Id { get; set; }

        public string RouteId { get; set; }
        public virtual Route Route { get; set; }

        public string StoryId { get; set; }
        public virtual Story Story { get; set; }
    }
}