using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;
using CAS.Entity.DBEntity;

namespace CAS.Entity.GJBEntity
{
    public class SYS_QueryApprovalUser : SYSQueryApprovalUser
    {
        [SQLReadOnly]
        public string subcompanyname { get; set; }
        [SQLReadOnly]
        public string entrysttypename { get; set; }
        [SQLReadOnly]
        public string querytypename { get; set; }
    }
    public class QueryApprovalUserCompare : IEqualityComparer<SYS_QueryApprovalUser>
    {
        public bool Equals(SYS_QueryApprovalUser x, SYS_QueryApprovalUser y)
        {
            if (x.subcompanyid == y.subcompanyid && x.entrusttypecode == y.entrusttypecode && x.businesstypeid==y.businesstypeid)       //分别对属性进行比较
                return true;
            else
                return false;   
        }

        public int GetHashCode(SYS_QueryApprovalUser obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
