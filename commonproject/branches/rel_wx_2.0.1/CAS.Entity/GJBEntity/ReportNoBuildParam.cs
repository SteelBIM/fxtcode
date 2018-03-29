using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.GJBEntity
{
    public class ReportNoBuildParam
    {
        private int _reporttype;
        /// <summary>
        /// 报告类型 10010
        /// </summary>
        public int reporttype
        {
            get { return _reporttype; }
            set { _reporttype = value; }
        }
        private int _entrusttype;
        /// <summary>
        /// 委托类型 对公 或者 个人
        /// </summary>
        public int entrusttype
        {
            get { return _entrusttype; }
            set { _entrusttype = value; }
        }
        private int _biztype;
        /// <summary>
        /// 业务类型  报告或者预评 2018006 2018005 
        /// </summary>
        public int biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private int _departmentid;
        /// <summary>
        /// 部门ID 或者分公司ID
        /// </summary>
        public int departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _subcompanyid;
        /// <summary>
        /// 分公司ID
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private string _companycode;
        /// <summary>
        /// 公司code
        /// </summary>
        public string companycode
        {
            get { return _companycode; }
            set { _companycode = value; }
        }
        private int _ismoreobject;
        /// <summary>
        /// 是否多套物业  0单套 1多套
        /// </summary>
        public int ismoreobject
        {
            get { return _ismoreobject; }
            set { _ismoreobject = value; }
        }
        private int _ownertype;
        /// <summary>
        /// 产权
        /// </summary>
        public int ownertype
        {
            get { return _ownertype; }
            set { _ownertype = value; }
        }
        private string _assespurposes;
        /// <summary>
        /// 评估目的
        /// </summary>
        public string assespurposes
        {
            get { return _assespurposes; }
            set { _assespurposes = value; }
        }
        private string _reportobjecttype;
        /// <summary>
        /// 报告委估对象类型
        /// </summary>
        public string reportobjecttype
        {
            get { return _reportobjecttype; }
            set { _reportobjecttype = value; }
        }
        private string _customername;
        /// <summary>
        /// 客户名称
        /// </summary>
        public string customername
        {
            get { return _customername; }
            set { _customername = value; }
        }
        private string _housingtype;
        /// <summary>
        /// 住宅类型 非住宅 非普通住宅 普通住宅
        /// </summary>
        public string housingtype
        {
            get { return _housingtype; }
            set { _housingtype = value; }
        }
        private DateTime _createdate;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _createuserid;
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private bool _reportedresults;
        /// <summary>
        /// 是否上报协会
        /// </summary>
        public bool reportedresults
        {
            get { return _reportedresults; }
            set { _reportedresults = value; }
        }

        private int _cityid;
        /// <summary>
        /// 项目城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }


    }
}
