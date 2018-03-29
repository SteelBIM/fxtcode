using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_CustomForm
    {
        private int _formid;
        //[SQLField("formid", EnumDBFieldUsage.PrimaryKey, true)]
        public int formid
        {
            get { return _formid; }
            set { _formid = value; }
        }
        private string _formname;
        public string formname
        {
            get { return _formname; }
            set { _formname = value; }
        }
        private string _createuser;
        public string createuser
        {
            get { return _createuser; }
            set { _createuser = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private byte _valid = ((1));
        public byte valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}
