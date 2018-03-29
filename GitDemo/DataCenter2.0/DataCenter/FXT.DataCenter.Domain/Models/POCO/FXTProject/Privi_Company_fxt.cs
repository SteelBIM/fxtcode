using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    class Privi_Company_fxt
    {
        private int _fk_companyid;
        //[SQLField("fk_companyid", EnumDBFieldUsage.PrimaryKey)]
        public int fk_companyid
        {
            get { return _fk_companyid; }
            set { _fk_companyid = value; }
        }
        private DateTime _overdate = DateTime.Now;
        public DateTime overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private int _usernumber = -1;
        public int usernumber
        {
            get { return _usernumber; }
            set { _usernumber = value; }
        }
        private int _innerusernumber = 20;
        public int innerusernumber
        {
            get { return _innerusernumber; }
            set { _innerusernumber = value; }
        }
        private string _sysip1;
        public string sysip1
        {
            get { return _sysip1; }
            set { _sysip1 = value; }
        }
        private string _sysip2;
        public string sysip2
        {
            get { return _sysip2; }
            set { _sysip2 = value; }
        }
        private string _sysurl;
        public string sysurl
        {
            get { return _sysurl; }
            set { _sysurl = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _sysdefaultpage;
        public string sysdefaultpage
        {
            get { return _sysdefaultpage; }
            set { _sysdefaultpage = value; }
        }
        private string _sysloginpage;
        public string sysloginpage
        {
            get { return _sysloginpage; }
            set { _sysloginpage = value; }
        }
        private string _syscsspath;
        public string syscsspath
        {
            get { return _syscsspath; }
            set { _syscsspath = value; }
        }
        private string _sysjspath;
        public string sysjspath
        {
            get { return _sysjspath; }
            set { _sysjspath = value; }
        }
        private string _sysimagepath;
        public string sysimagepath
        {
            get { return _sysimagepath; }
            set { _sysimagepath = value; }
        }
        private string _sysphotopath;
        public string sysphotopath
        {
            get { return _sysphotopath; }
            set { _sysphotopath = value; }
        }
        private int _fk_cityid;
        //[SQLField("fk_cityid", EnumDBFieldUsage.PrimaryKey)]
        public int fk_cityid
        {
            get { return _fk_cityid; }
            set { _fk_cityid = value; }
        }
        private int _fk_systypecode;
        //[SQLField("fk_systypecode", EnumDBFieldUsage.PrimaryKey)]
        public int fk_systypecode
        {
            get { return _fk_systypecode; }
            set { _fk_systypecode = value; }
        }
        private string _fileno;
        /// <summary>
        /// 询价单号简称
        /// </summary>
        public string fileno
        {
            get { return _fileno; }
            set { _fileno = value; }
        }
        private string _skinpath;
        public string skinpath
        {
            get { return _skinpath; }
            set { _skinpath = value; }
        }
        private string _shortname;
        public string shortname
        {
            get { return _shortname; }
            set { _shortname = value; }
        }
        private string _logopath;
        public string logopath
        {
            get { return _logopath; }
            set { _logopath = value; }
        }
        private string _smalllogopath;
        public string smalllogopath
        {
            get { return _smalllogopath; }
            set { _smalllogopath = value; }
        }
        private string _ftpuser;
        public string ftpuser
        {
            get { return _ftpuser; }
            set { _ftpuser = value; }
        }
        private string _ftppass;
        public string ftppass
        {
            get { return _ftppass; }
            set { _ftppass = value; }
        }
        private string _ftpip;
        public string ftpip
        {
            get { return _ftpip; }
            set { _ftpip = value; }
        }
        private string _picpath;
        public string picpath
        {
            get { return _picpath; }
            set { _picpath = value; }
        }
        private string _websysname;
        public string websysname
        {
            get { return _websysname; }
            set { _websysname = value; }
        }
        private string _innersysname;
        public string innersysname
        {
            get { return _innersysname; }
            set { _innersysname = value; }
        }
        private string _soasysname;
        public string soasysname
        {
            get { return _soasysname; }
            set { _soasysname = value; }
        }
        private string _surveysysname;
        public string surveysysname
        {
            get { return _surveysysname; }
            set { _surveysysname = value; }
        }
        private int _isexporthose = 1;
        /// <summary>
        /// 是否可以导出物业数据
        /// </summary>
        public int isexporthose
        {
            get { return _isexporthose; }
            set { _isexporthose = value; }
        }
        private decimal _weight = 1M;
        public decimal weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        private string _version = "2.0";
        /// <summary>
        /// 产品版本号
        /// </summary>
        public string version
        {
            get { return _version; }
            set { _version = value; }
        }
        private DateTime _versiondate =Convert.ToDateTime("2012-2-15");
        /// <summary>
        /// 产品版本发布时间
        /// </summary>
        public DateTime versiondate
        {
            get { return _versiondate; }
            set { _versiondate = value; }
        }
        private string _showbanner = "11100111";
        /// <summary>
        /// 显示导航：首页，询价纪录，价格走势，价格监测，云房指数，市场案例，评估案例，统计中心
        /// </summary>
        public string showbanner
        {
            get { return _showbanner; }
            set { _showbanner = value; }
        }
        private string _telephone;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }
        private int _showsubhouse = 0;
        /// <summary>
        /// 是否显示附属房屋
        /// </summary>
        public int showsubhouse
        {
            get { return _showsubhouse; }
            set { _showsubhouse = value; }
        }
        private string _staticweburl;
        /// <summary>
        /// 静态文件网址
        /// </summary>
        public string staticweburl
        {
            get { return _staticweburl; }
            set { _staticweburl = value; }
        }
        private string _apiurl;
        /// <summary>
        /// API网址
        /// </summary>
        public string apiurl
        {
            get { return _apiurl; }
            set { _apiurl = value; }
        }
        private string _loginurl;
        /// <summary>
        /// 单点登录入口网址
        /// </summary>
        public string loginurl
        {
            get { return _loginurl; }
            set { _loginurl = value; }
        }
        private string _themename;
        /// <summary>
        /// 主题名称
        /// </summary>
        public string themename
        {
            get { return _themename; }
            set { _themename = value; }
        }
        private string _stylename;
        /// <summary>
        /// 样式名称
        /// </summary>
        public string stylename
        {
            get { return _stylename; }
            set { _stylename = value; }
        }
        private string _msgserverpath;
        /// <summary>
        /// 消息服务路径
        /// </summary>
        public string msgserverpath
        {
            get { return _msgserverpath; }
            set { _msgserverpath = value; }
        }
        private string _surveycenterurl;
        public string surveycenterurl
        {
            get { return _surveycenterurl; }
            set { _surveycenterurl = value; }
        }
        private int _isshowdiscountprice = 0;
        /// <summary>
        /// 是否显示折扣价
        /// </summary>
        public int isshowdiscountprice
        {
            get { return _isshowdiscountprice; }
            set { _isshowdiscountprice = value; }
        }
        private int _isreal = 0;
        /// <summary>
        /// 是否评估按揭系统(Real Estate Appraisal loan)(加中介、按揭角色)
        /// </summary>
        public int isreal
        {
            get { return _isreal; }
            set { _isreal = value; }
        }
        private string _shuiyintext;
        /// <summary>
        /// 相关图片的文字水印
        /// </summary>
        public string shuiyintext
        {
            get { return _shuiyintext; }
            set { _shuiyintext = value; }
        }
        private decimal _mapheight = 13M;
        public decimal mapheight
        {
            get { return _mapheight; }
            set { _mapheight = value; }
        }
        private decimal _mapwidth = 15M;
        public decimal mapwidth
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

    }
}
