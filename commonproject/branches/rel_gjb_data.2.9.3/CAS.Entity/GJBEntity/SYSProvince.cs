using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_Province:SYSProvince
    {
        [SQLReadOnly]
        public List<SYS_City> CityList { get; set; }
    }
}
