using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private DateTime _aptitudetype;
        /// <summary>
        /// 提交审核时间
        /// </summary>
        public DateTime aptitudetype
        {
            get { return _aptitudetype; }
            set { _aptitudetype = value; }
        }
        private DateTime? _aptitudename;
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? aptitudename
        {
            get { return _aptitudename; }
            set { _aptitudename = value; }
        }
        private int? _valid;
        /// <summary>
        /// 是否通过审核(-1)
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
