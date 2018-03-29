using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.Business")]
    public class Business : BaseTO
    {
        private int _businessid;
        /// <summary>
        /// 业务ID
        /// </summary>
        [SQLField("businessid", EnumDBFieldUsage.PrimaryKey, true)]
        public int businessid
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
        private int? _service;
        /// <summary>
        /// 服务类型
        /// </summary>
        public int? service
        {
            get { return _service; }
            set { _service = value; }
        }
        private string _projectname;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private string _customercompanyfullname;
        /// <summary>
        /// 委估机构名称
        /// </summary>
        public string customercompanyfullname
        {
            get { return _customercompanyfullname; }
            set { _customercompanyfullname = value; }
        }
        private int? _cityid;
        /// <summary>
        /// 项目城市
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _remark;
        /// <summary>
        /// 需求描述
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int? _limittime;
        /// <summary>
        /// 限定时间(单位：小时)
        /// </summary>
        public int? limittime
        {
            get { return _limittime; }
            set { _limittime = value; }
        }
        private decimal? _price;
        /// <summary>
        /// 报价
        /// </summary>
        public decimal? price
        {
            get { return _price; }
            set { _price = value; }
        }
        private bool? _valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }

}
