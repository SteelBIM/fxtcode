using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
     [Serializable]
    public class SYS_Code
    {

        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _code;
        //[SQLField("code", EnumDBFieldUsage.PrimaryKey)]
        public int code
        {
            get { return _code; }
            set { _code = value; }
        }
        private string _codename;
        public string codename
        {
            get { return _codename; }
            set { _codename = value; }
        }
        private string _codetype;
        public string codetype
        {
            get { return _codetype; }
            set { _codetype = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int? _subcode;
        public int? subcode
        {
            get { return _subcode; }
            set { _subcode = value; }
        }

    }
}
