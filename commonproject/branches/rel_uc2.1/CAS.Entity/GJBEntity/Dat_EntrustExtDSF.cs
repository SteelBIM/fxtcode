using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_EntrustExtDSF : DatEntrustExtDSF
    {
        /// <summary>
        /// 文件guid
        /// </summary>
        [SQLReadOnly]
        public string guid { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [SQLReadOnly]
        public string name { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        [SQLReadOnly]
        public string path { get; set; }
    }
}
