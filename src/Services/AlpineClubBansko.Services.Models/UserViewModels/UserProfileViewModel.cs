using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using AlpineClubBansko.Services.Models.StoryViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Services.Models.UserViewModels
{
    public class UserProfileViewModel : IMapTo<User>, IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Avatar { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public int PostCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public Level Level { get; set; }

        public List<StoryViewModel> Stories { get; set; }

        public List<AlbumViewModel> Albums { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            return;
        }
    }
}
