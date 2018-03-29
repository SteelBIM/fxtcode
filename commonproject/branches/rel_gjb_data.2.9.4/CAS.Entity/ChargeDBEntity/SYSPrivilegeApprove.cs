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
        private int? _biztype;
        /// <summary>
        /// 业务类型
        /// </summary>
        public int? biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private int? _reporttype;
        /// <summary>
        /// 报告类型
        /// </summary>
        public int? reporttype
        {
            get { return _reporttype; }
            set { _reporttype = value; }
        }
        private string _reportsubtype;
        /// <summary>
        /// 报告子类型
        /// </summary>
        public string reportsubtype
        {
            get { return _reportsubtype; }
            set { _reportsubtype = value; }
        }
        private decimal? _discount;
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal? discount
        {
            get { return _discount; }
            set { _discount = value; }
        }
        private bool _valid = true;
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private bool _isuse = true;
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool isuse
        {
            get { return _isuse; }
            set { _isuse = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int? _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int? createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime? _updatedate;
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? updatedate
        {
            get { return _updatedate; }
            set { _updatedate = value; }
        }
        private int? _updateuserid;
        /// <summary>
        /// 更新人
        /// </summary>
        public int? updateuserid
        {
            get { return _updateuserid; }
            set { _updateuserid = value; }
        }

        private int? _chargetype;
        /// <summary>
        /// 收费方式
        /// </summary>
        public int? chargetype
        {
            get { return _chargetype; }
            set { _chargetype = value; }
        }
        private decimal? _amount;
        /// <summary>
        /// 一口价金额  Alex Add by 2015-09-06
        /// </summary>
        public decimal? amount
        {
            get { return _amount; }
            set { _amount = value; }
        }		
        private int _entruststage;
        /// <summary>
        /// 业务阶段：报告(2018006)or预评(2018005),为全部
        /// </summary>
        public int entruststage
        {
            get { return _entruststage; }
            set { _entruststage = value; }
        }
        private string _chargestandards;
        /// <summary>
        /// 对应的收费标准Id，0为全部
        /// </summary>
        public string chargestandards
        {
            get { return _chargestandards; }
            set { _chargestandards = value; }
        }
    }


    public class SYS_PrivilegeApprove : SYSPrivilegeApprove
    {
        public string biztypename { get; set; }
        public string reporttypename { get; set; }
        public string chargetypename { get; set; }
    }
}