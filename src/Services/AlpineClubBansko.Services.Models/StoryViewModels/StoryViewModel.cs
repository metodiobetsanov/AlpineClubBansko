using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Services.Models.StoryViewModels
{
    public class StoryViewModel : IMapTo<Story>, IMapFrom<Story>
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int Rating { get; set; }
    }
}
