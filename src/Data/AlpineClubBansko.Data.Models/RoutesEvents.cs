namespace AlpineClubBansko.Data.Models
{
    public class RoutesEvents
    {
        public string Id { get; set; }

        public string RouteId { get; set; }
        public virtual Route Route { get; set; }

        public string EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}