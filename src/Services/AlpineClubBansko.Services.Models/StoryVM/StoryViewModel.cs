using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AlpineClubBansko.Services.Models.StoryVM
{
    public class StoryViewModel : IMapFrom<Story>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int Rating { get; set; }
    }
}
