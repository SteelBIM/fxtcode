using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.BLL
{
    public class ExamPaperBLL
    {
        ExamPaperDAL elr = new ExamPaperDAL();


        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        /// <summary>
        ///  获取班级期末测评详细成绩信息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetUserExamInfo(string classId, string catalogId, int pageNumber)
        {
            List<StudentCount> stuCount = new List<StudentCount>();
            IBS_ClassUserRelation userClassList = classBLL.GetClassUserRelationByClassOtherId(classId, 1);
            return elr.GetUserExamInfo(classId, catalogId, pageNumber, userClassList);
        }
        /// <summary>
        /// 获取期末模拟测试卷
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetExamPaperList(string bookId, string classShortId)
        {
            IBS_ClassUserRelation classinfo = classBLL.GetClassUserRelationByClassOtherId(classShortId, 1);
            if (classinfo != null)
            {
                if (classinfo.ClassStuList.Count > 0)
                {
                    int ClassNum = classinfo.ClassStuList.Count;
                    StringBuilder sbclass = new StringBuilder();
                    sbclass.Append("0");
                    classinfo.ClassStuList.ForEach(a =>
                    {

                        sbclass.Append("," + a.StuID);
                    });

                    var exampaperList = elr.GetExamPaperList(bookId, classinfo, ClassNum, classShortId);
                    if (exampaperList != null)
                    {
                       return JsonHelper.GetResult(exampaperList, "操作成功");
                    }
                    else
                    {
                        return  JsonHelper.GetErrorResult("找不到任何学生！");
                    }
                }
                else
                {
                    return JsonHelper.GetErrorResult("该班级没有学生！");
                }
            }
            else
            {
                return JsonHelper.GetErrorResult("该班级不存在！");
            }
        }
           
    }
}
