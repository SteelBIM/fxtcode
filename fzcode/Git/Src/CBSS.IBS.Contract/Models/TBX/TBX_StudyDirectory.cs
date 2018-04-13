using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract
{
    public class TBX_StudyDirectory
    {
        public int ID { get; set; }

        public int UserID { get; set; }
        public DateTime CreateTime { get; set; }

        public int BookID { get; set; }

        public int VideoNumber { get; set; }

        public int FirstModularID { get; set; }

        public string FirstModular { get; set; }

        public int FirstTitleID { get; set; }

        public string FirstTitle { get; set; }

        public int SecondTitleID { get; set; }

        public string SecondTitle { get; set; }

        public int StudentStudyCount { get; set; }

        public int ClassStudentCount { get; set; }

        public int ClassNum { get; set; }
    }
}
