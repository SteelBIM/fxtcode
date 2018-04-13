using Kingsun.ExamPaper.DAL;
using Kingsun.ExamPaper.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace Kingsun.ExamPaper.BLL
{
    public class SelectItemBLL
    {
        public Tb_SelectItem GetSelectItem(string questionid, int sort)
        {
            return new SelectItemDAL().GetSelectItem(questionid, sort);
        }
        public IList<Tb_SelectItem> GetSelectItemList(string questionid)
        {
            return new SelectItemDAL().GetSelectItemList(questionid);
        }
        public bool UpdateSelectItem(string questionid, int sort, int isAnswer, string imgUrl, string selectItem)
        {
            return new SelectItemDAL().UpdateSelectItem(questionid, sort, isAnswer, imgUrl, selectItem);
        }
    }
}
