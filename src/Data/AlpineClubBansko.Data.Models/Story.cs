using AlpineClubBansko.Services.Mapping.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlpineClubBansko.Data.Models
{
    public class Story : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public virtual Album Album { get; set; }

        public virtual Route Route { get; set; }

        public int Rating { get; set; }
    }
}