using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;

namespace CAS.Entity.GJBEntity
{
    public class Charge_Standrad:SYSChargeStandard
    {
        private string _subcompannyame;

        public string subcompanyname
        {
            get { return _subcompannyame; }
            set { _subcompannyame = value; }
        }
        private string _typecodename;

        public string typecodename
        {
            get { return _typecodename; }
            set { _typecodename = value; }
        }
        private string _rangevalue;

        public string rangevalue
        {
            get { return _rangevalue; }
            set { _rangevalue = value; }
        }
    }
}
