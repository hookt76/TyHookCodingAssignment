using System;
using System.Collections.Generic;
using System.Text;

namespace TyHookCodingAssignment.Services.SampledStream
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SampledStreamModel
    {
        public Data data { get; set; }
        public Includes includes { get; set; }
    }

    public partial class ReferencedTweet
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    public partial class Url
    {
        public int start { get; set; }
        public int end { get; set; }
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public int status { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class Annotation
    {
        public int start { get; set; }
        public int end { get; set; }
        public double probability { get; set; }
        public string type { get; set; }
        public string normalized_text { get; set; }
    }

    public partial class Entities
    {
        public List<Url> urls { get; set; }
        public List<Annotation> annotations { get; set; }
    }

    public partial class Stats
    {
        public int retweet_count { get; set; }
        public int reply_count { get; set; }
        public int like_count { get; set; }
        public int quote_count { get; set; }
    }

    public partial class Domain
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Entity
    {
        public object id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public partial class ContextAnnotation
    {
        public Domain domain { get; set; }
        public Entity entity { get; set; }
    }

    public partial class Data
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public string text { get; set; }
        public string author_id { get; set; }
        public string in_reply_to_user_id { get; set; }
        public List<ReferencedTweet> referenced_tweets { get; set; }
        public Entities entities { get; set; }
        public Stats stats { get; set; }
        public bool possibly_sensitive { get; set; }
        public string lang { get; set; }
        public string source { get; set; }
        public List<ContextAnnotation> context_annotations { get; set; }
        public string format { get; set; }
    }

    public partial class Includes
    {
        public List<Media> media { get; set; }

        public List<User> users { get; set; }
    }

    public partial class Media
    {
        public Uri url { get; set; }

        public long width { get; set; }

        public string media_Key { get; set; }

        public long height { get; set; }

        public string type { get; set; }
    }

    public partial class User
    {
        public DateTimeOffset created_at { get; set; }

        public string id { get; set; }

        public string username { get; set; }

        public string name { get; set; }
    }
}
