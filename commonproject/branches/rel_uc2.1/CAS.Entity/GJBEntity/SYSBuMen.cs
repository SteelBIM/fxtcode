using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_BuMen : SYSBuMen
    {
        [SQLReadOnly]
        public List<SYS_BuMen> subbumenlist { get; set; }
        [SQLReadOnly]
        public int subbumencount { get; set; }
        [SQLReadOnly]
        public int provinceid { get; set; }
        [SQLReadOnly]
        public List<SYS_User> userlist { get; set; }
        [SQLReadOnly]
        public string cityname { get; set; }
        private SYS_SubCompanyCert _subcompanycert = new SYS_SubCompanyCert();
        [SQLReadOnly]
        public SYS_SubCompanyCert subcompanycert
        {
            get
            {
                return _subcompanycert;
            }
            set
            {
                _subcompanycert = value;
            }
        }
        [SQLReadOnly]
        public string chargemantruename { get; set; }
    }
}
