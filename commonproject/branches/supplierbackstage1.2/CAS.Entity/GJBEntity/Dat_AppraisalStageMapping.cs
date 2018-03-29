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
        [SQLReadOnly]
        public int stagetype { get; set; }
        [SQLReadOnly]
        public int stage { get; set; }
    }
    public class Dat_AppraisalStageStatistics : BaseTO
    {
        public string truename { get; set; }
        public string projectfullname { get; set; }
        public string reportno { get; set; }
        public int righttypecode { get; set; }
        public string name { get; set; }
        public DateTime updatedon { get; set; }
        public string entrustid { get; set; }
        public int id { get; set; }
        public decimal score { get; set; }
        public int stagetype { get; set; }
    }
}
