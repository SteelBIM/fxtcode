using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Licence
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _cityname;
        public string cityname
        {
            get { return _cityname; }
            set { _cityname = value; }
        }
        private int _syscode;
        public int syscode
        {
            get { return _syscode; }
            set { _syscode = value; }
        }
        private int _fxtcompanyid;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _fxtcompanyname;
        public string fxtcompanyname
        {
            get { return _fxtcompanyname; }
            set { _fxtcompanyname = value; }
        }
        private string _overdate;
        public string overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private string _licence;
        public string licence
        {
            get { return _licence; }
            set { _licence = value; }
        }
        private DateTime _createtime = DateTime.Now;
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }

    }
}
