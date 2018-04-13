using Kingsun.ExamPaper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.ExamPaper.Model;

namespace Kingsun.ExamPaper.BLL
{
    public class PracticeEveryDayBLL
    {
        private PracticeEveryDay.PracticeEveryDay pedService = new PracticeEveryDay.PracticeEveryDay();
        /// <summary>
        /// 根据BookID获取MOD相关信息
        /// </summary>
        /// <param name="bookid"></param>
        /// <returns></returns>
        public Custom_MODBook GetBookInfoFromMOD(int bookid)
        {
            string result = "";
            try
            {
                result = pedService.GetBookInfo(bookid);//{"Success":true,"Data":[{"BookID":0,"EditionID":21,"GradeID":4,"BookReel":2}],"Message":""}
                Custom_MODBookJson listModBook = JsonHelper.DecodeJson<Custom_MODBookJson>(result);
                if (listModBook.Success && listModBook.Data.Count > 0)
                {
                    return listModBook.Data[0];
                }
            }
            catch
            {
            }
            return null;
        }
    }
}
