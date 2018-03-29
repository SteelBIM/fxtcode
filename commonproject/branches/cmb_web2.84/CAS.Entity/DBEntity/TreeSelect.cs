using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.DBEntity
{
    /*
      用于树形下拉框
      */
    [Serializable]
    public class TreeSelect
    {
        public string id;
        public string value;
        public string name;
        public int? parentid;
    }
}
