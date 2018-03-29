using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Company_ygj
    {
        private int _fk_companyid;
        /// <summary>
        /// 机构ID
        /// </summary>
        //[SQLField("fk_companyid", EnumDBFieldUsage.PrimaryKey)]
        public int fk_companyid
        {
            get { return _fk_companyid; }
            set { _fk_companyid = value; }
        }
        private int _fk_cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        //[SQLField("fk_cityid", EnumDBFieldUsage.PrimaryKey)]
        public int fk_cityid
        {
            get { return _fk_cityid; }
            set { _fk_cityid = value; }
        }
        private int _fk_systypecode = 1003001;
        /// <summary>
        /// 系统类型
        /// </summary>
        //[SQLField("fk_systypecode", EnumDBFieldUsage.PrimaryKey)]
        public int fk_systypecode
        {
            get { return _fk_systypecode; }
            set { _fk_systypecode = value; }
        }
        private DateTime _overdate;
        /// <summary>
        /// 期限
        /// </summary>
        public DateTime overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private int _usernumber = 1000;
        /// <summary>
        /// 最大账号数
        /// </summary>
        public int usernumber
        {
            get { return _usernumber; }
            set { _usernumber = value; }
        }
        private string _sysurl;
        /// <summary>
        /// 系统对应网址
        /// </summary>
        public string sysurl
        {
            get { return _sysurl; }
            set { _sysurl = value; }
        }
        private string _sysip1;
        /// <summary>
        /// 系统对应网址1
        /// </summary>
        public string sysip1
        {
            get { return _sysip1; }
            set { _sysip1 = value; }
        }
        private string _sysip2;
        /// <summary>
        /// 系统对应网址1
        /// </summary>
        public string sysip2
        {
            get { return _sysip2; }
            set { _sysip2 = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 是否删除
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _fileno;
        /// <summary>
        /// 报告编号前缀
        /// </summary>
        public string fileno
        {
            get { return _fileno; }
            set { _fileno = value; }
        }
        private string _shortname;
        /// <summary>
        /// 机构简称
        /// </summary>
        public string shortname
        {
            get { return _shortname; }
            set { _shortname = value; }
        }
        private string _smalllogopath;
        /// <summary>
        /// 机构小LOGO路径
        /// </summary>
        public string smalllogopath
        {
            get { return _smalllogopath; }
            set { _smalllogopath = value; }
        }
        private string _logopath;
        /// <summary>
        /// 机构LOGO路径
        /// </summary>
        public string logopath
        {
            get { return _logopath; }
            set { _logopath = value; }
        }
        private string _showbanner = "11100111";
        /// <summary>
        /// 左边导航栏显示项
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
        private DateTime _versiondate = Convert.ToDateTime("2012-02-15");
        /// <summary>
        /// 产品版本发布时间
        /// </summary>
        public DateTime versiondate
        {
            get { return _versiondate; }
            set { _versiondate = value; }
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
        private int _ismaster = 0;
        /// <summary>
        /// 是否运营方
        /// </summary>
        public int ismaster
        {
            get { return _ismaster; }
            set { _ismaster = value; }
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
        private string _outurl;
        /// <summary>
        /// 评估机构外部接口地址
        /// </summary>
        public string outurl
        {
            get { return _outurl; }
            set { _outurl = value; }
        }
        private string _outurlname;
        /// <summary>
        /// 评估机构外部接口名称
        /// </summary>
        public string outurlname
        {
            get { return _outurlname; }
            set { _outurlname = value; }
        }

    }
}
