using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_CustomFieldLog
    {
        private long _fieldlogid;
        public long fieldlogid
        {
            get { return _fieldlogid; }
            set { _fieldlogid = value; }
        }
        private int _fieldid;
        public int fieldid
        {
            get { return _fieldid; }
            set { _fieldid = value; }
        }
        private long _formlogid;
        public long formlogid
        {
            get { return _formlogid; }
            set { _formlogid = value; }
        }
        private string _fieldvalue;
        public string fieldvalue
        {
            get { return _fieldvalue; }
            set { _fieldvalue = value; }
        }

    }
}
