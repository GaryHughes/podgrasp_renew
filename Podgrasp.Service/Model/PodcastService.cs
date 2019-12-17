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

        public Podcast[] Podcasts(string userId)
        {
            using var scope = _serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetService<PodgraspContext>();
            return context.Podcasts.Where(podcast => podcast.UserId == userId).ToArray();
        }

        public void Subscribe(string userId, Subscription subscription)
        {
            if (!Uri.TryCreate(subscription.Url, UriKind.Absolute, out var validUrl)) {
                throw new ArgumentException($"Invalid URL '{subscription.Url}'", nameof(subscription.Url));
            }

            var podcast = new Podcast {
                UserId = userId,
                Url = validUrl
            };

            using var scope = _serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetService<PodgraspContext>();
            context.Podcasts.Add(podcast);
            context.SaveChanges();
        }
    };
}