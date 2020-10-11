using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atelier.Port.Models
{
    public class Identification_Panne
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Identification_panne { set; get; }
    }
    public class Identification_Photos
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Identification_photos { set; get; }
    }
    public class ItemRemorque
    {
        public Guid _id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Numero_Remorque { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Nom_Client { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Email_Client { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Identification_Panne Pannes { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Identification_Photos Photos { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Envoi_Email_Client { set; get; }
    }
}