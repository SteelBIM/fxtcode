using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_CodeMapping : DatCodeMapping
    {
        [SQLReadOnly]
        public string codename { get; set; }
    }
}
