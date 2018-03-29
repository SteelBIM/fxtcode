using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_PrivilegeApprove")]
    public class SYSPrivilegeApprove : BaseTO
    {
        private int _privilegeapproveid;
        [SQLField("privilegeapproveid", EnumDBFieldUsage.PrimaryKey, true)]
        public int privilegeapproveid
        {
            get { return _privilegeapproveid; }
            set { _privilegeapproveid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司Id
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _approvetype;
        /// <summary>
        /// 不需要审批
        ///全部需要审批
        ///低于最低收费才审批
        ///自定义审批条件
        /// </summary>
        public int approvetype
        {
            get { return _approvetype; }
            set { _approvetype = value; }
        }
    }
}