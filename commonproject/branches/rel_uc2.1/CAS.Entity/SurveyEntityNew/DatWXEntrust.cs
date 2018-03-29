using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyEntityNew
{
    [Serializable]
    [TableAttribute("dbo.Dat_WXEntrust")]
    public class DatWXEntrust : BaseTO
    {
        private long _eid;
        /// <summary>
        /// 主键
        /// </summary>
        [SQLField("eid", EnumDBFieldUsage.PrimaryKey, true)]
        public long eid
        {
            get { return _eid; }
            set { _eid = value; }
        }
        private int? _reporttypecode;
        /// <summary>
        /// 报告类型
        /// </summary>
        public int? reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        private int? _biztype;
        /// <summary>
        /// 委托类型
        /// </summary>
        public int? biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
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
        private int? _typeid;
        /// <summary>
        /// 报告(1出预评、0出报告)
        /// </summary>
        public int? typeid
        {
            get { return _typeid; }
            set { _typeid = value; }
        }
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int? _provinceid;
        /// <summary>
        /// 省份ID
        /// </summary>
        public int? provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        private string _provincename;
        /// <summary>
        /// 省份名字
        /// </summary>
        public string provincename
        {
            get { return _provincename; }
            set { _provincename = value; }
        }
        private int? _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _cityname;
        /// <summary>
        /// 城市名字
        /// </summary>
        public string cityname
        {
            get { return _cityname; }
            set { _cityname = value; }
        }
        private int? _areaid;
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int? areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private string _areaname;
        /// <summary>
        /// 行政区名字
        /// </summary>
        public string areaname
        {
            get { return _areaname; }
            set { _areaname = value; }
        }
        private string _wxopenid;
        /// <summary>
        /// 微信id
        /// </summary>
        public string wxopenid
        {
            get { return _wxopenid; }
            set { _wxopenid = value; }
        }
        private DateTime? _createtime;
        /// <summary>
        /// 询价时间
        /// </summary>
        public DateTime? createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _companywxopenid;
        /// <summary>
        /// 公司微信ID
        /// </summary>
        public string companywxopenid
        {
            get { return _companywxopenid; }
            set { _companywxopenid = value; }
        }
        private string _companyname;
        /// <summary>
        /// 公司名字
        /// </summary>
        public string companyname
        {
            get { return _companyname; }
            set { _companyname = value; }
        }


    }

}
