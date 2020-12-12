using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterMetrics.Model
{
    public class AverageTweets
    {
        public double TweetsPerSecond { get; set; }
        public double TweetsPerMinute { get; set; }
        public double TweetsPerHour { get; set; }
    }
}
