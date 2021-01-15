using System.Runtime.Serialization;
using System.Collections.Generic;

namespace GetCompanyInfoByCVR
{
    [DataContract()]
    public class Request
    {
        [DataMember(Name = "_source")]
        public List<string> Source { get; set; }

        [DataMember(Name = "query")]
        public Query Query { get; set; }

        public Request(List<string> source, Query query)
        {
            Source = source;
            Query = query;
        }
    }
}
