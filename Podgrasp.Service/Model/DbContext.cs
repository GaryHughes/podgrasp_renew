using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Podgrasp.Service.Model
{
    public class PodgraspContext : DbContext
    {
        public PodgraspContext(DbContextOptions<PodgraspContext> options)
		:   base(options)
        {
        }

        public DbSet<Podcast> Podcasts { get; set; }
        
        public DbSet<Episode> Episodes { get; set; }

        public DbSet<PlaylistEpisode> Playlist { get; set; }
    }
}