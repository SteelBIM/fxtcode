using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class DatFxtCustomerFollowup : CAS.Entity.DBEntity.DatFxtCustomerFollowup
    {
        [SQLReadOnly]
        public string createusername { get; set; }
        [SQLReadOnly]
        public string followuptypename { get; set; }
        [SQLReadOnly]
        public string customername { get; set; }
        [SQLReadOnly]
        public string provincename { get; set; }
        [SQLReadOnly]
        public string cityname { get; set; }
        
    }
}
