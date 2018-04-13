using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model.IBSLearnReport
{
    public class IBSLearnReport
    {
        public string ClassID { get; set; }

        public int SubjectID { get; set; }

        public int BookID { get; set; }

        public int ModuleID { get; set; }

        public int ReadNum { get; set; }

        public int UnReadNum { get; set; }

        public int Flag { get; set; }

        public List<BestRecordsList> BestRecordsList;

        public IBSLearnReport()
        {
            BestRecordsList=new List<BestRecordsList>();
        }
    }

    public class BestRecordsList
    {
        public int UserId { get; set; }

        public int Num { get; set; }

        public int BestRecordID { get; set; }
        public float BestScore { get; set; }

        public int BestRank { get; set; }

        public DateTime BestDate { get; set; }
    }
}
