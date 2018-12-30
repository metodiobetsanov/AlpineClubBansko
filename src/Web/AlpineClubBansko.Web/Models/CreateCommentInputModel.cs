using MagicStrings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
