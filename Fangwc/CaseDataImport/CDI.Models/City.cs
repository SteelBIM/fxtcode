using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CDI.Models
{
    [DataContract]
    [Serializable]
    public class City
    {
        [DataMember]
        public int CityID { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public int ProvinceId { get; set; }
        [DataMember]
        public Dictionary<string, int> AreaMap { get; set; }
        [DataMember]
        public string CaseTable { get; set; }
        [DataMember]
        public string ProjectTable { get; set; }
    }
}
