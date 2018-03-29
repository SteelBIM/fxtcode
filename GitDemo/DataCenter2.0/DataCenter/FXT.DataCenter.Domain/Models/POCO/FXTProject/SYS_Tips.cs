using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_Tips
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _titel;
        public string titel
        {
            get { return _titel; }
            set { _titel = value; }
        }
        private string _simpletext;
        public string simpletext
        {
            get { return _simpletext; }
            set { _simpletext = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private string _url;
        public string url
        {
            get { return _url; }
            set { _url = value; }
        }
        private int? _tipstype;
        public int? tipstype
        {
            get { return _tipstype; }
            set { _tipstype = value; }
        }
        private DateTime _tipsdate = DateTime.Now;
        public DateTime tipsdate
        {
            get { return _tipsdate; }
            set { _tipsdate = value; }
        }
        private int? _fxtcompanyid;
        public int? fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }

    }
}
