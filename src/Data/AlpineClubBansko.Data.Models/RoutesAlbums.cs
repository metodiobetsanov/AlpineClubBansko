namespace AlpineClubBansko.Data.Models
{
    public class RoutesAlbums
    {
        public string Id { get; set; }

        public string RouteId { get; set; }
        public virtual Route Route { get; set; }

        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }
    }
}