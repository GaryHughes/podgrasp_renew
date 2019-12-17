namespace Podgrasp.Service.Model
{
    public class UserPodcast
    {
        public string UserId { get; set; }
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }
    }
}