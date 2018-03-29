using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_AppraisalStage : DatAppraisalStage
    {
        [SQLReadOnly]
        public string query { get; set; }
        [SQLReadOnly]
        public string userid { get; set; }
        [SQLReadOnly]
        public string truename { get; set; }
        [SQLReadOnly]
        public long businessid { get; set; }
    }
}
