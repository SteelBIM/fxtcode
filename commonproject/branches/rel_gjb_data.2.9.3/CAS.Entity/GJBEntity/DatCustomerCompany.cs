using System.Collections.Generic;
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
        /// <summary>
        /// 分行或支行数
        /// </summary>
        [SQLReadOnly]
        public int bccount { get; set; }
    }
}
