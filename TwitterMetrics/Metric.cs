using NeoSmart.Unicode;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitterMetrics.Model;
using TyHookCodingAssignment.Services;
using TyHookCodingAssignment.Services.DTO;

namespace TwitterMetrics
{
    public partial class Metric : Form
    {
        private ConcurrentBag<Model.Metrics> metricsBag = new ConcurrentBag<Model.Metrics>();
        private Control _control = new Control();
        private int _twitterCount = 0;
        private bool _isStop = false;
        public ConcurrentBag<Dictionary<string, int>> _bagHashTags = new ConcurrentBag<Dictionary<string, int>>();
        public ConcurrentBag<Dictionary<string, int>> _bagUrls = new ConcurrentBag<Dictionary<string, int>>();
        public ConcurrentBag<Dictionary<string, int>> _bagPhotoUrl = new ConcurrentBag<Dictionary<string, int>>();
        public ConcurrentBag<Dictionary<string, int>> _bagEmojis = new ConcurrentBag<Dictionary<string, int>>();
        public ConcurrentBag<Dictionary<string, int>> _bagDomain = new ConcurrentBag<Dictionary<string, int>>();
        public Dictionary<string, int> hashTags = new Dictionary<string, int>();
        public Dictionary<string, int> urls = new Dictionary<string, int>();
        public Dictionary<string, int> photoUrl = new Dictionary<string, int>();
        public Dictionary<string, int> emojis = new Dictionary<string, int>();
        public Dictionary<string, int> domain = new Dictionary<string, int>();

        Stopwatch _timer = new Stopwatch();

        public Metric()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //initialize dictionaries and add them to their perspective
            //the dictionaries are being added to the bag to allow concurrent use
            //of the dictionaries

            _bagHashTags.Add(hashTags);
            _bagUrls.Add(urls);
            _bagPhotoUrl.Add(photoUrl);
            _bagEmojis.Add(emojis);
            _bagDomain.Add(domain);

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //starts streaming service and returns focus back to form
            _isStop = false;
            Task<int> stream = StartStream();
            stream.ConfigureAwait(false);

            Task<int> display = DisplayData();
            display.ConfigureAwait(false);

            btnStart.Enabled = false;

            _timer.Start();

        }

        private void TwitterSampleService_IncomingDataEvent(object sender, EventArgs e)
        {
            _twitterCount += 1;
            TwitterSampleService.IncomingDataEventArgs eventArgs = e as TwitterSampleService.IncomingDataEventArgs;
            SampledStreamDTO model = eventArgs.StreamDataResponse;
            string jsonString = JsonConvert.SerializeObject(model.data);
            this.CollecteMetrics(model);

        }

        private void btnStopStream_Click(object sender, EventArgs e)
        {
            //stop stream from processing 
            _isStop = true;
            _timer.Stop();
            btnStart.Enabled = true;
        }

        /// <summary>
        /// used to get tweets from the sample twitter stream, excute on a seperate thread once the start button is clicked
        /// </summary>
        /// <returns></returns>
        private async Task<int> StartStream()
        {
            //get settings from app config
            string APIKey = Properties.Settings.Default.APIKey;
            string APISecret = Properties.Settings.Default.APISecret;
            string BearerToken = Properties.Settings.Default.BearerToken;
            string address = Properties.Settings.Default.Address;
            int timeOutMs = Properties.Settings.Default.TimeOutMS;

            int result = 0;
            //try to start stream via the service
            try
            {
                await Task.Run(() =>
                {
                    while (!_isStop)
                    {
                        TwitterSampleService twitterSampleService = new TwitterSampleService(APIKey, APISecret, BearerToken);
                        twitterSampleService.IncomingDataEvent += TwitterSampleService_IncomingDataEvent;
                        twitterSampleService.StartStream(address, timeOutMs);
                        _timer.Stop();
                        Thread.Sleep(30000);
                        _timer.Start();
                    };

                });

                return result;
            }
            catch (Exception ex)
            {
                result = 1;
                MessageBox.Show(ex.Message);
                btnStart.Enabled = true;
                return result;
            }

        }
        /// <summary>
        /// called on a seperate this method provides data to the UI every 2 seconds
        /// </summary>
        /// <returns></returns>
        private async Task<int> DisplayData()
        {
            int result = 0;
            try
            {
                await Task.Run(() =>
                {
                    while (!_isStop)
                    {

                        foreach (var item in metricsBag)
                        {

                            this.UIThread(() => this.lblTotalTweets.Text = _twitterCount.ToString());
                            this.UIThread(() => this.lblPerSecond.Text = GetAverageCount().TweetsPerSecond.ToString());
                            this.UIThread(() => this.lblPerMinute.Text = GetAverageCount().TweetsPerMinute.ToString());
                            this.UIThread(() => this.lblPerHour.Text = GetAverageCount().TweetsPerHour.ToString());
                            this.UIThread(() => this.lblEmojiPrecent.Text = GetEmojiPrecentage() + "%");
                            this.UIThread(() => this.lblPhotoPrecent.Text = GetPhotoPrecentage() + "%"); 
                            this.UIThread(() => this.lblURLPrecent.Text = GetURLPrecentage() + "%"); 
                            this.UIThread(() => this.lblTopDomain.Text = GetTopDomain());
                            this.UIThread(() => this.lblTopEmoji.Text = GetTopEmoji());
                            this.UIThread(() => this.lblTopHashTag.Text = GetTopHashTag() + "%");
                        }
                        Thread.Sleep(2000);
                    };

                });

                return result;
            }
            catch (Exception ex)
            {
                result = 1;
                MessageBox.Show(ex.Message);
                btnStart.Enabled = true;
                return result;
            }

        }
        /// <summary>
        /// as the incoming gets an event this method is used to populate the control we are using for memory storage 
        /// </summary>
        /// <param name="model"></param>
        public void CollecteMetrics(SampledStreamDTO model)
        {
            try
            {
                if (model != null)
                {
                    foreach (var item in _bagHashTags)
                    {
                        if (model.data != null && model.data.entities != null && model.data.entities.hagtags != null)
                        {
                            foreach (var dictItem in model.data.entities.hagtags)
                            {
                                if (item.ContainsKey(dictItem.tag))
                                {
                                    item[dictItem.tag]++;
                                }
                                else
                                {
                                    item.Add(dictItem.tag, 1);
                                }
                            }
                        }
                    }

                    foreach (var item in _bagUrls)
                    {
                        if (model.data.entities != null && model.data.entities.urls != null)
                        {
                            foreach (var dictItem in model.data.entities.urls)
                            {
                                if (dictItem.expanded_url != null)
                                {
                                    if (item.ContainsKey(dictItem.expanded_url))
                                    {
                                        item[dictItem.expanded_url]++;
                                    }
                                    else
                                    {
                                        item.Add(dictItem.expanded_url, 1);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var photo in _bagPhotoUrl)
                    {
                        if (model.includes != null && model.includes.Media != null)
                        {
                            foreach (var item in model.includes.Media)
                            {
                                if (item.url != null)
                                {
                                    if (photo.ContainsKey(item.url.AbsoluteUri))
                                    {
                                        photo[item.url.AbsoluteUri]++;
                                    }
                                    else
                                    {
                                        photo.Add(item.url.AbsoluteUri, 1);
                                    }
                                }
                            }
                        }
                        
                    }

                    foreach (var domain in _bagDomain) 
                    {
                        if (model.data != null && model.data.entities != null && model.data.entities.urls != null && model.data.entities.urls.Count() > 0 )
                        {
                            foreach (var item in model.data.entities.urls)
                            {
                                if (item.expanded_url != null)
                                {
                                    if (domain.ContainsKey(item.expanded_url))
                                    {
                                        domain[item.expanded_url]++;
                                    }
                                    else
                                    {
                                        domain.Add(item.expanded_url, 1);
                                    }
                                }
                            }
                        }

                    }

                    foreach (var emoji in _bagEmojis)
                    {

                        if (model.data.text != null && Emoji.IsEmoji(model.data.text))
                        {
                            if (emoji.ContainsKey(model.data.text))
                            {
                                emoji[model.data.text]++;
                            }
                            else
                            {
                                emoji.Add(model.data.text, 1);
                            }

                        }

                    }

                    Model.Metrics metrics = new Model.Metrics();
                    metrics.Domain = model.data.text;

                    metricsBag.Add(metrics);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

        }
       
        /// <summary>
        /// returns the total number of tweets
        /// </summary>
        /// <returns></returns>
        private int GetTwitterCount()
        {

            int result = 0;
            if (metricsBag != null)
            {
                result = metricsBag.Count();
            }
            return result;
        }

        /// <summary>
        /// calculates the rate of return by sec, minute and hour
        /// </summary>
        /// <returns></returns>
        private AverageTweets GetAverageCount()
        {
            int totalCount = GetTwitterCount();

            AverageTweets averageTweet = new AverageTweets
            {
                TweetsPerSecond = totalCount / (_timer.ElapsedMilliseconds / 1000),
                TweetsPerMinute = totalCount / (_timer.ElapsedMilliseconds / 1000) * 60,
                TweetsPerHour = totalCount / (_timer.ElapsedMilliseconds / 1000) * 3600
            };

            return averageTweet;
        }

        /// <summary>
        /// meant to use linq to find the top hashtag reporting on Domain, needed to figure out how to get the count
        /// so i could have selected the hashTag with the max number of counts
        /// </summary>
        /// <returns></returns>
        private string GetTopHashTag()
        {
            string result = string.Empty;
            var topHashTag =
                from tag in metricsBag
                group tag by tag.HashTags into newGroup
                orderby newGroup.Key
                select newGroup.FirstOrDefault();

            foreach (var item in topHashTag)
            {
                result = item.Domain;
            }

            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTopDomain()
        {
            string result = "None to report";
            foreach (var item in _bagDomain)
            {
                if (item.Count() > 0)
                {
                    result = item.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                }
            }
            return result;
        }


        /// <summary>
        /// Get Top Emoji
        /// </summary>
        /// <returns></returns>
        public string GetTopEmoji()
        {
            string result = "None to report";
            foreach (var item in _bagEmojis)
            {
                if (item.Count() > 0)
                {
                    result = item.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                }
            }
            return result;
        }

        /// <summary>
        /// calculates the precentage of photo url
        /// </summary>
        /// <returns></returns>
        private string GetPhotoPrecentage()
        {
            string result = "0";
            foreach (var item in _bagPhotoUrl)
            {
                if (metricsBag.Count() > 0)
                {
                    double precentage = (double)item.Count() / (double)metricsBag.Count();
                    precentage = precentage * 100;
                    result = precentage.ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// calculates the precentage of wteets with emoji
        /// </summary>
        /// <returns></returns>
        private string GetEmojiPrecentage()
        {
            string result = "0";
            foreach (var item in _bagEmojis)
            {
                if (metricsBag.Count() > 0)
                {
                    double precentage = (double)item.Count() / (double)metricsBag.Count();
                    precentage = precentage * 100;
                    result = precentage.ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// calculates the precentage of wteets with emoji
        /// </summary>
        /// <returns></returns>
        private string GetURLPrecentage()
        {
            string result = "0";
            foreach (var item in _bagUrls)
            {
                if (metricsBag.Count() > 0)
                {
                    double precentage = (double)item.Count() / (double)metricsBag.Count();
                    precentage = precentage * 100;
                    result = precentage.ToString();
                }
            }
            return result;
        }

    }

    /// <summary>
    /// extention class used to allow a threads to update the UI on the main thread
    /// </summary>
    public  static partial class ControlExtensions
    {
        /// Used to update controls on the main thread.
        public static void UIThread(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
