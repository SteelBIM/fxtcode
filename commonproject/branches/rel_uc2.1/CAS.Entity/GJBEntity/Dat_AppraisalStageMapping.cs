using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_AppraisalStageMapping : DatAppraisalStageMapping
    {
        [SQLReadOnly]
        public string name { get; set; }
    }
    public class Dat_AppraisalStageStatistics : BaseTO
    {
        public string truename { get; set; }
        public int id { get; set; }
        public decimal score { get; set; }
    }
}
