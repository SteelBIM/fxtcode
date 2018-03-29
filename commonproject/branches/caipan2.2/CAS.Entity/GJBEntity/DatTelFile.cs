using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_TelFile:DatTelFile
    {
        [SQLReadOnly]
        public List<DatTelFileUser> telfileuserlist { get;set;}
    }
}
