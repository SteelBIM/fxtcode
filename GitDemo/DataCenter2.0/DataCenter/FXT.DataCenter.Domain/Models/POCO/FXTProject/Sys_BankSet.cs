using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Sys_BankSet
    {
        private int _id;
        /// <summary>
        /// 银行设置表
        /// </summary>
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _companyid;
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int? _departmentid;
        public int? departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
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
        private int _ischeck = 1;
        /// <summary>
        /// 是否需要审批
        /// </summary>
        public int ischeck
        {
            get { return _ischeck; }
            set { _ischeck = value; }
        }
        private decimal _checkprice = 0M;
        /// <summary>
        /// 审批生效条件：金额（万元）
        /// </summary>
        public decimal checkprice
        {
            get { return _checkprice; }
            set { _checkprice = value; }
        }
        private int _istax = 0;
        /// <summary>
        /// 是否需要计算税费
        /// </summary>
        public int istax
        {
            get { return _istax; }
            set { _istax = value; }
        }
        private int _requerydate = 2;
        /// <summary>
        /// 重新发起同笔业务询价间隔天数
        /// </summary>
        public int requerydate
        {
            get { return _requerydate; }
            set { _requerydate = value; }
        }
        private DateTime? _createdate;
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _creator;
        public string creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

    }
}
