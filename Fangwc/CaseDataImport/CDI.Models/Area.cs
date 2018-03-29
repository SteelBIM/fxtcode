using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CDI.Models
{
    [DataContract]
    [Serializable]
    public class Area
    {
        [DataMember]
        public int AreaId { get; set; }
        [DataMember]
        public string AreaName { get; set; }
        [DataMember]
        public int CityId { get; set; }
    }
}
