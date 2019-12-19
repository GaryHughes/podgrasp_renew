using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Podgrasp.Service.Model;

namespace Podgrasp.Service.Tests
{
    [TestClass]
    public class PodcastServiceTests
    {
        [TestMethod]
        public void TestDefaultService()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            Assert.IsFalse(service.SubscribedPodcasts("bart").Any());
            Assert.IsFalse(service.Podcasts().Any());
        }

        [TestMethod]
        public void TestSubscribeToPodcast()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Subscribe("bart", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            var podcasts = service.SubscribedPodcasts("bart");
            Assert.AreEqual(1, podcasts.Length);
            Assert.AreEqual("http://www.podcast.com/feed.xml", podcasts[0].Url.ToString());
            Assert.AreEqual(1, service.Podcasts().Length);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestUnsubscribeWithMalformedUrlThrows()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Unsubscribe("bart", "blahblahblah" );
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestUnsubscribeToPodcastUserIsNotSubscribedTo()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Subscribe("bart", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            service.Unsubscribe("nelson", "http://www.podcast.com/feed.xml"); 
        }

        [TestMethod]
        public void TestUnsubscribeFromPodcast()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Subscribe("bart", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            var podcasts = service.SubscribedPodcasts("bart");
            Assert.AreEqual(1, podcasts.Length);
            Assert.AreEqual("http://www.podcast.com/feed.xml", podcasts[0].Url.ToString());
            Assert.AreEqual(1, service.Podcasts().Length);
            service.Unsubscribe("bart", "http://www.podcast.com/feed.xml");
            podcasts = service.SubscribedPodcasts("bart");
            Assert.AreEqual(0, podcasts.Length);
            Assert.AreEqual(1, service.Podcasts().Length);
        }

        [TestMethod]
        public void TestSubscriptionsAreUserSpecific()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Subscribe("bart", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            var podcasts = service.SubscribedPodcasts("milhouse");
            Assert.AreEqual(0, podcasts.Length);
            Assert.AreEqual(1, service.Podcasts().Length);
        }

        [TestMethod]
        public void TestMultipleSubscriptionsDontDuplicatePodcasts()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Subscribe("bart", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            service.Subscribe("nelson", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            Assert.AreEqual(1, service.Podcasts().Length);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestSubscriptionWithMalformedUrlThrows()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Subscribe("bart", new Subscription { Url = "blahblahblah" });
        }

        [TestMethod]
        public void TestPodcastAddedEvent()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            Podcast added = null;
            service.PodcastAdded += (sender, ev) => {
                added = ev.Podcast;
            };
            service.Subscribe("bart", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            Assert.IsNotNull(added);
            Assert.AreEqual("http://www.podcast.com/feed.xml", added.Url.ToString());
        }

        [TestMethod]
        public void TestPodcastUpdatedEvent()
        {
        }

        [TestMethod]
        public void TestPodcastRemovedEvent()
        {
        }

        [TestMethod]
        public void TestPodcastSubscribedEvent()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            UserPodcast subscribed = null;
            service.PodcastSubscribed += (sender, ev) => {
                subscribed = ev.UserPodcast;
            };
            service.Subscribe("bart", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            Assert.IsNotNull(subscribed);
            Assert.AreEqual("bart", subscribed.UserId);
            Assert.AreEqual("http://www.podcast.com/feed.xml", subscribed.Podcast.Url.ToString());
        }

        [TestMethod]
        public void TestPodcastUnsubscribedEvent()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Subscribe("bart", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            UserPodcast unsubscribed = null;
            service.PodcastUnsubscribed += (sender, ev) => {
                unsubscribed = ev.UserPodcast;
            };
            service.Unsubscribe("bart", "http://www.podcast.com/feed.xml");
            Assert.IsNotNull(unsubscribed);
            Assert.AreEqual("bart", unsubscribed.UserId);
            Assert.AreEqual("http://www.podcast.com/feed.xml", unsubscribed.Podcast.Url.ToString());
        }

    }
}
