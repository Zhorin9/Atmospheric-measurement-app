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
        public DateTime CreatedData { get; private set; }
        private DateTime _Created_at;
        [JsonProperty("created_at")]
        public DateTime Created_at
        {
            get { return _Created_at; }
            set { _Created_at = value; AddHours(); }
        }
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

        public void AddHours()
        {
            var time = new TimeSpan(2, 0, 0);
            CreatedData = Created_at.Add(time);
        }
    }
}
