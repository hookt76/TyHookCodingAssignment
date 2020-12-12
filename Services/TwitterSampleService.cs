using AutoMapper;
using Newtonsoft.Json;
using System;
using TyHookCodingAssignment.Clients;
using TyHookCodingAssignment.Services.DTO;
using TyHookCodingAssignment.Services.SampledStream;

namespace TyHookCodingAssignment.Services
{
    public class TwitterSampleService
    {
        //private IMapper _iMapper;
        private Token _token;


        //the event handler
        public event EventHandler IncomingDataEvent;

        //event arg used to transport the data
        public class IncomingDataEventArgs : EventArgs
        {
            public SampledStreamDTO StreamDataResponse { get; set; }
        }
        protected void OnIncomingDataEvent(IncomingDataEventArgs IncomingDataEventArgs)
        {
            if (IncomingDataEvent == null)
                return;
            IncomingDataEvent(this, IncomingDataEventArgs);
        }

        /// <summary>
        /// Mapper
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="bearer"></param>
        public TwitterSampleService(string apiKey, string apiSecret,  string bearer)
        {
            _token = new Token { APIKey = apiKey, APISecret = apiSecret, BearerToken = bearer };

        }


        public void StartStream(string address, int timeOutMS)
        {
            try
            {
                TwitterSampleClient streamClient = new TwitterSampleClient();

                streamClient.StreamDataReceivedEvent += StreamClient_StreamIncomingDataEvent;

                streamClient.StartStream(address, timeOutMS, _token);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        private void StreamClient_StreamIncomingDataEvent(object sender, EventArgs e)
        {
            try
            {
                // convert to dto and model
                TwitterSampleClient.IncomingTweetEventArgs eventArgs = e as TwitterSampleClient.IncomingTweetEventArgs;
                SampledStreamDTO resultsDTO = JsonConvert.DeserializeObject<SampledStreamDTO>(eventArgs.StreamDataResponse);
                // SampledStreamModel model = _iMapper.Map<SampledStreamDTO, SampledStreamModel>(resultsDTO);

                // raise event with Model
                OnIncomingDataEvent(new IncomingDataEventArgs { StreamDataResponse = resultsDTO });
            }
            catch (Exception ex)
            {

                throw ex;
            }
            

        }
    }
}
