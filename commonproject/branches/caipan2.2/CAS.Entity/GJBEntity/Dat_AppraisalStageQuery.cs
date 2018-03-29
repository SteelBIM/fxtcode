using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_AppraisalStageQuery : DatAppraisalStageQuery
    {
        [SQLReadOnly]
        public string userid { get; set; }
        [SQLReadOnly]
        public string truename { get; set; }
        [SQLReadOnly]
        public long businessid { get; set; }
    }
}
