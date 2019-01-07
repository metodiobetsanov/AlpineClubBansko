using System.ComponentModel.DataAnnotations.Schema;

namespace AlpineClubBansko.Data.Models
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }

        [Column(TypeName = "decimal(11,8)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(11, 8)")]
        public decimal Longitude { get; set; }

        public string RouteId { get; set; }
        public virtual Route Route { get; set; }
    }
}