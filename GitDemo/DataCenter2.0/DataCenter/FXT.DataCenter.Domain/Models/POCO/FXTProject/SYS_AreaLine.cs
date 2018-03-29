using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_AreaLine
    {

        private int _arealineid;
        //[SQLField("arealineid", EnumDBFieldUsage.PrimaryKey)]
        public int arealineid
        {
            get { return _arealineid; }
            set { _arealineid = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _arealinename;
        public string arealinename
        {
            get { return _arealinename; }
            set { _arealinename = value; }
        }
        private int? _areaid;
        public int? areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }

    }
}
