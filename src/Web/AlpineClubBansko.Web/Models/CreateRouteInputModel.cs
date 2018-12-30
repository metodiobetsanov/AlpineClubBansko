using MagicStrings;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Web.Models
{
    public class CreateRouteInputModel
    {
        [Display(Name = "Заглавие", Prompt = Validations.Placeholder)]
        [Required(ErrorMessage = Validations.Required)]
        [StringLength(60, ErrorMessage = Validations.StringLength, MinimumLength = 10)]
        public string Title { get; set; }
    }
}