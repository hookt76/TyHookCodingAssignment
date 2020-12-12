using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterMetrics.Model
{
    public class Metrics
    {
        public string Emoji { get; set; }
        public double PrencentageOfEmoji { get; set; }
        public string HashTags { get; set; }
        public string URL { get; set; }
        public string PhotoUrl { get; set; }
        public string Domain { get; set; }

    }
}
