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

        public DbSet<Event> Events { get; set; }

        public DbSet<UsersEvents> UsersEvents { get; set; }

        public DbSet<Level> Levels { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<Story> Stories { get; set; }

        public DbSet<RoutesAlbums> RoutesAlbums { get; set; }

        public DbSet<RoutesEvents> RoutesEvents { get; set; }

        public DbSet<RoutesStories> RoutesStories { get; set; }

        public DbSet<LikedStories> UsersLikedStories { get; set; }
    }
}