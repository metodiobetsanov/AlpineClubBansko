using MagicStrings;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Web.Models
{
    public class CreateCommentInputModel
    {
        [Display(Name = "Коментар", Prompt = Validations.Placeholder)]
        [Required(ErrorMessage = Validations.Required)]
        [StringLength(250, ErrorMessage = Validations.StringLength, MinimumLength = 10)]
        public string Content { get; set; }
    }
}