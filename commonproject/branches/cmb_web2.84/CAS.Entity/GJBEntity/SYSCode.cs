using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_Code : SYSCode
    {
        public List<SYSCode> sublist = new List<SYSCode>();
        [SQLReadOnly]
        public List<Dat_CustomerCompany> customercompanylist;
    }
}
