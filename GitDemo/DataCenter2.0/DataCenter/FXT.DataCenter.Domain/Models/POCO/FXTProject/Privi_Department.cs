using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Department
    {
        private int _departmentid;
        //[SQLField("departmentid", EnumDBFieldUsage.PrimaryKey, true)]
        public int departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        private int _fk_companyid;
        public int fk_companyid
        {
            get { return _fk_companyid; }
            set { _fk_companyid = value; }
        }
        private string _departmentname;
        public string departmentname
        {
            get { return _departmentname; }
            set { _departmentname = value; }
        }
        private int _fk_cityid;
        public int fk_cityid
        {
            get { return _fk_cityid; }
            set { _fk_cityid = value; }
        }
        private int _fk_deptypecode = 5005003;
        public int fk_deptypecode
        {
            get { return _fk_deptypecode; }
            set { _fk_deptypecode = value; }
        }
        private int? _fk_parentid;
        public int? fk_parentid
        {
            get { return _fk_parentid; }
            set { _fk_parentid = value; }
        }
        private string _address;
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _fax;
        /// <summary>
        /// 传真
        /// </summary>
        public string fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        private string _telephone;
        /// <summary>
        /// 电话
        /// </summary>
        public string telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }
        private string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _linkman;
        public string linkman
        {
            get { return _linkman; }
            set { _linkman = value; }
        }
        private int _fxtcompanyid = 0;
        /// <summary>
        /// 0公共
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _dvalid = 1;
        public int dvalid
        {
            get { return _dvalid; }
            set { _dvalid = value; }
        }
        private int? _fk_depattr;
        /// <summary>
        /// 部门属性：直属部门，分支机构
        /// </summary>
        public int? fk_depattr
        {
            get { return _fk_depattr; }
            set { _fk_depattr = value; }
        }
        private int _suspended = 0;
        /// <summary>
        /// 暂停
        /// </summary>
        public int suspended
        {
            get { return _suspended; }
            set { _suspended = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

    }
}
