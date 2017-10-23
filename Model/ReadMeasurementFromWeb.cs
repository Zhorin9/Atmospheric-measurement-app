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
        private readonly string UrlThingSpeakGetChannelField;
        private string _AmountOfSamples { get; set; }

        public ReadMeasurementFromWeb()
        {
            UrlThingSpeakGetChannelField = "https://api.thingspeak.com/channels/318552/feeds.json?results=";
        }
        public async Task<RootObject> ReadChannelField(int amountOfSamples)
        {
            string url = UrlThingSpeakGetChannelField + amountOfSamples;
            try
            {
                using (var _httpClient = new HttpClient())
                {
                    var json = await _httpClient.GetStringAsync(url);
                    var samples = await Task.Run(() => JsonConvert.DeserializeObject<RootObject>(json));
                    return samples;
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Wystąpił błąd, spróbuj ponownie");
                return null;
            }

        }
        public async Task<RootObject> ReadChannelLastMeasurements()
        {
            string url = UrlThingSpeakGetChannelField + "1";
            try
            {
                using (var _httpClient = new HttpClient())
                {
                    var json = await _httpClient.GetStringAsync(url);
                    var sample = JsonConvert.DeserializeObject<RootObject>(json);
                    return sample;
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Nie pobrano najnowszej próbki");
                return null;
            }

        }
    }
}
    
