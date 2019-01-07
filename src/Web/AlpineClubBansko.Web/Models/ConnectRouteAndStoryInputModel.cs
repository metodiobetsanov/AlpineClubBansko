using MagicStrings;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Web.Models
{
    public class ConnectRouteAndStoryInputModel
    {
        [Display(Name = "Преход")]
        [Required(ErrorMessage = Validations.Required)]
        public string RouteId { get; set; }

        [Display(Name = "История")]
        [Required(ErrorMessage = Validations.Required)]
        public string StoryId { get; set; }
    }
}