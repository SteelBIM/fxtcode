using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.App.Filter;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Common.Model;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.BLL;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    /// <summary>
    /// 整包课程相关接口
    /// </summary>
    public class YxCourseController : ApiController
    {
        BaseManagement bm = new BaseManagement();
        CourseBLL coursebll = new CourseBLL();
        /// <summary>
        /// 获取书的版本，合集版用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse GetBookVersion()
        {
            var re = XmlHelper.Read<BookVersion>(@"XmlFiles\APPManagement.xml", "APPManagements");
            return GetResult(re);
        }

        /// <summary>
        /// 获取科目，合集版用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse GetSubject()
        {
            var re = XmlHelper.Read<Subject>(@"XmlFiles\Subject.xml", "Subjects");
            return GetResult(re);
        }

        /// <summary>
        /// 新增用户书本记录，合集版用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ShowApi]
        public ApiResponse AddUserBookInfo([FromBody] KingRequest request)
        {
            UserBookInfo submitData = JsonHelper.DecodeJson<UserBookInfo>(request.Data);
            if (submitData == null)
            {
                return GetErrorResult("输入参数信息为空！");
            }
            RedisHashHelper redis = new RedisHashHelper();
            var re = redis.Set<UserBookInfo>("UserBookInfo", submitData.UserID, submitData);
            return GetResult("保存成功！");
        }

        /// <summary>
        /// 获取用户书本记录，合集版用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse GetUserBookInfo(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return GetErrorResult("userID不能为空！");
            }
            RedisHashHelper redis = new RedisHashHelper();
            UserBookInfo caa = redis.Get<UserBookInfo>("UserBookInfo", userID);

            if (caa == null)
            {
                return GetErrorResult("没有找到对应的用户信息！");
            }

            //1 获取科目
            var subjects = XmlHelper.Read<Subject>(@"XmlFiles\Subject.xml", "Subjects");
            string subjectID = caa.SubjectID.ToString();
            var whereSubjects = subjects.Where(x => x.ClassifyID == subjectID);
            if (whereSubjects.Count() == 0)
            {
                return GetErrorResult("没有找到对应的科目！");
            }
            var subject = whereSubjects.First();

            //2 获取版本
            var versions = XmlHelper.Read<BookVersion>(@"XmlFiles\APPManagement.xml", "APPManagements");
            string appID = caa.AppID.ToString();
            var whereVersions = versions.Where(x => x.ID == appID);
            if (whereVersions.Count() == 0)
            {
                return GetErrorResult("没有找到对应的版本！");
            }
            var version = whereVersions.First();

            //3 获取书本
            string errMsg;
            var courses = coursebll.GetCourseListMethod(caa.AppID, out errMsg);
            if (errMsg != "")
            {
                return GetErrorResult(errMsg);
            }
            int bookID = Convert.ToInt32(caa.CourseID);
            var whereCourses = courses.Where(x => x.BookID == bookID);
            if (whereCourses.Count() == 0)
            {
                return GetErrorResult("没有找到对应的课本信息！");
            }
            var course = whereCourses.First();

            return GetResult(new { subject = subject, version = version, course = course });
        }

        /// <summary>
        /// 通过BookID获取下载信息
        /// </summary>
        /// <param name="CourseID">BookID</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse QueryCourse(string CourseID)
        {
            var courseList = bm.Search<TB_CourseVersion>(" CourseID='" + CourseID + "' and Disable='True' order by dbo.fn_version(Version) desc");
            if (courseList == null)
            {
                return GetErrorResult("此课本没有配置可用版本");
            }

            return GetResult(new
            {
                //BookID = courseList[0].CourseID,
                //ModuleID = "1",
                //ModuleName = "点读",
                //ModuleSort = "0",
                ModuleAddress = courseList[0].UpdateURL,
                MD5 = courseList[0].UpdateMD5,
                //IncrementalPacketAddress = "",
                //IncrementalPacketMD5 = "",
                ModuleVersion = courseList[0].Version
            });
        }

        /// <summary>
        /// 检查课程更新
        /// </summary>
        /// <param name="CourseID">课程ID</param>
        /// <param name="Version">课程版本(1.0.0)</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse CheckCourseUpdate(string CourseID, string Version)
        {


            var courseList = bm.Search<TB_CourseVersion>(" CourseID='" + CourseID + "' and dbo.fn_version(Version)>dbo.fn_version('" + Version + "')  and Disable='True' order by dbo.fn_version(Version) desc");
            if (courseList == null)
            {
                return GetErrorResult("没有更新");
            }

            return GetResult(new
            {
                //BookID = courseList[0].CourseID,
                //ModuleID = "1",
                //ModuleName = "点读",
                //ModuleSort = "0",
                ModuleAddress = courseList[0].UpdateURL,
                MD5 = courseList[0].UpdateMD5,
                //IncrementalPacketAddress = "",
                //IncrementalPacketMD5 = "",
                ModuleVersion = courseList[0].Version
            });
        }

        private ApiResponse GetErrorResult(string message)
        {
            return new ApiResponse
            {
                Success = false,
                data = null,
                Message = message
            };
        }

        private ApiResponse GetResult(object Data, string message = "")
        {

            return new ApiResponse
            {
                Success = true,
                data = Data,
                Message = message
            };
        }

        /// <summary>
        /// 根据bookid，模块id获取更新信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse GetExerciseBookUpdate([FromBody] KingRequest request)
        {
            EbookList submitData = JsonHelper.DecodeJson<EbookList>(request.Data);
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.CourseID))
            {
                return GetErrorResult("书籍ID不能为空");
            }
            if (string.IsNullOrEmpty(submitData.ModuleID))
            {
                return GetErrorResult("模块ID不能为空");
            }
            var courseList = bm.Search<TB_CourseVersion>(" CourseID='" + submitData.CourseID + "' and ModuleID='" + submitData.ModuleID + "' and dbo.fn_version(Version)>dbo.fn_version('" + submitData.Version + "')  and Disable='True' order by dbo.fn_version(Version) desc");
            if (courseList == null)
            {
                return GetErrorResult("没有更新");
            }
            return GetResult(new
            {
                ModuleAddress = courseList[0].UpdateURL,
                MD5 = courseList[0].UpdateMD5,
                ModuleVersion = courseList[0].Version
            });
        }

        /// <summary>
        /// 根据bookid，模块id获取下载信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse GetExerciseBookInfo([FromBody] KingRequest request)
        {
            EbookList submitData = JsonHelper.DecodeJson<EbookList>(request.Data);
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.CourseID))
            {
                return GetErrorResult("书籍ID不能为空");
            }
            if (string.IsNullOrEmpty(submitData.ModuleID))
            {
                return GetErrorResult("模块ID不能为空");
            }
            var courseList = bm.Search<TB_CourseVersion>(" CourseID='" + submitData.CourseID + "' and ModuleID='" + submitData.ModuleID + "'  and Disable='True' order by dbo.fn_version(Version) desc");
            if (courseList == null)
            {
                return GetErrorResult("此课本没有配置可用版本");
            }
            return GetResult(new
            {
                ModuleAddress = courseList[0].UpdateURL,
                MD5 = courseList[0].UpdateMD5,
                ModuleVersion = courseList[0].Version
            });


        }
    }
}
