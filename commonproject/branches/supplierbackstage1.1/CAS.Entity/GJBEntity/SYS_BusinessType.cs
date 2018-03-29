using System.Collections.Generic;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_BusinessType : BaseTO
    {
        private int _typecode;

        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private string _typecodename;

        public string typecodename
        {
            get { return _typecodename; }
            set { _typecodename = value; }
        }
        
        private List<Business_Type> _businesstypelist;

        public List<Business_Type> businesstypelist
        {
            get { return _businesstypelist; }
            set { _businesstypelist = value; }
        }
    }
}
