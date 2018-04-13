using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.ExamPaper.Model
{
    public class Exampaper2DbModel
    {
        public string UserID { get; set; }
        public int CatalogID { get; set; }
        public float TotalScore { get; set; }
        public List<Custom_StuAnswer> AnswerList { get; set; }
    }
}
