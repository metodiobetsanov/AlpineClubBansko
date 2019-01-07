using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using MagicStrings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Services.Models.RouteViewModels
{
    public class RouteViewModel : IMapTo<Route>, IMapFrom<Route>
    {
        public string Id { get; set; }

        [Display(Name = "Заглавие", Prompt = Validations.Placeholder)]
        [Required(ErrorMessage = Validations.Required)]
        [StringLength(60, ErrorMessage = Validations.StringLength, MinimumLength = 10)]
        public string Title { get; set; }

        [Display(Name = "Съдържание")]
        [Required(ErrorMessage = Validations.Required)]
        [MinLength(10, ErrorMessage = Validations.MinLength)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Време за преминаване")]
        [Required(ErrorMessage = Validations.Required)]
        [RegularExpression(@"(\d дни), (([0-1][0-9]|[2][0-3]) часа)", ErrorMessage = Validations.TimeNeeded)]
        public string TimeNeeded { get; set; }

        public User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Story Story { get; set; }

        public Album Album { get; set; }

        public List<LocationViewModel> Locations { get; set; }

        public List<RouteCommentViewModel> Comments { get; set; }

        public List<LikedRoutes> Favorite { get; set; }

        public int Views { get; set; }
    }
}