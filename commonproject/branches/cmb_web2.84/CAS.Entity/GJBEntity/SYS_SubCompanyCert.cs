using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_SubCompanyCert : SYSSubCompanyCert
    {
        [SQLReadOnly]
        public string legalusername { get; set; }
    }
}
