using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Podgrasp.Service.Model
{
    public class Episode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        
        // These properties come from the feed
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime DatePublished { get; set; }
        public Uri AudioUrl { get; set; }
        public Podcast Podcast { get; set; }

        // These properties are derived
        public TimeSpan Duration { get; set; }
        public TimeSpan Position { get; set; }
    }
}