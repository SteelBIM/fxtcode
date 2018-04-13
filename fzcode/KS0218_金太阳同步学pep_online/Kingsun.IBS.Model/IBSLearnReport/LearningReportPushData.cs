using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model.IBSLearnReport
{
    public class LearningReportPushData
    {
        public string BookID { get; set; }

        public string ClassID { get; set; }

        public DateTime CreateTime { get; set; }

        public int ModuleType { get; set; }

        public string VideoID { get; set; }

        public string VideoNumber { get; set; }

        public string UserID { get; set; }

        public float TotalScore { get; set; }

    }
}
