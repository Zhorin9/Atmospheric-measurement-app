using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringThesis.Model.Json
{
    class Measurements
    {        
        [JsonProperty("created_at")]
        public DateTime created_at { get; set; }
        [JsonProperty("entry_id")]
        public int entry_id { get; set; }
        [JsonProperty("field1")]
        public string TemperatureDht { get; set; }
        [JsonProperty("field2")]
        public string Humidity { get; set; }
        [JsonProperty("field3")]
        public string TemperatureBmp { get; set; }
        [JsonProperty("field4")]
        public string Pressure { get; set; }        
    }
}
