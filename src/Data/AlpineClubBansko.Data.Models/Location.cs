using System.Runtime.Serialization;

namespace AlpineClubBansko.Data.Models
{
    [DataContract]
    public class Location : BaseEntity
    {
        public string Name { get; set; }

        [DataMember]
        public decimal Latitude { get; set; }

        [DataMember]
        public decimal Longitude { get; set; }

        public string RouteId { get; set; }
        public virtual Route Route { get; set; }
    }
}