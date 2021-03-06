﻿using System;

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
        private string _subcompanyname;
        /// <summary>
        /// 分公司名称
        /// </summary>
        public string subcompanyname
        {
            get { return _subcompanyname; }
            set { _subcompanyname = value; }
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
        private string _subcustomername;
        /// <summary>
        /// 客户二级机构名称
        /// </summary>
        public string subcustomername
        {
            get { return _subcustomername; }
            set { _subcustomername = value; }
        }
        private string _subcustomername2;
        /// <summary>
        /// 客户三级机构名称
        /// </summary>
        public string subcustomername2
        {
            get { return _subcustomername2; }
            set { _subcustomername2 = value; }
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
        private int _reportedresults;
        /// <summary>
        /// 是否上报协会
        /// </summary>
        public int reportedresults
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

        private int _reportnotype;
        /// <summary>
        /// 报告编号类型
        /// </summary>
        public int reportnotype
        {
            get { return _reportnotype; }
            set { _reportnotype = value; }
        }
        private string _writername;
        /// <summary>
        /// 预评、报告撰写人名称
        /// </summary>
        public string writername
        {
            get { return _writername; }
            set { _writername = value; }
        }
        private int _businesstypeid;
        /// <summary>
        /// 技术团队
        /// </summary>
        public int businesstypeid
        {
            get { return _businesstypeid; }
            set { _businesstypeid = value; }
        }

        private string _businesstype;
        /// <summary>
        /// 技术团队文本
        /// </summary>
        public string businesstype
        {
            get { return _businesstype; }
            set { _businesstype = value; }
        }
        private string _reportsubtype;
        /// <summary>
        /// 报告子类型
        /// </summary>
        public string reportsubtype
        {
            get { return _reportsubtype; }
            set { _reportsubtype = value; }
        }

        private DateTime? _reportcompletedate;
        /// <summary>
        /// 报告完成时间
        /// </summary>
        public DateTime? reportcompletedate
        {
            get { return _reportcompletedate; }
            set { _reportcompletedate = value; }
        }

        private decimal _totalprice;
        /// <summary>
        /// 总值
        /// </summary>
        public decimal totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }

        private int _objectcount;
        /// <summary>
        /// 委估对象数量
        /// </summary>
        public int objectcount
        {
            get { return _objectcount; }
            set { _objectcount = value; }
        }

        private string _areaname;
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaname
        {
            get { return _areaname; }
            set { _areaname = value; }
        }

        private long _entrustid;
        /// <summary>
        /// 业务编号
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
    }
}
