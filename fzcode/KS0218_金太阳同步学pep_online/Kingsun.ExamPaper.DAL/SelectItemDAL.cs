using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.DAL
{
    public class SelectItemDAL : BaseManagement
    {
        public Tb_SelectItem GetSelectItem(string questionid, int sort)
        {
            return SelectByCondition<Tb_SelectItem>("QuestionID='" + questionid + "' and sort=" + sort.ToString());
        }

        public IList<Tb_SelectItem> GetSelectItemList(string questionid)
        {
            IList<Tb_SelectItem> list = Search<Tb_SelectItem>("QuestionID='" + questionid + "'", "Sort");
            if (list != null && list.Count > 0)
            {
                return list;
            }
            else
            {
                return new List<Tb_SelectItem>();
            }
        }

        public bool UpdateSelectItem(string questionid, int sort, int isAnswer, string imgUrl, string selectItem)
        {
            ExecuteSql(string.Format("update Tb_SelectItem set IsAnswer={2},SelectItem='{3}',ImgUrl='{4}' where QuestionID='{0}' and Sort={1}", questionid, sort, isAnswer, selectItem, imgUrl));
            return string.IsNullOrEmpty(_operatorError);
        }
    }
}
