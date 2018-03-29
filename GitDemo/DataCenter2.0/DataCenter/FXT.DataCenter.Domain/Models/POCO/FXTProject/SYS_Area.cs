using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    [Serializable]
    public class SYS_Area
    {
        private int _areaid;
        //[SQLField("areaid", EnumDBFieldUsage.PrimaryKey)]
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private string _areaname;
        public string areaname
        {
            get { return _areaname; }
            set { _areaname = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
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
        private string _areaplacepicname;
        public string areaplacepicname
        {
            get { return _areaplacepicname; }
            set { _areaplacepicname = value; }
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
        private int _orderid = 1;
        /// <summary>
        /// 排序ID
        /// </summary>
        public int orderid
        {
            get { return _orderid; }
            set { _orderid = value; }
        }

    }
}
