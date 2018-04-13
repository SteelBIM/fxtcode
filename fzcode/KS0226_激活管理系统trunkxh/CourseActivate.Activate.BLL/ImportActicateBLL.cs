using CourseActivate.Core.Utility;
using CourseActivate.Framework.BLL;
using CourseActivate.Resource.Constract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.BLL
{
    public class ImportActicateBLL:Manage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EditionID"></param>
        /// <param name="Subject"></param>
        /// <param name="Grade"></param>
        /// <returns></returns>
        public string GetBookInfoById(string EditionID, string Subject, string Grade)
        {
            //MetadataService.Service service = new MetadataService.Service();
            //string bookinfo = service.GetBookData("", Grade, Subject, EditionID, "");
            int eid = Convert.ToInt32(EditionID);
            int sid = Convert.ToInt32(Subject);
            int gid = Convert.ToInt32(Grade);

            var books = SelectSearch<tb_res_book>(o=>o.EditionID==eid&&o.SubjectID==sid&&o.GradeID==gid&&o.Status==1).ToList();
            return JsonHelper.ToJson(books);
        }
    }
}
