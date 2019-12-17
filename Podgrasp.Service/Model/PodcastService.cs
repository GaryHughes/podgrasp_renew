using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Podgrasp.Service.Model
{
    public class PodcastService
    {
        readonly IServiceProvider _serviceProvider;
        
        public PodcastService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Podcast[] Podcasts()
        {
            using var scope = _serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetService<PodgraspContext>();
            return context.Podcasts.ToArray();
        }

        public Podcast[] SubscribedPodcasts(string userId)
        {
            using var scope = _serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetService<PodgraspContext>();
            
            var podcasts = from up in context.UserPodcasts 
                           where up.UserId == userId
                           select up.Podcast;
            
            return podcasts.ToArray();
        }

        public void Subscribe(string userId, Subscription subscription)
        {
            if (!Uri.TryCreate(subscription.Url, UriKind.Absolute, out var validUrl)) {
                throw new ArgumentException($"Invalid URL '{subscription.Url}'", nameof(subscription.Url));
            }

            using var scope = _serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetService<PodgraspContext>();
         
            var podcast = context.Podcasts.FirstOrDefault(podcast => podcast.Url == validUrl);

            if (podcast is null) {
                podcast = new Podcast {
                    Url = validUrl
                };
                context.Podcasts.Add(podcast);
            }

            var userPodcast = (from up in context.UserPodcasts
                               where up.UserId == userId && up.Podcast == podcast
                               select up).FirstOrDefault();

            if (userPodcast is null) {
                userPodcast = new UserPodcast {
                    UserId = userId,
                    PodcastId = podcast.Id,
                    Podcast = podcast
                };
                context.UserPodcasts.Add(userPodcast);
            }

            context.SaveChanges();
        }

    };
}