using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Podgrasp.Service.Model
{
    public class Podcast
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Uri Url { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public Uri ImageUrl { get; set; }
        public DbSet<Episode> Episodes { get; set; }
    }
}