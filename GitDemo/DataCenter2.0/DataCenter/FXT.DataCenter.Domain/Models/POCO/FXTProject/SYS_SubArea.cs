using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_SubArea
    {
        private int _subareaid;
        //[SQLField("subareaid", EnumDBFieldUsage.PrimaryKey)]
        public int subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
        private string _subareaname;
        public string subareaname
        {
            get { return _subareaname; }
            set { _subareaname = value; }
        }
        private int _areaid;
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private int? _constructioncount;
        public int? constructioncount
        {
            get { return _constructioncount; }
            set { _constructioncount = value; }
        }
        private int? _gis_id;
        public int? gis_id
        {
            get { return _gis_id; }
            set { _gis_id = value; }
        }
        private string _regionplacepicname;
        public string regionplacepicname
        {
            get { return _regionplacepicname; }
            set { _regionplacepicname = value; }
        }
        private int? _oldid;
        public int? oldid
        {
            get { return _oldid; }
            set { _oldid = value; }
        }
        private decimal? _x;
        public decimal? x
        {
            get { return _x; }
            set { _x = value; }
        }
        private decimal? _y;
        public decimal? y
        {
            get { return _y; }
            set { _y = value; }
        }
        private int? _xyscale;
        /// <summary>
        /// 比例尺
        /// </summary>
        public int? xyscale
        {
            get { return _xyscale; }
            set { _xyscale = value; }
        }

    }
}
