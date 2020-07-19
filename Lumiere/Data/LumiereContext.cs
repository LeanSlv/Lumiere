using Lumiere.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lumiere.Data
{
    public class LumiereContext : IdentityDbContext<User>
    {
        public LumiereContext(DbContextOptions<LumiereContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>(entity => entity.Property(m => m.Id).HasMaxLength(128));
            builder.Entity<IdentityUser>(entity => entity.Property(m => m.Email).HasMaxLength(128));
            builder.Entity<IdentityUser>(entity => entity.Property(m => m.NormalizedEmail).HasMaxLength(128));
            builder.Entity<IdentityUser>(entity => entity.Property(m => m.UserName).HasMaxLength(128));
            builder.Entity<IdentityUser>(entity => entity.Property(m => m.NormalizedUserName).HasMaxLength(128));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(128));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Name).HasMaxLength(128));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.NormalizedName).HasMaxLength(128));
            builder.Entity<User>(entity => entity.Property(m => m.Id).HasMaxLength(128));
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<FilmPoster> Posters { get; set; }
        public DbSet<FilmTrailer> Trailers { get; set; }
        public DbSet<FilmSeance> Seances { get; set; }
        public DbSet<ReservedSeat> ReservedSeats { get; set; }
        public DbSet<FilmFeedback> FilmFeedbacks { get; set; }
    }
}
