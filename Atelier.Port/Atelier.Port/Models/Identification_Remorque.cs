using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atelier.Port.Models
{
    public class Identification_Remorque
    {
        public Guid _id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Numero_Remorque { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Nom_Client { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Email_Client { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Identification_Photos Photos { get; set; }


    }
}
