using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.Audit")]
    public class Audit : BaseTO
    {
        private int _id;
        /// <summary>
        /// id
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private DateTime _submittime;
        /// <summary>
        /// 提交审核时间
        /// </summary>
        public DateTime submittime
        {
            get { return _submittime; }
            set { _submittime = value; }
        }
        private DateTime? _audittime;
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? audittime
        {
            get { return _audittime; }
            set { _audittime = value; }
        }
        private int? _valid;
        /// <summary>
        /// 是否通过审核:0-审核失败；-1：未提交审核资料；2：审核中；1：审核通过；
        /// </summary>
        public int? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _remark;
        /// <summary>
        /// 说明
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }

}
