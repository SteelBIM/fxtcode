using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_DisplayColumn : DatDisplayColumn
    {
        [SQLReadOnly]
        public int mappingid { get; set; }
        [SQLReadOnly]
        public int sortid { get; set; }
    }
}
