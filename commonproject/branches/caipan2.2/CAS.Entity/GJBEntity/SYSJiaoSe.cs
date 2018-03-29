using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_JiaoSe : SYSJiaoSe
    {
        [SQLReadOnly]
        public string subcompanyname { get; set; }
    }
}
