using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.VW
{
    public class V_StatisticBook
    {
        public int bookrecordid { get; set; }
        public string BookName { get; set; }
        public int bookid { get; set; }
        public int num { get; set; }
        public int usenum { get; set; }
        public int Status { get; set; }
        public int ReelID { get; set; }
        public int GradeID { get; set; }
        public int SubjectID { get; set; }
        public int EditionID { get; set; }
        public int PeriodID { get; set; }
    }
}
