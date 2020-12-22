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
        /// Test to insure calculations are correct
        /// addition to this i would create a test for 
        /// each dictionary with a negative test
        /// </summary>
        [TestMethod]
        public void TestGetDataByDictionaryHashTag()
        {
            TwitterMetrics.Metric metric = new TwitterMetrics.Metric();


            metric.hashTagsDict = new ConcurrentDictionary<string, int>() ;
            metric.urlsDict = new ConcurrentDictionary<string, int>();
            metric.photoUrlDict = new ConcurrentDictionary<string, int>();
            metric.emojisDict = new ConcurrentDictionary<string, int>();
            metric.domainDict = new ConcurrentDictionary<string, int>();

            SampledStreamDTO dto = new SampledStreamDTO
            {
                data = new Data { entities = new Entities { hagtags = new List<Hashtag>() , urls = new List<Url>()}  }
            };
            
            dto.data.entities.hagtags.Add(new Hashtag { tag = "#itworks" });
            dto.data.text = "DataText";
            dto.data.entities.urls.Add(new Url { expanded_url = "http://tyreehook.com" });
            metric.CollecteMetrics(dto);

            int value = 0;
            if(metric.hashTagsDict.ContainsKey("#itworks") && metric.urlsDict.ContainsKey("http://tyreehook.com"))
            {
                value = 1;
            }
            else
            {
                value = 0;
            }

            string text = "";
            bool result = metric.metricsBag.TryPeek(out text);

            Assert.AreEqual(text, dto.data.text);
            Assert.AreEqual(value,1);


        }

        [TestMethod]
        public void TestGetDataByDictionaryEmojiPrecent()
        {
            TwitterMetrics.Metric metric = new TwitterMetrics.Metric();

            metric.emojisDict = new ConcurrentDictionary<string, int>();

            SampledStreamDTO dto = new SampledStreamDTO
            {
                data = new Data { entities = new Entities { hagtags = new List<Hashtag>() } }
            };

            metric.emojisDict[":)"] = 1;

            metric.CollecteMetrics(dto);

            int value = 0;
            if (metric.GetPrecentageByCategory(metric.emojisDict) ==  "100")
            {
                value = 1;
            }
            else
            {
                value = 0;
            }

            Assert.AreEqual(1, value);


        }
        [TestMethod]
        public void TestGetDataByDictionaryTopEmoji()
        { 

            TwitterMetrics.Metric metric = new TwitterMetrics.Metric();

            metric.emojisDict = new ConcurrentDictionary<string, int>();

            SampledStreamDTO dto = new SampledStreamDTO
            {
                data = new Data { entities = new Entities { hagtags = new List<Hashtag>() } }
            };

            metric.emojisDict[":)"] = 1;

            metric.CollecteMetrics(dto);

            int value = 0;
            if (metric.GetTopItemByCategory(metric.emojisDict) == ":)")
            {
                value = 1;
            }
            else
            {
                value = 0;
            }

            Assert.AreEqual(1, value);


        }
        [TestMethod]
        public void TestGetDataByDictionaryDomainPrecent()
        {
            TwitterMetrics.Metric metric = new TwitterMetrics.Metric();

            metric.emojisDict = new ConcurrentDictionary<string, int>();

            SampledStreamDTO dto = new SampledStreamDTO
            {
                data = new Data { entities = new Entities { hagtags = new List<Hashtag>() } }
            };

            metric.domainDict["Http://TyreeHook.com"] = 1;

            metric.CollecteMetrics(dto);

            int value = 0;
            if (metric.GetPrecentageByCategory(metric.domainDict) == "100")
            {
                value = 1;
            }
            else
            {
                value = 0;
            }

            Assert.AreEqual(1, value);


        }
        [TestMethod]
        public void TestGetDataByDictionaryTopDomain()
        {

            TwitterMetrics.Metric metric = new TwitterMetrics.Metric();

            metric.domainDict = new ConcurrentDictionary<string, int>();

            SampledStreamDTO dto = new SampledStreamDTO
            {
                data = new Data { entities = new Entities { hagtags = new List<Hashtag>() } }
            };

            metric.domainDict["Http://TyreeHook.com"] = 2;
            metric.domainDict["Http://TyreeHooklessValue.com"] = 1;

            metric.CollecteMetrics(dto);

            int value = 0;
            if (metric.GetTopItemByCategory(metric.domainDict) == "Http://TyreeHook.com")
            {
                value = 1;
            }
            else
            {
                value = 0;
            }

            Assert.AreEqual(1, value);


        }
        [TestMethod]
        public void TestGetDataByDictionaryHashTagPrecent()
        {
            TwitterMetrics.Metric metric = new TwitterMetrics.Metric();

            metric.domainDict = new ConcurrentDictionary<string, int>();

            SampledStreamDTO dto = new SampledStreamDTO
            {
                data = new Data { entities = new Entities { hagtags = new List<Hashtag>(), urls = new List<Url>() } }
            };

            dto.data.entities.hagtags.Add(new Hashtag { tag = "#itworks" });
            dto.data.text = "DataText";
            dto.data.entities.urls.Add(new Url { expanded_url = "http://tyreehook.com" });
            metric.CollecteMetrics(dto);


            int value = 0;
            if (metric.GetPrecentageByCategory(metric.domainDict) == "100")
            {
                value = 1;
            }
            else
            {
                value = 0;
            }

            Assert.AreEqual(1, value);


        }
        [TestMethod]
        public void TestGetDataByDictionaryTopHashTag()
        {

            TwitterMetrics.Metric metric = new TwitterMetrics.Metric();

            metric.hashTagsDict = new ConcurrentDictionary<string, int>();

            metric.hashTagsDict["#TyreeHook2"] = 2;
            metric.hashTagsDict["#TyreeHook1"] = 1;

            int value = 0;
            if (metric.GetTopItemByCategory(metric.hashTagsDict) == "#TyreeHook2")
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
