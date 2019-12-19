using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Podgrasp.Service.Model
{
    public class PodcastService
    {
        readonly IServiceProvider _serviceProvider;

        #region Events

        public class PodcastEventArgs : EventArgs
        {
            public PodcastEventArgs(Podcast podcast)
            {
                Podcast = podcast;
            }

            public Podcast Podcast { get; }
        }

        public class UserPodcastEventArgs : EventArgs
        {
            public UserPodcastEventArgs(UserPodcast userPodcast)
            {
                UserPodcast = userPodcast;
            }

            public UserPodcast UserPodcast { get; }
        }

        public delegate void PodcastHandler(object sender, PodcastEventArgs eventArgs);
        public delegate void UserPodcastHandler(object sender, UserPodcastEventArgs eventArgs);

        public event PodcastHandler PodcastAdded;
        public event PodcastHandler PodcastUpdated;
        public event PodcastHandler PodcastRemoved;

        public event UserPodcastHandler PodcastSubscribed;
        public event UserPodcastHandler PodcastUnsubscribed;
        
        protected void OnPodcastAdded(Podcast podcast)
        {
            PodcastAdded?.Invoke(this, new PodcastEventArgs(podcast));
        }

        protected void OnPodcastUpdated(Podcast podcast)
        {
            PodcastUpdated?.Invoke(this, new PodcastEventArgs(podcast));
        }

        protected void OnPodcastRemoved(Podcast podcast)
        {
            PodcastRemoved?.Invoke(this, new PodcastEventArgs(podcast));
        }

        protected void OnPodcastSubscribed(UserPodcast userPodcast)
        {
            PodcastSubscribed?.Invoke(this, new UserPodcastEventArgs(userPodcast));
        }

        protected void OnPodcastUnsubscribed(UserPodcast userPodcast)
        {
            PodcastUnsubscribed?.Invoke(this, new UserPodcastEventArgs(userPodcast));
        }

        #endregion

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
                OnPodcastAdded(podcast);
            }

            var userPodcast = (from up in context.UserPodcasts
                               where up.UserId == userId && up.PodcastId == podcast.Id
                               select up).FirstOrDefault();

            bool subscribed = false;

            if (userPodcast is null) {
                userPodcast = new UserPodcast {
                    UserId = userId,
                    PodcastId = podcast.Id,
                    Podcast = podcast
                };
                context.UserPodcasts.Add(userPodcast);
                subscribed = true;
            }

            context.SaveChanges();

            if (subscribed) {
                OnPodcastSubscribed(userPodcast);
            }
        }

        public void Unsubscribe(string userId, string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var validUrl)) {
                throw new ArgumentException($"Invalid URL '{url}'", nameof(url));
            }

            using var scope = _serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetService<PodgraspContext>();
        
            var podcast = context.Podcasts.FirstOrDefault(podcast => podcast.Url == validUrl);

            if (podcast is null) {
                throw new ArgumentException($"Unknown URL '{url}'", nameof(url));
            }

            var userPodcast = (from up in context.UserPodcasts
                               where up.UserId == userId && up.PodcastId == podcast.Id
                               select up).FirstOrDefault();

            if (userPodcast is null) {
                throw new ArgumentException($"User is not subscribed to '{url}'", nameof(url));
            }

            context.UserPodcasts.Remove(userPodcast);
            context.SaveChanges();
            OnPodcastUnsubscribed(userPodcast);
        }

    };
}