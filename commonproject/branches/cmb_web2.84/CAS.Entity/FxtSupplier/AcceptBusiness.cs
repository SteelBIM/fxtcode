using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.AcceptBusiness")]
    public class AcceptBusiness : BaseTO
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
        /// 供应商id
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int? _businessid;
        /// <summary>
        /// 业务Id
        /// </summary>
        public int? businessid
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
        private int? _state;
        /// <summary>
        /// 业务受理状态:0-拒绝；1-完成；2-未受理;3-任务处理中
        /// </summary>
        public int? state
        {
            get { return _state; }
            set { _state = value; }
        }
        private DateTime? _businessdate;
        /// <summary>
        /// 业务处理时间
        /// </summary>
        public DateTime? businessdate
        {
            get { return _businessdate; }
            set { _businessdate = value; }
        }
        private string _remark;
        /// <summary>
        /// 业务描述
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int? _cityid;
        /// <summary>
        /// 业务所属城市
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
    }
}
