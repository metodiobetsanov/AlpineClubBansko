using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Services.Models.RouteViewModels
{
    public class LocationViewModel : IMapTo<Location>, IMapFrom<Location>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual User Author { get; set; }

        public string Name { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string RouteId { get; set; }

        public override string ToString()
        {
            return $"{this.Name}: {Latitude} , {Longitude}";
        }
    }
}
