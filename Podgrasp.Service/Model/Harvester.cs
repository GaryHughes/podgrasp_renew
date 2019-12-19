using System;

namespace Podgrasp.Service.Model
{
    public class Harvester
    {
        readonly PodcastService _podcastService;

        public Harvester(PodcastService podcastService)
        {
            _podcastService = podcastService;
        }   

        

    }
}