using EngineeringThesis.Model.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringThesis.Model
{
    class RootObject
    {
        [JsonProperty("channel")]
        public Channel ChannelInformation { get; set; }
        [JsonProperty("feeds")]
        public List<Measurements> Measurements { get; set; }
    }
}

