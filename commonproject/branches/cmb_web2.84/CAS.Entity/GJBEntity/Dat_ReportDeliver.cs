using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ReportDeliver : DatReportDeliver
    {
        [SQLReadOnly]
        public long entrustid { get; set; }

        [SQLReadOnly]
        public long reportid { get; set; }

        [SQLReadOnly]
        public string reportno { get; set; }

        [SQLReadOnly]
        public int reporttype { get; set; }

        [SQLReadOnly]
        public string projectname { get; set; }

        [SQLReadOnly]
        public string delivermodename { get; set; }
    }
}
