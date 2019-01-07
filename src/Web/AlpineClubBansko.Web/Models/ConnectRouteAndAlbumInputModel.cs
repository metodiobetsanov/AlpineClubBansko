using MagicStrings;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Web.Models
{
    public class ConnectRouteAndAlbumInputModel
    {
        [Display(Name = "Преход")]
        [Required(ErrorMessage = Validations.Required)]
        public string RouteId { get; set; }

        [Display(Name = "Албум")]
        [Required(ErrorMessage = Validations.Required)]
        public string AlbumId { get; set; }
    }
}