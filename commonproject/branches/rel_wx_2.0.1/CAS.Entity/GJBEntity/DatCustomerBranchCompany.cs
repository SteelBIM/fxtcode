using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_CustomerBranchCompany : DatCustomerBranchCompany
    {
        [SQLReadOnly]
        public string cityname { get; set; }
        [SQLReadOnly]
        public List<Dat_CustomerBranchCompany> subcustomerbranchcompanylist;
        [SQLReadOnly]
        public int provinceid { get; set; }

        [SQLReadOnly]
        public string customercompanyname { get; set; }
    }
}
