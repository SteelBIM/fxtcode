using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_CustomerCompany : DatCustomerCompany
    {
        [SQLReadOnly]
        public List<Dat_CustomerBranchCompany> customerbranchcompanylist;
        [SQLReadOnly]
        public int provinceid { get; set; }
        [SQLReadOnly]
        public string branchcompanycitys { get; set; }
        [SQLReadOnly]
        public string companyfullname{ get; set; }
    }
}
