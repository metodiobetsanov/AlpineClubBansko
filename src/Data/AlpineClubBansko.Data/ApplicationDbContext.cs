using AlpineClubBansko.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlpineClubBansko.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<Story> Stories { get; set; }

        public DbSet<LikedStories> UsersLikedStories { get; set; }

        public DbSet<StoryComment> StoriesComments { get; set; }

        public DbSet<LikedAlbums> UsersLikedAlbums { get; set; }

        public DbSet<AlbumComment> AlbumsComments { get; set; }

        public DbSet<LikedRoutes> UsersLikedRoutes { get; set; }

        public DbSet<RouteComment> RoutesComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}