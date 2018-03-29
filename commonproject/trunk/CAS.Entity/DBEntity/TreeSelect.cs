using System;

namespace CAS.Entity.DBEntity
{
    /*
      用于树形下拉框
      */
    [Serializable]
    public class TreeSelect
    {
        public string id { get; set; }
        public string value { get; set; }
        public string name { get; set; }
        public int? parentid { get; set; }
    }
}
