using System.Collections.Generic;
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
        /// <summary>
        /// 部门
        /// </summary>
        [SQLReadOnly]
        public string bumenname { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        [SQLReadOnly]
        public string truename { get; set; }
        /// <summary>
        /// 分支行名称拼音首字母
        /// </summary>
        [SQLReadOnly]
        public string branchpy { get; set; }
    }
}
