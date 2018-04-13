using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class CourseDAL : BaseManagement
    {

        public TB_CurriculumManage GetCourse(int id)
        {
            TB_CurriculumManage course = SelectByCondition<TB_CurriculumManage>("ID=" + id);
            return course;
        }

        public TB_CurriculumManage GetCourseByBookID(int bookID)
        {
            TB_CurriculumManage course = SelectByCondition<TB_CurriculumManage>("BookID=" + bookID);
            return course;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_CurriculumManage> GetCourseByCondition(string where)
        {
            IList<TB_CurriculumManage> list = Search<TB_CurriculumManage>(where);
            return list;
        }

        /// <summary>
        /// 新增课程信息
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public bool AddCourse(TB_CurriculumManage course)
        {
            return Insert<TB_CurriculumManage>(course);
        }

        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <returns></returns>
        public IList<TB_CurriculumManage> QueryCourse()
        {
            IList<TB_CurriculumManage> list = Search<TB_CurriculumManage>("State=1 ORDER BY CreateDate DESC");
            return list;
        }

        ///// <summary>
        ///// 获取版本标号和版本名称
        ///// </summary>
        ///// <returns></returns>
        //public ArrayList GetEdition()
        //{
        //    var ds = ExecuteSql("Select EditionID, TextbookVersion From TB_CurriculumManage  where State='True' Group BY EditionID,TextbookVersion");
        //    if (ds != null && ds.Tables.Count > 0)
        //    {
        //        var dt = ds.Tables[0];
        //        var tempList = FillData<TB_CurriculumManage>(dt);
        //        var edList = new ArrayList();
        //        foreach (var item in tempList)
        //        {
        //            //"select JuniorGrade,TeachingBooks,BookID From TB_CurriculumManage where EditionID=4 and State ='True' order by GradeID asc"
        //            edList.Add(new
        //            {
        //                EditionID = item.EditionID,
        //                EditionName = item.TextbookVersion,
        //            });
        //        }
        //        return edList;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 获得有配置资源的版本号和版本名称
        /// </summary>
        /// <returns></returns>
        public ArrayList GetEdition()
        {
            //var ds = ExecuteSql("Select EditionID, TextbookVersion,BookID From TB_CurriculumManage  where State='True' Group BY EditionID,TextbookVersion");
           // var ds = ExecuteSql("SELECT EditionID, TextbookVersion FROM TB_ModuleConfiguration left join TB_CurriculumManage on TB_ModuleConfiguration.BookID=TB_CurriculumManage.BookID where State='True' Group BY EditionID,TextbookVersion");
            var ds = ExecuteSql("SELECT EditionID, TextbookVersion FROM  TB_CurriculumManage left join TB_VersionChange on TB_VersionChange.BookID=TB_CurriculumManage.BookID where TB_CurriculumManage.State=1 and TB_VersionChange.State=1 Group BY EditionID,TextbookVersion");
            if (ds != null && ds.Tables.Count > 0)
            {
                var dt = ds.Tables[0];
                var tempList = FillData<TB_CurriculumManage>(dt);
                var edList = new ArrayList();

                foreach (var item in tempList)
                {
                    //"select JuniorGrade,TeachingBooks,BookID From TB_CurriculumManage where EditionID=4 and State ='True' order by GradeID asc"
                    edList.Add(new
                    {
                        EditionID = item.EditionID,
                        EditionName = item.TextbookVersion,
                    });
                }
                return edList;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 根据条件获取课程信息
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public IList<TB_CurriculumManage> GetCourseList(string where)
        {
            IList<TB_CurriculumManage> result = Search<TB_CurriculumManage>(where, "");
            return result;
        }

        /// <summary>
        /// 获取课程模块列表信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_ModuleSort> GetModuleSort(string where)
        {
            IList<TB_ModuleSort> result = Search<TB_ModuleSort>(where, "");
            return result;
        }

        /// <summary>
        /// 新增课程历史信息
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public bool AddBookHistory(TB_UserBookHistory course)
        {
            string where = "UserID=" + course.UserID + " and BookID=" + course.BookID;
            TB_UserBookHistory userhistory = SelectByCondition<TB_UserBookHistory>(where);
            if (userhistory != null)
            {
                return false;
            }
            else
            {
                return Insert<TB_UserBookHistory>(course);
            }
        }

        ///// <summary>
        ///// 获取当前用户所加载过的书籍信息
        ///// </summary>
        ///// <param name="where"></param>
        ///// <returns></returns>
        //public IList<View_UserHistory> GetListBookHistory(string where)
        //{
        //    IList<View_UserHistory> result = Search<View_UserHistory>(where, "");
        //    return result;
        //}

        public DataSet GetListUserHistory(string where)
        {
            DataSet ds = ExecuteSql(where); 
            return ds;
           
            
        }
    }
}
