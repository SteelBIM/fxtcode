using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Resource.Constract.Model
{
    public class tb_res_book
    {
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string BookCover { get; set; }
        public int PeriodID { get; set; }
        public string PeriodName { get; set; }
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        public int ReelID { get; set; }
        public string ReelName { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int EditionID { get; set; }
        public string EditionName { get; set; }
        public int Publishid { get; set; }
        public string PublishidName { get; set; }
        public string EPlate { get; set; }
        public int Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
