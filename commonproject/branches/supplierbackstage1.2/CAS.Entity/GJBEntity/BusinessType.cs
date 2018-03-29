using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Business_Type:SYSBusinessType
    {
        private string _subcompanyname;

        public string subcompanyname
        {
            get { return _subcompanyname; }
            set { _subcompanyname = value; }
        }

        private string _typecodename;

        public string typecodename
        {
            get { return _typecodename; }
            set { _typecodename = value; }
        }
        /// <summary>
        /// 业务分配人
        /// </summary>
        [SQLReadOnly]
        public string AssinguserIds { get; set; }
    }
}
