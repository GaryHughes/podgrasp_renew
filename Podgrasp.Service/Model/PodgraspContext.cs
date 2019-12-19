using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;
using IdentityServer4.EntityFramework.Options;

namespace Podgrasp.Service.Model
{
    public class PodgraspContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public PodgraspContext(DbContextOptions<PodgraspContext> options,
                               IOptions<OperationalStoreOptions> operationalStoreOptions)
		:   base(options, operationalStoreOptions)
        {
        }

        public DbSet<Podcast> Podcasts { get; set; }
        
        public DbSet<Episode> Episodes { get; set; }
       
        public DbSet<PlaylistEpisode> Playlist { get; set; }

        public DbSet<UserPodcast> UserPodcasts { get; set; }

        public DbSet<UserEpisode> UserEpisodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Podcast>()
                .HasIndex(p => p.Url);

            modelBuilder.Entity<PlaylistEpisode>()
                .HasKey(pe => new { pe.UserId, pe.EpisodeId });

            modelBuilder.Entity<UserPodcast>()
                .HasKey(up => new { up.UserId, up.PodcastId });

            modelBuilder.Entity<UserEpisode>()
                .HasKey(ue => new { ue.UserId, ue.EpisodeId });
        }
    }
}