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
        private int? _cityid;
        /// <summary>
        /// 产品所在城市
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _weburl1;
        /// <summary>
        /// 产品网址1
        /// </summary>
        public string weburl1
        {
            get { return _weburl1; }
            set { _weburl1 = value; }
        }
        private string _logopath;
        /// <summary>
        /// 产品LOGO
        /// </summary>
        public string logopath
        {
            get { return _logopath; }
            set { _logopath = value; }
        }
        private string _smalllogopath;
        /// <summary>
        /// 产品小LOGO
        /// </summary>
        public string smalllogopath
        {
            get { return _smalllogopath; }
            set { _smalllogopath = value; }
        }
        private string _titlename;
        /// <summary>
        /// 对外显示的产品名称
        /// </summary>
        public string titlename
        {
            get { return _titlename; }
            set { _titlename = value; }
        }
        private int? _isexporthose;
        /// <summary>
        /// 是否可以导出数据中心数据
        /// </summary>
        public int? isexporthose
        {
            get { return _isexporthose; }
            set { _isexporthose = value; }
        }
        private string _telephone;
        /// <summary>
        /// 产品联系电话
        /// </summary>
        public string telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }
        private int? _showsubhouse;
        /// <summary>
        /// 是否显示附属房屋
        /// </summary>
        public int? showsubhouse
        {
            get { return _showsubhouse; }
            set { _showsubhouse = value; }
        }
        private int? _isshowdiscountprice;
        /// <summary>
        /// 是否显示折扣价
        /// </summary>
        public int? isshowdiscountprice
        {
            get { return _isshowdiscountprice; }
            set { _isshowdiscountprice = value; }
        }
        private decimal? _mapheight;
        public decimal? mapheight
        {
            get { return _mapheight; }
            set { _mapheight = value; }
        }
        private decimal? _mapwidth;
        public decimal? mapwidth
        {
            get { return _mapwidth; }
            set { _mapwidth = value; }
        }
        private int _automakename = 1;
        /// <summary>
        /// 是否自动生成物业全称；1自动（楼盘名称+楼栋名称+（栋）+楼层+层+房号名称），0原始（地址+楼盘名称+楼栋名称+房号名称）
        /// </summary>
        public int automakename
        {
            get { return _automakename; }
            set { _automakename = value; }
        }
        private int _isdeletetrue = 0;
        /// <summary>
        /// 是否直接删除数据
        /// </summary>
        public int isdeletetrue
        {
            get { return _isdeletetrue; }
            set { _isdeletetrue = value; }
        }
        private string _skinpath;
        public string skinpath
        {
            get { return _skinpath; }
            set { _skinpath = value; }
        }
        private string _bg_pic;
        public string bg_pic
        {
            get { return _bg_pic; }
            set { _bg_pic = value; }
        }
        private string _homepage;
        /// <summary>
        /// 评估机构主页
        /// </summary>
        public string homepage
        {
            get { return _homepage; }
            set { _homepage = value; }
        }
        private string _twodimensionalcode;
        /// <summary>
        /// 二维码图片
        /// </summary>
        public string twodimensionalcode
        {
            get { return _twodimensionalcode; }
            set { _twodimensionalcode = value; }
        }

    }
}