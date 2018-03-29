using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test20160519
{ /// <summary>
    /// 城市选择控件
    /// </summary>
    public partial class UC_Case
    {
        private const string _tempProjectsSheetName = "temp_Projects";
        private const string _tempAreaSheetName = "temp_Areas";
        /// <summary>
        /// 数据库楼盘总数
        /// </summary>
        private int _projectCount = 0;
    }
    public class Project
    {
        public int projectid { get; set; }
        public string projectname { get; set; }
    }
    public class Area
    {
        public int areaid { get; set; }
        public string areaname { get; set; }
    }
    public class Province
    {
        public int provinceid { get; set; }
        public string provincename { get; set; }
    }

    public class City
    {
        public int cityid { get; set; }
        public string cityname { get; set; }
        public int provinceid { get; set; }
    }
}