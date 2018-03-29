using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_NWorkBusinessErrors : DatNWorkBusinessErrors
    {
        [SQLReadOnly]
        public string typename { get; set; }
    }
}
