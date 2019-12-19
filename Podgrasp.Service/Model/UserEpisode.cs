using System;

namespace Podgrasp.Service.Model
{
    public class UserEpisode
    {
        public string UserId { get; set; }
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
        public TimeSpan Position { get; set; }
    }
}