using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;

namespace CAS.Entity.GJBEntity
{
    public class SYS_Code : SYSCode
    {
        public List<SYSCode> sublist = new List<SYSCode>();
    }
}
