using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TyHookCodingAssignment.Services.DTO;

namespace TwitterSampleStreamTest
{
    [TestClass]
    public class UITest
    {
        /// <summary>
        /// Test to insure data is being mapped to the correct dictionary in 
        /// addition to this i would create a test for 
        /// each dictionary 
        /// </summary>
        [TestMethod]
        public void TestCollectData()
        {
            TwitterMetrics.Metric metric = new TwitterMetrics.Metric();

            metric._bagHashTags = new ConcurrentBag<Dictionary<string, int>>();
            metric._bagUrls = new ConcurrentBag<Dictionary<string, int>>();
            metric._bagPhotoUrl = new ConcurrentBag<Dictionary<string, int>>();
            metric._bagEmojis = new ConcurrentBag<Dictionary<string, int>>();
            metric._bagDomain = new ConcurrentBag<Dictionary<string, int>>();
            metric.hashTags = new Dictionary<string, int>();
            metric.urls = new Dictionary<string, int>();
            metric.photoUrl = new Dictionary<string, int>();
            metric.emojis = new Dictionary<string, int>();
            metric.domain = new Dictionary<string, int>();

            metric._bagHashTags.Add(metric.hashTags);

            SampledStreamDTO dto = new SampledStreamDTO
            {
                data = new Data { entities = new Entities { hagtags = new List<Hashtag>() } }
            };
            
            dto.data.entities.hagtags.Add(new Hashtag { tag = "#itworks" });
            metric.CollecteMetrics(dto);

            int value = 0;
            foreach (var item in metric._bagHashTags)
            {
                value = item["#itworks"];
            }

            Assert.AreEqual(1, value);


        }
    }
}
