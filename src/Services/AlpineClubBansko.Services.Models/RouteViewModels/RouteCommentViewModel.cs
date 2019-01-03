using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Services.Models.RouteViewModels
{
    public class RouteCommentViewModel : IMapTo<RouteComment>, IMapFrom<RouteComment>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public User Author { get; set; }

        public string Comment { get; set; }

        public Route Route { get; set; }
    }
}
