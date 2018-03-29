using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyEntityNew
{
    [Serializable]
    [TableAttribute("dbo.Dat_WXQuery")]
    public class DatWXQuery : BaseTO
    {
        private long _qid;
        /// <summary>
        /// 询价主键
        /// </summary>
        [SQLField("qid", EnumDBFieldUsage.PrimaryKey, true)]
        public long qid
        {
            get { return _qid; }
            set { _qid = value; }
        }
        private decimal? _buildarea;
        /// <summary>
        /// 面积(平方米)
        /// </summary>
        public decimal? buildarea
        {
            get { return _buildarea; }
            set { _buildarea = value; }
        }
        private int? _ownertypecode;
        /// <summary>
        /// 产权类型(20010502个人、20010501企业）
        /// </summary>
        public int? ownertypecode
        {
            get { return _ownertypecode; }
            set { _ownertypecode = value; }
        }
        private decimal? _registrationprice;
        /// <summary>
        /// 登记价(元)
        /// </summary>
        public decimal? registrationprice
        {
            get { return _registrationprice; }
            set { _registrationprice = value; }
        }
        private DateTime? _housecertdate;
        /// <summary>
        /// 房产证登记日期
        /// </summary>
        public DateTime? housecertdate
        {
            get { return _housecertdate; }
            set { _housecertdate = value; }
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
        private string _fullname;
        /// <summary>
        /// 委估对象全称
        /// </summary>
        public string fullname
        {
            get { return _fullname; }
            set { _fullname = value; }
        }

        /// <summary>
        /// 委估对象全称
        /// </summary>
        [SQLReadOnly]
        public string customername { get; set; }
        /// <summary>
        /// 委估对象全称
        /// </summary>
        [SQLReadOnly]
        public string email { get; set; }
        /// <summary>
        /// 委估对象全称
        /// </summary>
        [SQLReadOnly]
        public string telphone { get; set; }
        /// <summary>
        /// 委估对象全称
        /// </summary>
        [SQLReadOnly]
        public DateTime createdate { get; set; }
        /// <summary>
        /// 委估对象全称
        /// </summary>
        [SQLReadOnly]
        public long customerid { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        private string _imagepath;
        public string imagepath
        {
            get { return _imagepath; }
            set { _imagepath = value; }
        }
        /// <summary>
        /// 图片名字
        /// </summary>
        private string _imagename;
        public string imagename
        {
            get { return _imagename; }
            set { _imagename = value; }
        }
        /// <summary>
        /// 询价类型
        /// </summary>
        private int? _typecode;
        public int? typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        //CustomerName,b.Email,b.Telphone,b.CompanyName,b.CreateDate,b.CustomerId
    }

}
