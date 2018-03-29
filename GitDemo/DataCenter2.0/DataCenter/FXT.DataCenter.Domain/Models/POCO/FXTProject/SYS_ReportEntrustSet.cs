using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_ReportEntrustSet
    {
        private int _id;
        /// <summary>
        /// 委托规则
        /// </summary>
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 客户公司ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _departmentid;
        /// <summary>
        /// 客户部门ID
        /// </summary>
        public int departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        private int _entrusttype = 9003002;
        /// <summary>
        /// 委托规则
        /// </summary>
        public int entrusttype
        {
            get { return _entrusttype; }
            set { _entrusttype = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 规则是否生效
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
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
        private string _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public string createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime? _savedate;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? savedate
        {
            get { return _savedate; }
            set { _savedate = value; }
        }
        private string _saveuserid;
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string saveuserid
        {
            get { return _saveuserid; }
            set { _saveuserid = value; }
        }
        private int _replynumber = 0;
        /// <summary>
        /// 条件：回复询价的评估机构数量
        /// </summary>
        public int replynumber
        {
            get { return _replynumber; }
            set { _replynumber = value; }
        }
        private decimal _replypercent = 0M;
        /// <summary>
        /// 条件：回复询价的评估机构数量百分比
        /// </summary>
        public decimal replypercent
        {
            get { return _replypercent; }
            set { _replypercent = value; }
        }
        private int _starttype = 0;
        /// <summary>
        /// 委托发起类型：0直接委托，1从询价中委托
        /// </summary>
        public int starttype
        {
            get { return _starttype; }
            set { _starttype = value; }
        }
        private int _biztype = 1026001;
        /// <summary>
        /// 业务类型，个人、公司..
        /// </summary>
        public int biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }

    }
}
