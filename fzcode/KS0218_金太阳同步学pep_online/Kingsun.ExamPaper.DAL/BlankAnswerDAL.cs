using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.DAL
{
    public class BlankAnswerDAL : BaseManagement
    {
        public Tb_BlankAnswer GetBlankAnswer(string questionid)
        {
            IList<Tb_BlankAnswer> list = Search<Tb_BlankAnswer>("QuestionID='" + questionid + "'", "AnswerType");
            if (list != null && list.Count > 0)
                return list[0];
            else
                return null;
        }
        public bool UpdateBlankAnswer(Tb_BlankAnswer ba)
        {
            ExecuteSql("update Tb_BlankAnswer set Answer='" + ba.Answer.Replace("'", "''") + "',AnswerType=" + ba.AnswerType.Value.ToString() + " where QuestionID='" + ba.QuestionID + "'");
            return string.IsNullOrEmpty(_operatorError);
        }
    }
}
