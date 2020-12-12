using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TyHookCodingAssignment.Clients;
using TyHookCodingAssignment.Services;

namespace TwitterSampleStreamTest
{
    [TestClass]
    public class ServiceTest
    {


        /// <summary>
        /// not actual test more like used to debug stream
        /// </summary>
        [TestMethod]
        public void TestClient()
        {
            Token key = new Token
            {
                APIKey = "D1ymPWHCxpkPOeNIrXZmNjXxZ",
                APISecret = "",
                BearerToken = "AAAAAAAAAAAAAAAAAAAAAOluKQEAAAAAIzRc6a4zvMqFqqpMx5%2FUH3hYhv0%3DkRzPRlz6D8YkzJcsyg1ld2vsOqhMJMBKo2zOumc7Edf6gNaLFY"
            };
            TwitterSampleClient twitterClient = new TwitterSampleClient();
            twitterClient.StartStream("https://api.twitter.com/2/tweets/sample/stream?expansions=attachments.poll_ids,attachments.media_keys,author_id,entities.mentions.username,geo.place_id,in_reply_to_user_id,referenced_tweets.id,referenced_tweets.id.author_id&user.fields=created_at&media.fields=duration_ms,height,media_key,preview_image_url,type,url,width,public_metrics&place.fields=contained_within,country,country_code,full_name,geo,id,name,place_type&poll.fields=duration_minutes,end_datetime,id,options,voting_status&tweet.fields=attachments,author_id,context_annotations,conversation_id,created_at,entities,geo,id,in_reply_to_user_id,lang,public_metrics,possibly_sensitive,referenced_tweets,reply_settings,source,text,withheld", 600000,key);
        }

        /// <summary>
        /// not actual test more like used to debug stream
        /// </summary>
        [TestMethod]
        public void TestService()
        {
            Token key = new Token
            {
                APIKey = "D1ymPWHCxpkPOeNIrXZmNjXxZ",
                APISecret = "",
                BearerToken = "AAAAAAAAAAAAAAAAAAAAAOluKQEAAAAAIzRc6a4zvMqFqqpMx5%2FUH3hYhv0%3DkRzPRlz6D8YkzJcsyg1ld2vsOqhMJMBKo2zOumc7Edf6gNaLFY"
            };
            TwitterSampleService twitterService = new TwitterSampleService(key.APIKey,key.APISecret,key.BearerToken);
            twitterService.StartStream("https://api.twitter.com/2/tweets/sample/stream?expansions=attachments.poll_ids,attachments.media_keys,author_id,entities.mentions.username,geo.place_id,in_reply_to_user_id,referenced_tweets.id,referenced_tweets.id.author_id&user.fields=created_at&media.fields=duration_ms,height,media_key,preview_image_url,type,url,width,public_metrics&place.fields=contained_within,country,country_code,full_name,geo,id,name,place_type&poll.fields=duration_minutes,end_datetime,id,options,voting_status&tweet.fields=attachments,author_id,context_annotations,conversation_id,created_at,entities,geo,id,in_reply_to_user_id,lang,public_metrics,possibly_sensitive,referenced_tweets,reply_settings,source,text,withheld", 2000);
        }

    }

}