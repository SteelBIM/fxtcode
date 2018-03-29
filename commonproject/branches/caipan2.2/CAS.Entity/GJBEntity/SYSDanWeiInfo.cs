using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_DanWeiInfo : SYSDanWeiInfo
    {
        [SQLReadOnly]
        public List<SYS_BuMen> bumenlist { get; set; }
    }
}
