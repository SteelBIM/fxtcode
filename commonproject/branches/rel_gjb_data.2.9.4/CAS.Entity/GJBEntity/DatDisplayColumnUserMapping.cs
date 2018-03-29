using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_DisplayColumnUserMapping : DatDisplayColumnUserMapping
    {
        [SQLReadOnly]
        public string displayname { get; set; }
    }
}
