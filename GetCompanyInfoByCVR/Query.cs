using System.Runtime.Serialization;

namespace GetCompanyInfoByCVR
{
    [DataContract(Name = "query")]
    public class Query
    {
        [DataMember(Name = "term")]
        public Term Term { get; set; }  
        public Query(Term term) { Term = term; }
    }

    [DataContract(Name = "term")]
    public class Term
    {
        [DataMember(Name = "Vrvirksomhed.cvrNummer")]
        public string CVRNumber { get; set; }
        
        public Term(string cvrNumber)
        {
            CVRNumber = cvrNumber;
        }
    }
}
