using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Services.Models.StoryViewModels
{
    public class StoryCommentViewModel : IMapTo<StoryComment>, IMapFrom<StoryComment>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public User Author { get; set; }

        public string Comment { get; set; }

        public Story Story { get; set; }

        public string CommentType { get; set; }
    }
}
