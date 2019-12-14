using System;
using System.ComponentModel.DataAnnotations.Schema;
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
    }
}