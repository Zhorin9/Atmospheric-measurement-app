using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringThesis.Model
{
    class Channel
    {
        //public int id { get; set; }
       // public string name { get; set; }
        //public string latitude { get; set; }
        //public string longitude { get; set; }
        [JsonProperty("field1")]
        public string TemperatureSenorDht{ get; set; }
        [JsonProperty("field2")]
        public string Humidity { get; set; }
        [JsonProperty("field3")]
        public string TemperatureSensorBmp { get; set; }
        [JsonProperty("field4")]
        public string Pressure { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        [JsonProperty("last_entry_id")]
        public int NumberOfMeasurement { get; set; }
    }
}
