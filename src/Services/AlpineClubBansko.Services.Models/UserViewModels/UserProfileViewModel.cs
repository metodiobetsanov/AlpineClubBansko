﻿using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Mapping.Contracts;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using AlpineClubBansko.Services.Models.RouteViewModels;
using AlpineClubBansko.Services.Models.StoryViewModels;
using System;
using System.Collections.Generic;

namespace AlpineClubBansko.Services.Models.UserViewModels
{
    public class UserProfileViewModel : IMapTo<User>, IMapFrom<User>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Avatar { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public int PostCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public List<StoryViewModel> Stories { get; set; }

        public List<AlbumViewModel> Albums { get; set; }

        public List<RouteViewModel> Routes { get; set; }

        public int Points => (Stories.Count * 5) + (Albums.Count * 20) + (Routes.Count * 10);
    }
}