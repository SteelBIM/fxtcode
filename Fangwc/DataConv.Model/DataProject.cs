using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DataConv.Model
{
    [DataContract]
    public class DataProject
    {
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string PinYin { get; set; }
        [DataMember]
        public string OtherName { get; set; }
        [DataMember]
        public int CityID { get; set; }
        [DataMember]
        public string PinYinAll { get; set; }
        [DataMember]
        public string OtherPinyin { get; set; }
        [DataMember]
        public string OtherPinyinAll { get; set; }

    }
}
