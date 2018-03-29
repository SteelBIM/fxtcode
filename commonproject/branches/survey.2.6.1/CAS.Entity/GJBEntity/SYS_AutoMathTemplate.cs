using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_AutoMathTemplate:DBEntity.SYSAutoMathTemplate
    {
        [SQLReadOnly]
        public string objecttypename { get; set; }
        [SQLReadOnly]
        public string createusername { get; set; }
        [SQLReadOnly]
        public string guid { get; set; }
        [SQLReadOnly]
        public string RTPinYin { get; set; }
    }
}
