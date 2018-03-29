using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Dat_Client
    {
        private int _id;
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
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _clientname;
        /// <summary>
        /// 委托方名称
        /// </summary>
        public string clientname
        {
            get { return _clientname; }
            set { _clientname = value; }
        }
        private string _companyname;
        /// <summary>
        /// 委托方单位
        /// </summary>
        public string companyname
        {
            get { return _companyname; }
            set { _companyname = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 客户单位ID,关联Privi_Company
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _department;
        /// <summary>
        /// 委托方部门
        /// </summary>
        public string department
        {
            get { return _department; }
            set { _department = value; }
        }
        private int? _departmentid;
        /// <summary>
        /// 客户部门ID，关联Privi_Department
        /// </summary>
        public int? departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        private int _clienttype = 2012001;
        /// <summary>
        /// 客户类型
        /// </summary>
        public int clienttype
        {
            get { return _clienttype; }
            set { _clienttype = value; }
        }
        private string _idnumber;
        /// <summary>
        /// 证件号码
        /// </summary>
        public string idnumber
        {
            get { return _idnumber; }
            set { _idnumber = value; }
        }
        private string _mobilephone;
        /// <summary>
        /// 手机
        /// </summary>
        public string mobilephone
        {
            get { return _mobilephone; }
            set { _mobilephone = value; }
        }
        private string _officephone;
        /// <summary>
        /// 电话
        /// </summary>
        public string officephone
        {
            get { return _officephone; }
            set { _officephone = value; }
        }
        private string _address;
        /// <summary>
        /// 地址
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _email;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email
        {
            get { return _email; }
            set { _email = value; }
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
        private string _linkman;
        /// <summary>
        /// 联系人
        /// </summary>
        public string linkman
        {
            get { return _linkman; }
            set { _linkman = value; }
        }
        private string _creater;
        public string creater
        {
            get { return _creater; }
            set { _creater = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

    }
}
