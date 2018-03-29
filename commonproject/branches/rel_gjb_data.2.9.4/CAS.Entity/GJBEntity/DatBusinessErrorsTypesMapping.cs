using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_BusinessErrorsTypesMapping : DatBusinessErrorsTypesMapping
    {
        [SQLReadOnly]
        public string errordescript { get; set; }
        [SQLReadOnly]
        public string typename { get; set; }
    }
}
