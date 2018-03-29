using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Customer_Fxt
    {
        private int _id;
        /// <summary>
        /// 评估机构入围银行关系
        /// </summary>
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _companyid;
        /// <summary>
        /// 入围公司(银行)
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _departmentid;
        /// <summary>
        /// 入围分公司（部门）
        /// </summary>
        public int departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        private DateTime? _stratdate;
        /// <summary>
        /// 入围起始时间
        /// </summary>
        public DateTime? stratdate
        {
            get { return _stratdate; }
            set { _stratdate = value; }
        }
        private DateTime? _enddate;
        /// <summary>
        /// 入围结束时间
        /// </summary>
        public DateTime? enddate
        {
            get { return _enddate; }
            set { _enddate = value; }
        }
        private int _typecode = 1026001;
        /// <summary>
        /// 业务类型,银行按个人、公司分（1026）,法院国资委按房地产、土地、资产分（1019）
        /// </summary>
        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private int _companytypecode = 2001013;
        /// <summary>
        /// 入围公司类别（银行、中介、按揭）2001013、2001023、2001012
        /// </summary>
        public int companytypecode
        {
            get { return _companytypecode; }
            set { _companytypecode = value; }
        }

    }
}
