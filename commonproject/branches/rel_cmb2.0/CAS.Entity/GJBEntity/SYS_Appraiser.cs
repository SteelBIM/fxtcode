using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;

namespace CAS.Entity.GJBEntity
{
    public class SYS_Appraiser:SYSAppraiser
    {
        private string _companyname;

        public string companyname
        {
            get { return _companyname; }
            set { _companyname = value; }
        }

        private string _typecodename;

        public string typecodename
        {
            get { return _typecodename; }
            set { _typecodename = value; }
        }

    }
}
