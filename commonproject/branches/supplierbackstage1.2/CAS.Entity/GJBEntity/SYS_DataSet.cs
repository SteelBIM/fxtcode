using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_DataSet : SYSDataSet
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        [SQLReadOnly]
        public string truename { get; set; }
    }
}
