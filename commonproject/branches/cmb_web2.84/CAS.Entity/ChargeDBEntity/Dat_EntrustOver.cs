using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    public class Dat_EntrustOver : DatEntrustOver
    {
        /// <summary>
        ///业务员姓名
        /// </summary>
        public string salesmanname
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string createrName
        {
            get;
            set;
        }
    }
}
