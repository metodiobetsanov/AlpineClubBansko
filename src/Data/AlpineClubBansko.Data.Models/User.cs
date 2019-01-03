using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public virtual ICollection<Story> Stories { get; set; }
        [Required]
        public virtual ICollection<Route> Routes { get; set; }
        [Required]
        public virtual ICollection<Album> Albums { get; set; }

        [Required]
        public virtual ICollection<LikedStories> StoriesLiked { get; set; }
        [Required]
        public virtual ICollection<LikedRoutes> RoutesLiked { get; set; }
        [Required]
        public virtual ICollection<LikedAlbums> AlbumsLiked { get; set; }

        [Required]
        public virtual ICollection<StoryComment> StoriesComments { get; set; }
        [Required]
        public virtual ICollection<RouteComment> RoutesComments { get; set; }
        [Required]
        public virtual ICollection<AlbumComment> AlbumsComments { get; set; }
    }
}