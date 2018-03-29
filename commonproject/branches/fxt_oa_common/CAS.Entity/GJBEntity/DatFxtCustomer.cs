using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class DatFxtCustomer : CAS.Entity.DBEntity.DatFxtCustomer
    {
        [SQLReadOnly]
        public string provincename { get; set; }
        [SQLReadOnly]
        public string cityname { get; set; }
        [SQLReadOnly]
        public string businesslevelname { get; set; }
        [SQLReadOnly]
        public string marketingtypename { get; set; }
        [SQLReadOnly]
        public string createusername { get; set; }
        [SQLReadOnly]
        public DateTime? lastupdatedate { get; set; }
        [SQLReadOnly]
        public string lastupdateremark { get; set; }        
        [SQLReadOnly]
        public string csusername { get; set; }
        [SQLReadOnly]
        public string custsysadminname { get; set; }
        [SQLReadOnly]
        public string custsysadminphone { get; set; }
        [SQLReadOnly]
        public string custsysadminpqq { get; set; }
    }
}
