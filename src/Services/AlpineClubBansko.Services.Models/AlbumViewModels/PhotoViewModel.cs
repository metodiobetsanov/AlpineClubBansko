using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using MagicStrings;
using System;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Services.Models.AlbumViewModels
{
    public class PhotoViewModel : IMapTo<Photo>, IMapFrom<Photo>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public User Author { get; set; }

        [Display(Name = "Заглавие")]
        [Required(ErrorMessage = Validations.Required)]
        [MinLength(5, ErrorMessage = Validations.MinLength)]
        public string Title { get; set; }

        [Display(Name = "Място/местност на заснемане")]
        [Required(ErrorMessage = Validations.Required)]
        [MinLength(3, ErrorMessage = Validations.MinLength)]
        public string Content { get; set; }

        [Display(Name = "Име на фотографа")]
        [Required(ErrorMessage = Validations.Required)]
        [MinLength(3, ErrorMessage = Validations.MinLength)]
        public string Photographer { get; set; }

        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public string LocationUrl { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}