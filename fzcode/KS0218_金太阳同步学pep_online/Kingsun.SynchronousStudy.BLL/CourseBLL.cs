using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;
using log4net;
using System.Reflection;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.BLL
{
    public class CourseBLL
    {
        CourseDAL cousedal = new CourseDAL();
        ModuleConfigurationBLL moduleconfig = new ModuleConfigurationBLL();
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<TB_CurriculumManage> GetCourseListMethod(string appID, out string errMsg)
        {
            errMsg = "";
            int VersionID = GetVersionID(appID);
            if (VersionID == 0)
            {
                errMsg = "不存在版本信息";
            }

            string where = "StageID=2 and SubjectID=3 and EditionID=" + VersionID + " and State =1 order by GradeID,BreelID";

            string sql = string.Format(@"SELECT * FROM TB_CurriculumManage WHERE {0}", where);
            DataSet ds = JsonHelper.SelectOrderSql(appID, sql);

            List<TB_CurriculumManage> courselist = JsonHelper.DataSetToIList<TB_CurriculumManage>(ds, 0); //GetCourseList(where) as List<TB_CurriculumManage>;
            if (courselist == null)
            {
                errMsg = "不存在版本信息";
            }
            string wheres = " State=0 ";

            string strsql = string.Format(@"SELECT * FROM TB_ModuleConfiguration WHERE  State=0");
            DataSet dsMC = JsonHelper.SelectOrderSql(appID, strsql);

            List<TB_ModuleConfiguration> newversionlist = JsonHelper.DataSetToIList<TB_ModuleConfiguration>(dsMC, 0);
            //moduleconfig.GetModuleList(wheres) as List<TB_ModuleConfiguration>;// versionbll.GetModuleByWhere(wheres) as List<TB_VersionChange>;
            //去重
            List<TB_ModuleConfiguration> versionlist = newversionlist.Where((x, i) => newversionlist.FindIndex(z => z.BookID == x.BookID) == i).ToList();
            var courselistes = from c in courselist
                               join v in versionlist on c.BookID equals v.BookID
                               select c;

            return courselistes.ToList();
        }

        /// <summary>
        /// 根据AppID获取版本IDVersionId
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public int GetVersionID(string AppID)
        {
            int versionId = 0;
            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("/XmlFiles/APPManagement.xml");
                TB_APPManagement appInfo = getAPPManagement(AppID, path);
                if (appInfo != null)
                {
                    versionId = Convert.ToInt32(appInfo.VersionID);
                }

            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return versionId;
        }

        /// <summary>
        /// 根据ID和xml路径得到TB_APPManagement
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="path">xml路径</param>
        /// <returns></returns>
        public TB_APPManagement getAPPManagement(string ID, string path)
        {
            TB_APPManagement model = null;
            try
            {
                bool flag = false;
                //读取
                XmlHelper xmlHelper = new XmlHelper(path);
                DataTable dt_xml = xmlHelper.GetDataSetByXml(path) == null ? null : xmlHelper.GetDataSetByXml(path).Tables[0];
                if (dt_xml != null && dt_xml.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_xml.Rows)
                    {
                        if (row["ID"].Equals(ID))
                        {
                            model = new TB_APPManagement()
                            {
                                ID = row["ID"].ToString(),
                                VersionName = row["VersionName"].ToString(),
                                VersionID = Convert.ToInt32(string.IsNullOrEmpty(row["VersionID"].ToString()) ? 0 : row["VersionID"]),
                                CreatePerson = row["CreatePerson"].ToString(),
                                CreateDate = DateTime.Parse(row["CreateDate"].ToString())
                            };
                            flag = true;
                            break;
                        }
                    }
                }

                if (flag == false)
                {
                    DataTable dt = null;
                    dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, @"select * from TB_APPManagement").Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["ID"].Equals(ID))
                            {
                                model = new TB_APPManagement()
                                {
                                    ID = row["ID"].ToString(),
                                    VersionName = row["VersionName"].ToString(),
                                    VersionID = Convert.ToInt32(string.IsNullOrEmpty(row["VersionID"].ToString()) ? 0 : row["VersionID"]),
                                    CreatePerson = row["CreatePerson"].ToString(),
                                    CreateDate = DateTime.Parse(row["CreateDate"].ToString())
                                };
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag == true)
                    {
                        bool flag_xml = XmlHelper.WriteToXml(dt, path, "APPManagements", "APPManagement");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return model;
        }

        /// <summary>
        /// 根据id获取课程信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public TB_CurriculumManage GetCourseByID(int courseId)
        {
            TB_CurriculumManage courseInfo = cousedal.GetCourse(courseId);
            return courseInfo;
        }

        /// <summary>
        /// 根据bookID获取课程信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public TB_CurriculumManage GetCourseByBookID(int bookID)
        {
            TB_CurriculumManage courseInfo = cousedal.GetCourseByBookID(bookID);
            return courseInfo;
        }

        /// <summary>
        /// 条件查询
        /// </summary>    
        public IList<TB_CurriculumManage> GetCourseByCondition(string where)
        {
            return cousedal.GetCourseByCondition(where);
        }

        /// <summary>
        /// 添加课程信息
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool AddCourse(TB_CurriculumManage course)
        {
            if (cousedal.AddCourse(course))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取所有课程信息
        /// </summary>
        /// <returns></returns>
        public IList<TB_CurriculumManage> GetCourseList()
        {
            IList<TB_CurriculumManage> list = cousedal.QueryCourse();
            return list;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="courseInfo"></param>
        /// <returns></returns>
        public bool UpdateCourse(TB_CurriculumManage courseInfo)
        {
            bool b = cousedal.Update<TB_CurriculumManage>(courseInfo);
            if (b)
            {
                //初始化缓存
                // QuestionCache.InitVCourseCache();
            }
            return b;
        }

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        public ArrayList GetEdition()
        {
            // var edList = new ArrayList();
            return cousedal.GetEdition();
        }

        /// <summary>
        /// 根据条件获取课程信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_CurriculumManage> GetCourseList(string where)
        {
            return cousedal.GetCourseList(where);
        }

        /// <summary>
        /// 获取课程模块信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_ModuleSort> GetModuleSort(string where)
        {
            return cousedal.GetModuleSort(where);
        }

        /// <summary>
        /// 新增课程历史信息
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public bool AddBookHistorys(TB_UserBookHistory course)
        {
            return cousedal.AddBookHistory(course);
        }

        /// <summary>
        /// 获取当前用户所加载过的书籍信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        //public IList<View_UserHistory> GetListBookHistory(string where)
        //{
        //    return cousedal.GetListBookHistory(where); ;
        //}

        public DataSet GetListUserHistory(string where)
        {
            return cousedal.GetListUserHistory(where);
        }

    }
}
