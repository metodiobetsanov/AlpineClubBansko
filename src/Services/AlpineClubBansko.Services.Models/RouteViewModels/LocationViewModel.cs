using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using MagicStrings;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AlpineClubBansko.Services.Models.RouteViewModels
{
    [DataContract]
    public class LocationViewModel : IMapTo<Location>, IMapFrom<Location>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public User Author { get; set; }

        [Display(Name = "Име на местност")]
        [Required(ErrorMessage = Validations.Required)]
        [StringLength(20, ErrorMessage = Validations.StringLength, MinimumLength = 3)]
        [DataMember]
        public string Name { get; set; }

        [Display(Name = "Г. ширина", Prompt = Validations.Placeholder)]
        [Required(ErrorMessage = Validations.Required)]
        [DataMember]
        public decimal Latitude { get; set; }

        [Display(Name = "Г. дължина")]
        [Required(ErrorMessage = Validations.Required)]
        [DataMember]
        public decimal Longitude { get; set; }

        public string RouteId { get; set; }
    }
}