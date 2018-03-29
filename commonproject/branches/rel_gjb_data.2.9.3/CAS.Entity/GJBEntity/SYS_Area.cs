using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_Area : SYSArea
    {
        [SQLReadOnly]
        public List<SYSSubArea> subarealist { get; set; }
        [SQLReadOnly]
        public bool isparent { get; set; }
    }
}
