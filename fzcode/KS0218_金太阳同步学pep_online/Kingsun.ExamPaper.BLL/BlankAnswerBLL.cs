using Kingsun.ExamPaper.DAL;
using Kingsun.ExamPaper.Model;

namespace Kingsun.ExamPaper.BLL
{
    public class BlankAnswerBLL
    {
        public Tb_BlankAnswer GetBlankAnswer(string questionid)
        {
            return new BlankAnswerDAL().GetBlankAnswer(questionid);
        }

        public bool UpdateBlankAnswer(Tb_BlankAnswer ba)
        {
            return new BlankAnswerDAL().UpdateBlankAnswer(ba);
        }
    }
}
