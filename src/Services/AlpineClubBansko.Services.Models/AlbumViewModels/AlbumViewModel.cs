using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using MagicStrings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AlpineClubBansko.Services.Models.AlbumViewModels
{
    public class AlbumViewModel : IMapTo<Album>, IMapFrom<Album>
    {
        public string Id { get; set; }
        [Display(Name = "Заглавие", Prompt = Validations.Placeholder)]
        [Required(ErrorMessage = Validations.Required)]
        [StringLength(60, ErrorMessage = Validations.StringLength, MinimumLength = 10)]
        public string Title { get; set; }

        [Display(Name = "Кратко описание")]
        [Required(ErrorMessage = Validations.Required)]
        [MinLength(10, ErrorMessage = Validations.MinLength)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public string Cover
        {
            get
            {
                if (Photos == null || Photos.Count == 0)
                {
                    return string.Empty;
                }

                return Photos.First().LocationUrl;
            }
        }

        [Display(Name = "Заснето място")]
        [Required(ErrorMessage = Validations.Required)]
        [MinLength(3, ErrorMessage = Validations.MinLength)]
        public string Place { get; set; }

        public List<PhotoViewModel> Photos { get; set; }

        public User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public List<AlbumCommentViewModel> Comments { get; set; }

        public List<LikedAlbums> Favorite { get; set; }

        public int Views { get; set; }
    }
}