using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_RedHead : DatRedHead
    {
        [SQLReadOnly]
        public string guid { get; set; }
    }
}
