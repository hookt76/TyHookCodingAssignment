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
        public void TestCollectDataForHashTag()
        {
            TwitterMetrics.Metric metric = new TwitterMetrics.Metric();


            metric.hashTagsDict = new ConcurrentDictionary<string, int>() ;
            metric.urlsDict = new ConcurrentDictionary<string, int>();
            metric.photoUrlDict = new ConcurrentDictionary<string, int>();
            metric.emojisDict = new ConcurrentDictionary<string, int>();
            metric.domainDict = new ConcurrentDictionary<string, int>();

            SampledStreamDTO dto = new SampledStreamDTO
            {
                data = new Data { entities = new Entities { hagtags = new List<Hashtag>() } }
            };
            
            dto.data.entities.hagtags.Add(new Hashtag { tag = "#itworks" });
            metric.CollecteMetrics(dto);

            int value = 0;
            if(metric.hashTagsDict.ContainsKey("#itworks"))
            {
                value = 1;
            }
            else
            {
                value = 0;
            }

            Assert.AreEqual(1, value);


        }
    }
}
