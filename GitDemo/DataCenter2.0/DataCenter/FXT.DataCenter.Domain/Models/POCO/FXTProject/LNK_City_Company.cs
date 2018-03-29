using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class LNK_City_Company
    {
        private int _cityid;
        //[SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _companyid;
        //[SQLField("companyid", EnumDBFieldUsage.PrimaryKey)]
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

    }
}
