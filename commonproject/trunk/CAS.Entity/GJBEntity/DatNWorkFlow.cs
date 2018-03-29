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
        
    }
}
