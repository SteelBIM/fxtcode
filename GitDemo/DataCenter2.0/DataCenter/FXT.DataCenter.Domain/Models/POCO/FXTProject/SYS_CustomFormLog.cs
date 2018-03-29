using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_CustomFormLog
    {
        private long _formlogid;
        //[SQLField("formlogid", EnumDBFieldUsage.PrimaryKey, true)]
        public long formlogid
        {
            get { return _formlogid; }
            set { _formlogid = value; }
        }
        private int _formid;
        public int formid
        {
            get { return _formid; }
            set { _formid = value; }
        }
        private string _saveuser;
        public string saveuser
        {
            get { return _saveuser; }
            set { _saveuser = value; }
        }
        private DateTime _savedate = DateTime.Now;
        public DateTime savedate
        {
            get { return _savedate; }
            set { _savedate = value; }
        }

    }
}
