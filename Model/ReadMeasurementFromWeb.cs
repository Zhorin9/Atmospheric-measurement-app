using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace EngineeringThesis.Model
{
    class ReadMeasurementFromWeb
    {
        private string UrlThingSpeakAmountOfMeasurements { get; }
        private string UrlThingSpeakGetChannelField { get; }
        private string amountOfSamples { get; set; }

        public ReadMeasurementFromWeb()
        {
            UrlThingSpeakAmountOfMeasurements = "https://api.thingspeak.com/update?api_key=2DNWLFBAKDV8B2EH&field2=0";
            UrlThingSpeakGetChannelField = "https://api.thingspeak.com/channels/318552/feeds.json?results=";
        }      
        
        public bool ReadAmountOfFeeds()
        {
            while (amountOfSamples == "0" || amountOfSamples == null)
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        amountOfSamples = webClient.DownloadString(UrlThingSpeakAmountOfMeasurements);
                    }  
                }                
                catch (WebException)
                {
                    MessageBox.Show("Wystąpił błąd odczytu, spróbuj ponownie");
                    return false;
                }
            }
            return true;
        }
        
        public async Task<RootObject> ReadChannelField()
        {
            string url = UrlThingSpeakGetChannelField  + amountOfSamples;

            using (var _httpClient = new HttpClient())
            {
                var json = await _httpClient.GetStringAsync(url);
                var samples = await Task.Run( () =>  JsonConvert.DeserializeObject<RootObject>(json));
                
                return  samples;

            }
        }
        public async Task<RootObject> ReadChannelLastMeasurements()
        {
            string url = UrlThingSpeakGetChannelField + "1";
            using (var _httpClient = new HttpClient())
            {
                var json = await _httpClient.GetStringAsync(url);
                var sample = JsonConvert.DeserializeObject<RootObject>(json);
                return sample;
            }
        }
    }
}
    
