using MagicStrings;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Web.Models
{
    public class ConnectAlbumAndStoryInputModel
    {
        [Display(Name = "Албум")]
        [Required(ErrorMessage = Validations.Required)]
        public string AlbumId { get; set; }

        [Display(Name = "История")]
        [Required(ErrorMessage = Validations.Required)]
        public string StoryId { get; set; }
    }
}