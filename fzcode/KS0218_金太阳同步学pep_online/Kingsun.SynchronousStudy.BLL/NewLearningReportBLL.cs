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
    public class NewLearningReportBLL
    {
        NewLearningReportDAL nlr = new NewLearningReportDAL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        /// <summary>
        /// 根据老师ID查询班级信息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetClassInfoByTeacherId(string appId, string classid, List<ClassInfoList> classList)
        {
            List<string> strlist = new List<string>(classid.Split(','));
            List<UserClass> ibs = new List<UserClass>();
            strlist.ForEach(a =>
            {
                List<StudentCount> count = new List<StudentCount>();
                var userclass = classBLL.GetUserClassRelationByNum(a, out count);
                userclass.ForEach(x =>
                {
                    if (ibs.FirstOrDefault(y => y.ClassID == x.ClassID && y.UserID == x.UserID) == null)
                    {
                        ibs.Add(x);
                    }

                });
            });
            Log4Net.LogHelper.Info(ibs.ToJson());
            return nlr.GetClassInfoByTeacherId(classList);
        }

        /// <summary>
        /// 根据班级Id查询年级学习人数
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetJuniorGradeNumByClassId(string appId, string classId)
        {
            return nlr.GetJuniorGradeNumByClassId(appId, classId);
        }

        /// <summary>
        /// 根据书籍id、班级Id获取单元模块下学习人数
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="classId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public HttpResponseMessage GetUnitLearningByBookId(string bookId, string classId, int pageNumber)
        {
            List<StudentCount> stuCount = new List<StudentCount>();
            var userClassList = classBLL.GetUserClassRelationByNum(classId, out stuCount);
            return nlr.GetUnitLearningByBookId(bookId, classId, pageNumber, userClassList);
        }

        /// <summary>
        /// 根据班级Id、册别、目录统计趣配音学习情况
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="classId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="firstTitleId"></param>
        /// <param name="secondTitleId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetVideoDetailsByModuleId(string bookId, string classId, string firstTitleId, string secondTitleId)
        {
            IBS_ClassUserRelation userClassList = classBLL.GetClassUserRelationByClassOtherId(classId, 1);
            return nlr.GetVideoDetailsByModuleId(bookId, classId, firstTitleId, secondTitleId, userClassList);
        }

        /// <summary>
        /// 根据书籍ID,班级ID获取班级详细学习情况
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="bookId">书籍ID</param>
        /// <param name="videoNumber">视频序号</param>
        /// <param name="pageNumber">分页代码</param>
        /// <returns></returns>
        public HttpResponseMessage GetClassStudyDetailsByClassId(string classId, string bookId, int videoNumber, int pageNumber)
        {
            IBS_ClassUserRelation userClassList = classBLL.GetClassUserRelationByClassOtherId(classId, 1);
            return nlr.GetClassStudyDetailsByClassId(classId, bookId, videoNumber, pageNumber, userClassList);
        }

    }
}
