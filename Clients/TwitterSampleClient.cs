using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Timers;

namespace TyHookCodingAssignment.Clients
{
    public class TwitterSampleClient
    {
        bool _keepAlive = true;

        //the event handler
        public event EventHandler StreamDataReceivedEvent;

        //event arg used to transport the data 
        public class IncomingTweetEventArgs : EventArgs
        {
            public string StreamDataResponse { get; set; }
        }

        //event used to subsribe to to receive events that add to it
        protected void OnStreamDataReceivedEvent(IncomingTweetEventArgs inComingDataEventArgs)
        {
            if (StreamDataReceivedEvent == null)
                return;
            StreamDataReceivedEvent(this, inComingDataEventArgs);
        }

        //starts the stream and loops through for 5 seconds the returns
        //adds events from the stream 
        public void StartStream(string address, int timeOutMS, Token key)
        {
            Timer _timer = new Timer(timeOutMS);
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _timer.Enabled = true;
            _keepAlive = true;

            try
            {
                //set web request authorization header and pass bearer for access to twitter stream
                //use get method to get stream
                WebRequest webRequest = WebRequest.Create(address);
                webRequest.Headers.Add("Authorization", "Bearer " + key.BearerToken);
                webRequest.Method = "GET";
                webRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

                try
                {
                    HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //using statement to open stream and also dispose stream once the block of coded is exited
                        using (StreamReader str = new StreamReader(response.GetResponseStream()))
                        {
                            do
                            {
                                string json = str.ReadLine();

                                if (!string.IsNullOrEmpty(json))
                                {
                                    // raise event as the stream pushes reocrds
                                    OnStreamDataReceivedEvent(new IncomingTweetEventArgs { StreamDataResponse = json });
                                }
                            }
                            while (_keepAlive && System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()
                                   && !str.EndOfStream);                         
                        }
                    }
                    else
                    {
                        //write out to file or event log in DB with response
                    }

                }
                catch (WebException ex)
                {
                    //write out to file or event log in DB
                    throw ex;
                }
                catch (Exception ex)
                {
                    //write out to file or event log in DB
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                //write out to file or event log in DB
                throw ex;
            }

        }
        //used to stop the loop of the stream helps with rate limiting
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _keepAlive = false;
        }

    }
}
