using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_City:SYSCity
    {
        [SQLReadOnly]
        public List<SYS_Area> AreaList {get;set; }
        [SQLReadOnly]
        public string provincename { get; set; }
    }
}
