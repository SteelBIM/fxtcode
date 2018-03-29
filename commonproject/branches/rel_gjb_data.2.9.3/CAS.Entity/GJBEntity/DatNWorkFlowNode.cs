using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_NWorkFlowNode : DatNWorkFlowNode
    {
        [SQLReadOnly]
        public string sptruenamelist { get; set; }
    }
}
