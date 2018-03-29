using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Config_Page
    {
        private int _configid;
        //[SQLField("configid", EnumDBFieldUsage.PrimaryKey)]
        public int configid
        {
            get { return _configid; }
            set { _configid = value; }
        }
        private int _uiid;
        //[SQLField("uiid", EnumDBFieldUsage.PrimaryKey)]
        public int uiid
        {
            get { return _uiid; }
            set { _uiid = value; }
        }

    }
}
