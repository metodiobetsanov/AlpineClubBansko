using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using MagicStrings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Services.Models.StoryViewModels
{
    public class StoryViewModel : IMapTo<Story>, IMapFrom<Story>
    {
        public string Id { get; set; }

        [Display(Name = "Заглавие", Prompt = Validations.Placeholder)]
        [Required(ErrorMessage = Validations.Required)]
        [StringLength(60, ErrorMessage = Validations.StringLength, MinimumLength = 10)]
        public string Title { get; set; }

        [Display(Name = "Съдържание")]
        [Required(ErrorMessage = Validations.Required)]
        [MinLength(60, ErrorMessage = Validations.MinLength)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public List<StoryCommentViewModel> StoryComments { get; set; }

        public List<LikedStories> Favorite { get; set; }

        public int Views { get; set; }

    }
}