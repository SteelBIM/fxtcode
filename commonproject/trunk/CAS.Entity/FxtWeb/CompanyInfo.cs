using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtWeb
{
    [Serializable]
    [TableAttribute("dbo.CompanyInfo")]
    public class CompanyInfo : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _companyname;
        /// <summary>
        /// 云估价成员公司名
        /// </summary>
        public string companyname
        {
            get { return _companyname; }
            set { _companyname = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 成员ID
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _companyintroduct;
        /// <summary>
        /// 公司介绍
        /// </summary>
        public string companyintroduct
        {
            get { return _companyintroduct; }
            set { _companyintroduct = value; }
        }
        private string _customerphoto;
        /// <summary>
        /// 证言客户照片
        /// </summary>
        public string customerphoto
        {
            get { return _customerphoto; }
            set { _customerphoto = value; }
        }
        private string _customermessages;
        /// <summary>
        /// 客户证言
        /// </summary>
        public string customermessages
        {
            get { return _customermessages; }
            set { _customermessages = value; }
        }
        private string _customername;
        /// <summary>
        /// 证言客户姓名
        /// </summary>
        public string customername
        {
            get { return _customername; }
            set { _customername = value; }
        }
        private string _businessscope;
        /// <summary>
        /// 业务范围
        /// </summary>
        public string businessscope
        {
            get { return _businessscope; }
            set { _businessscope = value; }
        }
        private string _servicecase;
        /// <summary>
        /// 服务案例
        /// </summary>
        public string servicecase
        {
            get { return _servicecase; }
            set { _servicecase = value; }
        }
        private bool _isshow = false;
        /// <summary>
        /// 是否在官网显示
        /// </summary>
        public bool isshow
        {
            get { return _isshow; }
            set { _isshow = value; }
        }
        private bool _hasdetails = false;
        /// <summary>
        /// 是否生成详情页
        /// </summary>
        public bool hasdetails
        {
            get { return _hasdetails; }
            set { _hasdetails = value; }
        }
        private DateTime? _updatedate;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? updatedate
        {
            get { return _updatedate; }
            set { _updatedate = value; }
        }
        private DateTime? _showdate;
        /// <summary>
        /// 展示时间
        /// </summary>
        public DateTime? showdate
        {
            get { return _showdate; }
            set { _showdate = value; }
        }
        /// <summary>
        /// 是否为云估价成员
        /// </summary>
        [SQLReadOnly]
        public bool issigned { get; set; }
        /// <summary>
        /// 账号后缀
        /// </summary>
        [SQLReadOnly]
        public string companycode{get;set;}
        /// <summary>
        /// 公司简称
        /// </summary>
        [SQLReadOnly]
        public string shortname{get;set;}
        /// <summary>
        /// 省份
        /// </summary>
        [SQLReadOnly]
        public string provincename{get;set;}
        /// <summary>
        /// 城市名称
        /// </summary>
        [SQLReadOnly]
        public string cityname { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        [SQLReadOnly]
        public string cityid{get;set;}
        /// <summary>
        /// 官网
        /// </summary>
        [SQLReadOnly]
        public string weburl{get;set;}
        /// <summary>
        /// 联系方式
        /// </summary>
        [SQLReadOnly]
        public string telephone{get;set;}
        /// <summary>
        /// 传真
        /// </summary>
        [SQLReadOnly]
        public string fax{get;set;}
        /// <summary>
        /// 地址
        /// </summary>
        [SQLReadOnly]
        public string address{get;set;}
        /// <summary>
        /// 加入时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? joindate{get;set;}
    }

}
