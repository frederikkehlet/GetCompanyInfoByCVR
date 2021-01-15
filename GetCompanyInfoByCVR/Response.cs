using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GetCompanyInfoByCVR
{
    [JsonObject()]
    class Response
    {
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "virksomhedMetadata")]
        public dynamic VirksomhedMetaData { get; set; }
    }
}
