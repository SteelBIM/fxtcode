using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_CustomField
    {
        private int _fieldid;
        //[SQLField("fieldid", EnumDBFieldUsage.PrimaryKey, true)]
        public int fieldid
        {
            get { return _fieldid; }
            set { _fieldid = value; }
        }
        private string _fieldname;
        public string fieldname
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }
        private string _fieldtype;
        public string fieldtype
        {
            get { return _fieldtype; }
            set { _fieldtype = value; }
        }
        private int _fieldsize = 0;
        public int fieldsize
        {
            get { return _fieldsize; }
            set { _fieldsize = value; }
        }
        private int _formid;
        public int formid
        {
            get { return _formid; }
            set { _formid = value; }
        }

    }
}
