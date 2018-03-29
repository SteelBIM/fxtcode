using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_City:SYSCity
    {
        [SQLReadOnly]
        public List<SYSArea> AreaList {get;set; }
    }
}
