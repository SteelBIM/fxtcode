using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DataConv.Model
{
    [DataContract]
    public class SysCode
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string CodeName { get; set; }
        [DataMember]
        public string CodeType { get; set; }
        [DataMember]
        public string Remark { get; set; }
        [DataMember]
        public int SubCode { get; set; }
    }
}
