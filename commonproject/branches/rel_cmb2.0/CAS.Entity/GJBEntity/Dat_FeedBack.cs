using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_FeedBack : DatFeedBack
    {
        /// <summary>
        /// 
        /// </summary>
        [SQLReadOnly]
        public string name { get; set; }

        [SQLReadOnly]
        public string subname { get; set; }

        [SQLReadOnly]
        public string department { get; set; }
    }
}
