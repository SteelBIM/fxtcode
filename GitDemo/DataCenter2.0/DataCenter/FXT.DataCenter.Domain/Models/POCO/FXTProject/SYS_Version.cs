using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_Version
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _syscode;
        public int syscode
        {
            get { return _syscode; }
            set { _syscode = value; }
        }
        private string _versionid;
        public string versionid
        {
            get { return _versionid; }
            set { _versionid = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
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
        private string _htmltext;
        /// <summary>
        /// html格式的说明
        /// </summary>
        public string htmltext
        {
            get { return _htmltext; }
            set { _htmltext = value; }
        }

    }
}
