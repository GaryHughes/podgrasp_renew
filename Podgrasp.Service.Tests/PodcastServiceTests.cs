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
            Assert.IsFalse(service.Podcasts("bart").Any());
        }

        [TestMethod]
        public void TestSubscribeToPodcast()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Subscribe("bart", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            var podcasts = service.Podcasts("bart");
            Assert.AreEqual(1, podcasts.Length);
            Assert.AreEqual("http://www.podcast.com/feed.xml", podcasts[0].Url.ToString());
        }

        [TestMethod]
        public void TestPodcastsAreUserSpecific()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Subscribe("bart", new Subscription { Url = "http://www.podcast.com/feed.xml" });
            var podcasts = service.Podcasts("milhouse");
            Assert.AreEqual(0, podcasts.Length);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestSubscriptionWithMalformedUrlThrows()
        {
            var provider = new TestServiceProvider();
            var service = new PodcastService(provider);
            service.Subscribe("bart", new Subscription { Url = "blahblahblah" });
        }
    }
}
