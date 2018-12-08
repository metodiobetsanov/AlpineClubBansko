using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

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

        public Level Level { get; set; }

        public virtual List<Story> Stories { get; set; }

        public virtual List<Album> Albums { get; set; }

        public virtual List<Route> Routes { get; set; }
    }
}