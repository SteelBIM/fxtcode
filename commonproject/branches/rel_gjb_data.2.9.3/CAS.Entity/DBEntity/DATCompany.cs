using System;
using CAS.Entity.BaseDAModels;
//创建人:曾智磊,日期:2014-06-26

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("FxtDataCenter.dbo.DAT_Company")]
    public class DATCompany : BaseTO
    {
        private int _companyid;
        /// <summary>
        /// 楼盘相关公司、商家
        /// </summary>
        [SQLField("companyid", EnumDBFieldUsage.PrimaryKey, true)]
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _chinesename;
        /// <summary>
        /// 中文名称
        /// </summary>
        public string chinesename
        {
            get { return _chinesename; }
            set { _chinesename = value; }
        }
        private string _englishname;
        /// <summary>
        /// 英文名称
        /// </summary>
        public string englishname
        {
            get { return _englishname; }
            set { _englishname = value; }
        }
        private int? _companytypecode;
        /// <summary>
        /// 公司类型
        /// </summary>
        public int? companytypecode
        {
            get { return _companytypecode; }
            set { _companytypecode = value; }
        }
        private int? _cityid;
        /// <summary>
        /// 公司所在城市
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
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
        private string _telephone;
        /// <summary>
        /// 电话
        /// </summary>
        public string telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
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
        private string _website;
        /// <summary>
        /// 网址
        /// </summary>
        public string website
        {
            get { return _website; }
            set { _website = value; }
        }
        private DateTime? _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int? _valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _cothername;
        /// <summary>
        /// 简称、别名
        /// </summary>
        public string cothername
        {
            get { return _cothername; }
            set { _cothername = value; }
        }
        private string _brand;
        /// <summary>
        /// 主营品牌
        /// </summary>
        public string brand
        {
            get { return _brand; }
            set { _brand = value; }
        }
        private string _fromcountry;
        /// <summary>
        /// 公司原注册国家
        /// </summary>
        public string fromcountry
        {
            get { return _fromcountry; }
            set { _fromcountry = value; }
        }
        private string _fromcity;
        /// <summary>
        /// 原注册城市
        /// </summary>
        public string fromcity
        {
            get { return _fromcity; }
            set { _fromcity = value; }
        }
        private int? _naturecode;
        /// <summary>
        /// 性质，1178
        /// </summary>
        public int? naturecode
        {
            get { return _naturecode; }
            set { _naturecode = value; }
        }
        private int? _industrycode;
        /// <summary>
        /// 行业大类，1158
        /// </summary>
        public int? industrycode
        {
            get { return _industrycode; }
            set { _industrycode = value; }
        }
        private int? _subindustrycode;
        /// <summary>
        /// 行业小类，1159~1177
        /// </summary>
        public int? subindustrycode
        {
            get { return _subindustrycode; }
            set { _subindustrycode = value; }
        }
        private int? _scalecode;
        /// <summary>
        /// 公司规模（人）
        /// </summary>
        public int? scalecode
        {
            get { return _scalecode; }
            set { _scalecode = value; }
        }
        private int? _registcapital;
        /// <summary>
        /// 注册资本（万）
        /// </summary>
        public int? registcapital
        {
            get { return _registcapital; }
            set { _registcapital = value; }
        }
        private int? _standingcode;
        /// <summary>
        /// 行业地位，1179
        /// </summary>
        public int? standingcode
        {
            get { return _standingcode; }
            set { _standingcode = value; }
        }
        private int? _groupid;
        /// <summary>
        /// 集团公司ID
        /// </summary>
        public int? groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }
    }

}
