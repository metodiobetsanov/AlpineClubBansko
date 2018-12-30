using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AlpineClubBansko.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            CreatedOn = DateTime.UtcNow;
        }

        public DateTime CreatedOn { get; set; }

        public string Avatar { get; set; }

        [ProtectedPersonalData] public string FirstName { get; set; }

        [ProtectedPersonalData] public string LastName { get; set; }

        [ProtectedPersonalData] public int PostCode { get; set; }

        [ProtectedPersonalData] public string City { get; set; }

        [ProtectedPersonalData] public string Country { get; set; }

        public virtual Level Level { get; set; }

        public virtual ICollection<UsersEvents> EventsParticipations { get; set; }

        public virtual ICollection<Story> Stories { get; set; }

        public virtual ICollection<Route> Routes { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<LikedStories> StoriesLiked { get; set; }
    }
}