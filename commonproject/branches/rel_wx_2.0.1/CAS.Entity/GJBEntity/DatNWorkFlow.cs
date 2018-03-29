using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_NWorkFlow : DatNWorkFlow
    {
        [SQLReadOnly]
        public int processingdocument { get; set; }
        [SQLReadOnly]
        public int lowversion { get; set; }
        [SQLReadOnly]
        public int highversion { get; set; }
        [SQLReadOnly]
        public string truename { get; set; }
        /// <summary>
        /// 节点数
        /// </summary>
        [SQLReadOnly]        
        public int nodes { get; set; }
        /// <summary>
        /// 流程是否包含盖章步骤
        /// </summary>
        [SQLReadOnly] 
        public bool containsstampnode { get; set; }
    }
}
