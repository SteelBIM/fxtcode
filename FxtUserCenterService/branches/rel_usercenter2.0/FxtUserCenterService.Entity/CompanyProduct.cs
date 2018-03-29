using System;
using CAS.Entity.BaseDAModels;

namespace FxtUserCenterService.Entity
{
    [Serializable]
    [TableAttribute("dbo.CompanyProduct")]
    public class CompanyProduct : BaseTO
    {
        private int _cpid;
        [SQLField("cpid", EnumDBFieldUsage.PrimaryKey, true)]
        public int cpid
        {
            get { return _cpid; }
            set { _cpid = value; }
        }

        private int _companyid;
        /// <summary>
        /// 机构ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _producttypecode = 1003001;
        /// <summary>
        /// 产品CODE
        /// </summary>
        public int producttypecode
        {
            get { return _producttypecode; }
            set { _producttypecode = value; }
        }
        private string _currentversion;
        /// <summary>
        /// 当前版本
        /// </summary>
        public string currentversion
        {
            get { return _currentversion; }
            set { _currentversion = value; }
        }
        private DateTime? _startdate;
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? startdate
        {
            get { return _startdate; }
            set { _startdate = value; }
        }
        private DateTime? _overdate;
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private string _weburl;
        /// <summary>
        /// 站点
        /// </summary>
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        private string _apiurl;
        /// <summary>
        /// API
        /// </summary>
        public string apiurl
        {
            get { return _apiurl; }
            set { _apiurl = value; }
        }
        private string _outapiurl;
        public string outapiurl
        {
            get { return _outapiurl; }
            set { _outapiurl = value; }
        }
        private string _msgserver;
        /// <summary>
        /// 消息服务器
        /// </summary>
        public string msgserver
        {
            get { return _msgserver; }
            set { _msgserver = value; }
        }
        private DateTime? _createdate;
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 有效
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _appabbreviation;
        /// <summary>
        /// 应用简称
        /// </summary>
        public string appabbreviation
        {
            get { return _appabbreviation; }
            set { _appabbreviation = value; }
        }
    }
}