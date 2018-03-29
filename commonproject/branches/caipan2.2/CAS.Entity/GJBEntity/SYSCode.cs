using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_Code : SYSCode
    {
        public List<SYSCode> sublist = new List<SYSCode>();
        [SQLReadOnly]
        public List<Dat_CustomerCompany> customercompanylist;
        [SQLReadOnly]
        public string py
        {
            get;
            set;
        }
    }
}
