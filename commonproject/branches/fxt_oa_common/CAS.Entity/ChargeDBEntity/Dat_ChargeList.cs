using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    public class Dat_EntrustCharge : BaseTO
    {

        private long _eid;
        /// <summary>
        /// 委托id
        /// </summary>
        [SQLField("eid", EnumDBFieldUsage.PrimaryKey, true)]
        public long eid
        {
            get { return _eid; }
            set { _eid = value; }
        }

        private long _entrustid;
        /// <summary>
        /// 流水号,业务编号(自动生成的,其他表关联)2013030001
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        /// <summary>
        /// 报告id
        /// </summary>
        public long reportid
        {
            get;
            set;
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
        /// 客户机构全称
        /// </summary>
        public string customercompanyfullname
        {
            get { return _customercompanyfullname; }
            set { _customercompanyfullname = value; }
        }

        private int? _businessuserid;
        /// <summary>
        /// 业务员
        /// </summary>
        public int? businessuserid
        {
            get { return _businessuserid; }
            set { _businessuserid = value; }
        }
        /// <summary>
        /// 业务员名称
        /// </summary>
        public string businessusername
        {
            get;
            set;
        }
        private string _reportno;
        /// <summary>
        /// 报告编号
        /// </summary>
        public string reportno
        {
            get { return _reportno; }
            set { _reportno = value; }
        }
        private decimal? _querytotalprice;
        /// <summary>
        /// 评估对象总价
        /// </summary>
        public decimal? querytotalprice
        {
            get { return _querytotalprice; }
            set { _querytotalprice = value; }
        }

        private decimal? _companystandardcharge;
        /// <summary>
        /// 公司标准收费
        /// </summary>
        public decimal? companystandardcharge
        {
            get { return _companystandardcharge; }
            set { _companystandardcharge = value; }
        }

        private decimal? _companymincharge;
        /// <summary>
        /// 公司最低收费
        /// </summary>
        public decimal? companymincharge
        {
            get { return _companymincharge; }
            set { _companymincharge = value; }
        }
        private decimal? _privilegediscount;
        /// <summary>
        /// 优惠折扣
        /// </summary>
        public decimal? privilegediscount
        {
            get { return _privilegediscount; }
            set { _privilegediscount = value; }
        }
        private decimal? _privilegemoney;
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? privilegemoney
        {
            get { return _privilegemoney; }
            set { _privilegemoney = value; }
        }
        private decimal? _receivable;
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal? receivable
        {
            get { return _receivable; }
            set { _receivable = value; }
        }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal? realityreceive
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get;
            set;
        }

        /// <summary>
        /// 结单时间
        /// </summary>
        public DateTime? overtime
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int statusvalue
        {
            get;
            set;
        }

        public string statusName
        {
            get;
            set;
        }
        /// <summary>
        /// 业务类型,对公/个人
        /// </summary>
        public int businesstype
        {
            get;
            set;
        }

        public string businessTypeName
        {
            get;
            set;

        }
        public int ReportTypeCode
        {
            get;
            set;
        }

        public string reportTypeName
        {
            get;
            set;
        }
    }
}
