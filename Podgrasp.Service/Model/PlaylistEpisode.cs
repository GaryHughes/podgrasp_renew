using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Podgrasp.Service.Model
{
    public class PlaylistEpisode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public Episode Episode { get; set; }
    }
}