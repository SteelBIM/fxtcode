using System.Collections.Generic;

namespace Kingsun.ExamPaper.Model
{
    public class Custom_MODBook
    {
        public int BookID { get; set; }
        public int EditionID { get; set; }
        public int GradeID { get; set; }
        public int BookReel { get; set; }
    }

    public class Custom_MODBookJson
    {
        public bool Success { get; set; }
        public List<Custom_MODBook> Data { get; set; }
        public string Message { get; set; }
    }
}
