using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model
{
    public class StudyCurriculum
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public string CourseCategory { get; set; }

        public int SubjectID { get; set; }

        public string TextbookVersion { get; set; }

        public int EditionID { get; set; }

        public string JuniorGrade { get; set; }

        public int GradeID { get; set; }

        public string TeachingBooks { get; set; }

        public int BreelID { get; set; }

        public int BookID { get; set; }

        public DateTime CreateTime { get; set; }

        public int StudentStudyCount { get; set; }

        public int ClassNum { get; set; }
    }
}
