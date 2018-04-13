using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Kingsun.DB;
using System.Web;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class CourseManagement : BaseManagement
    {
        /// <summary>
        /// 新增课程信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse AddCourse(KingRequest request)
        {
            //反序列化，得到客户端提交的数据
            CourseSubmitModel submitData = JsonHelper.DecodeJson<CourseSubmitModel>(request.Data);

            TB_CourseVersion subVersion = JsonHelper.DecodeJson<TB_CourseVersion>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("用户信息为空", request);
            }
            if (!submitData.AppID.HasValue || submitData.AppID.Value == Guid.Empty)
            {
                return KingResponse.GetErrorResponse("对应的应用为空", request);
            }
            if (!submitData.SubjectID.HasValue || submitData.SubjectID.Value == 0)
            {
                return KingResponse.GetErrorResponse("学科信息为空", request);
            }
            if (!submitData.GradeID.HasValue || submitData.GradeID.Value == 0)
            {
                return KingResponse.GetErrorResponse("年级信息为空", request);
            }
            if (!submitData.EditionID.HasValue || submitData.EditionID.Value == 0)
            {
                return KingResponse.GetErrorResponse("版本信息为空", request);
            }
            if (string.IsNullOrEmpty(submitData.Creator))
            {
                return KingResponse.GetErrorResponse("创建者信息为空", request);
            }
            #endregion

            submitData.CreateDateTime = submitData.ModifyDateTime = DateTime.Now;
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            bool result = false;
            try
            {
                dbManage.BeginTransaction();
                result = dbManage.Insert<TB_Course>(submitData);
                if (!result)
                {
                    dbManage.Rollback();
                    return KingResponse.GetErrorResponse("插入课程信息出错！", request);
                }
                TB_CourseVersion version = new TB_CourseVersion();
                version.CourseID = submitData.ID;
                version.UpdateURL = submitData.FilePath;
                version.UpdateMD5 = submitData.FileMD5;
                version.Disable = true;
                version.CreateDateTime = DateTime.Now;
                version.Version = submitData.Version;
                version.TryUpdate = subVersion.TryUpdate;
                if (submitData.FilePath != null)
                    version.FirstPageNum = subVersion.FirstPageNum;

                version.Description = "初始版本";
                result = dbManage.Insert<TB_CourseVersion>(version);
                if (!result)
                {
                    dbManage.Rollback();
                    return KingResponse.GetErrorResponse("插入课程版本信息出错！", request);
                }
                dbManage.Commit();
            }
            catch
            {
                dbManage.Rollback();
                result = false;
            }
            if (result)
            {
                return KingResponse.GetResponse(request,
                    new
                    {
                        Success = true,
                        ID = submitData.ID
                    });
            }
            else
            {
                return KingResponse.GetErrorResponse("插入数据出错，提示：" + _operatorError, request);
            }
        }

        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse EditCourse(KingRequest request)
        {
            //反序列化，得到客户端提交的数据
            TB_Course submitData = JsonHelper.DecodeJson<TB_Course>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("用户信息为空", request);
            }
            if (string.IsNullOrEmpty(submitData.ID))
            {
                return KingResponse.GetErrorResponse("课程ID为空", request);
            }
            if (string.IsNullOrEmpty(submitData.ImageURL))
            {
                return KingResponse.GetErrorResponse("课程封皮为空", request);
            }
            if (string.IsNullOrEmpty(submitData.Version))
            {
                return KingResponse.GetErrorResponse("课程版本为空", request);
            }
            #endregion
            TB_Course currentCourse = Select<TB_Course>(submitData.ID);
            if (currentCourse == null)
            {
                return KingResponse.GetErrorResponse("找不到对应的课程信息", request);
            }
            currentCourse.ImageURL = submitData.ImageURL;
            currentCourse.Version = submitData.Version;
            currentCourse.Description = submitData.Description;
            currentCourse.ModifyDateTime = DateTime.Now;
            currentCourse.Sort = submitData.Sort;

            string cookie = HttpContext.Current.Request.Cookies["KSRead"].Value ?? "";
            if (cookie == "")
            {
                return KingResponse.GetErrorResponse("没有登陆信息", request);
            }
            string[] arrCookie = cookie.Split('&');
            for (var i = 0; i < arrCookie.Length; i++)
            {
                string[] arr = arrCookie[i].Split('=');
                if (arr[0] == "UserName")
                {
                    currentCourse.Creator = arr[1];
                    break;
                }
            }

            if (Update<TB_Course>(currentCourse))
            {
                return KingResponse.GetResponse(request,
                    new
                    {
                        Success = true
                    });
            }
            else
            {
                return KingResponse.GetErrorResponse("更新数据出错，提示：" + _operatorError, request);
            }
        }

        /// <summary>
        /// 更新课程，新增一个版本
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse UpdateCourse(KingRequest request)
        {
            TB_CourseVersion versionData = JsonHelper.DecodeJson<TB_CourseVersion>(request.Data);
            #region 验证获取的数据有效性
            if (versionData == null)
            {
                return KingResponse.GetErrorResponse("提交版本信息为空", request);
            }
            if (string.IsNullOrEmpty(versionData.Version))
            {
                return KingResponse.GetErrorResponse("课程版本为空", request);
            }
            if (string.IsNullOrEmpty(versionData.UpdateURL))
            {
                return KingResponse.GetErrorResponse("增量更新url为空", request);
            }
            if (!versionData.FirstPageNum.HasValue || versionData.FirstPageNum.Value == 0)
            {
                return KingResponse.GetErrorResponse("课程首页页码值错误", request);
            }
            #endregion
            var courseList = Search<TB_CurriculumManage>("Bookid='" + versionData.CourseID + "'");

            if (courseList == null)
            {
                return KingResponse.GetErrorResponse("找不到对应的课程信息", request);
            }
            var courseData = courseList[0];
            versionData.Disable = false;
            string cookie = HttpContext.Current.Request.Cookies["FZKSTBX"].Value ?? "";
            if (cookie == "")
            {
                return KingResponse.GetErrorResponse("没有登陆信息", request);
            }
            string[] arrCookie = cookie.Split('&');
            for (var i = 0; i < arrCookie.Length; i++)
            {
                string[] arr = arrCookie[i].Split('=');
                if (arr[0] == "UserName")
                {
                    versionData.Creator = arr[1];
                    break;
                }
            }
            bool b = false;
            string sql = string.Format(@"     SELECT ModularID,ModularName FROM dbo.TB_ModularManage WHERE State=1 AND SuperiorID=0 AND ModularID='{0}'", versionData.ModuleID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    b = ds.Tables[0].Rows[0]["ModularName"].ToString().Contains("YX");
                }
            }
            if (b)
            {
                TB_CourseVersion_YX cyx = new TB_CourseVersion_YX()
                {
                    FirstPageNum = versionData.FirstPageNum,
                    TryUpdate = versionData.TryUpdate,
                    Creator = versionData.Creator,
                    ModuleID = versionData.ModuleID,
                    Description = versionData.Description,
                    CourseID = versionData.CourseID,
                    Version = versionData.Version,
                    UpdateMD5 = versionData.UpdateMD5,
                    UpdateURL = versionData.UpdateURL,
                    CompleteMD5 = versionData.CompleteMD5,
                    CompleteURL = versionData.CompleteURL,
                };
                if (Insert<TB_CourseVersion_YX>(cyx))
                {
                    return KingResponse.GetResponse(request, new
                    {
                        Success = true
                    });
                }
                else
                {
                    return KingResponse.GetErrorResponse("更新版本数据错误，提示：" + _operatorError, request);
                }
            }
            else
            {
                if (Insert<TB_CourseVersion>(versionData))
                {
                    return KingResponse.GetResponse(request, new
                    {
                        Success = true
                    });
                }
                else
                {
                    return KingResponse.GetErrorResponse("更新版本数据错误，提示：" + _operatorError, request);
                }
            }
        }

        /// <summary>
        /// 激活课程
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse ActiveCourse(KingRequest request)
        {
            //反序列化，得到客户端提交的数据
            TB_Course submitData = JsonHelper.DecodeJson<TB_Course>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空", request);
            }
            if (string.IsNullOrEmpty(submitData.ID))
            {
                return KingResponse.GetErrorResponse("课程ID为空", request);
            }
            #endregion
            TB_Course currentCourse = Select<TB_Course>(submitData.ID);
            if (currentCourse == null)
            {
                return KingResponse.GetErrorResponse("找不到对应的课程信息", request);
            }
            currentCourse.Disable = true;
            if (Update<TB_Course>(currentCourse))
            {
                return KingResponse.GetResponse(request,
                    new
                    {
                        Success = true
                    });
            }
            else
            {
                return KingResponse.GetErrorResponse("更新数据出错，提示：" + _operatorError, request);
            }
        }

        /// <summary>
        /// 禁用课程
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse DisableCourse(KingRequest request)
        {
            //反序列化，得到客户端提交的数据
            TB_Course submitData = JsonHelper.DecodeJson<TB_Course>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空", request);
            }
            if (string.IsNullOrEmpty(submitData.ID))
            {
                return KingResponse.GetErrorResponse("课程ID为空", request);
            }
            #endregion
            TB_Course currentCourse = Select<TB_Course>(submitData.ID);
            if (currentCourse == null)
            {
                return KingResponse.GetErrorResponse("找不到对应的课程信息", request);
            }
            currentCourse.Disable = false;
            if (Update<TB_Course>(currentCourse))
            {
                return KingResponse.GetResponse(request,
                    new
                    {
                        Success = true
                    });
            }
            else
            {
                return KingResponse.GetErrorResponse("更新数据出错，提示：" + _operatorError, request);
            }
        }

        /// <summary>
        /// 查询课程信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse SelectCourse(KingRequest request)
        {
            TB_Course subData = JsonHelper.DecodeJson<TB_Course>(request.Data);
            if (subData == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (string.IsNullOrEmpty(subData.ID))
            {
                return KingResponse.GetErrorResponse("课程ID为空", request);
            }
            TB_Course result = Select<TB_Course>(subData.ID);
            if (result == null)
            {
                return KingResponse.GetErrorResponse("找不到相应的信息", request);
            }
            else
            {
                return KingResponse.GetResponse(request, result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryCourse(KingRequest request)
        {
            PageParameter parameter = JsonHelper.DecodeJson<PageParameter>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (parameter.PageIndex < 1)
            {
                return KingResponse.GetErrorResponse("当前页面数不正确", request);
            }
            if (parameter.PageSize < 1)
            {
                return KingResponse.GetErrorResponse("当前页pageSize不正确", request);
            }
            parameter.OrderColumns = "CreateDateTime";
            parameter.TbNames = "V_Course";
            parameter.IsOrderByASC = 2;
            parameter.Columns = "*";
            if (parameter.Where == null)
                parameter.Where = "";
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = parameter.getParameterList();
            //System.Data.Common.DbParameter param=
            System.Data.DataSet ds = dbManage.ExecuteProcedure("proc_pageView", list);

            return KingResponse.GetResponse(request, new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = dbManage.FillData<TB_Course>(ds.Tables[0])
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryCourseName(KingRequest request)
        {
            PageParameter parameter = JsonHelper.DecodeJson<PageParameter>(request.Data);
            TB_Course versionData = JsonHelper.DecodeJson<TB_Course>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (parameter.PageIndex < 1)
            {
                return KingResponse.GetErrorResponse("当前页面数不正确", request);
            }
            if (parameter.PageSize < 1)
            {
                return KingResponse.GetErrorResponse("当前页pageSize不正确", request);
            }
            parameter.OrderColumns = "CreateDateTime";
            parameter.TbNames = "TB_Course";
            parameter.IsOrderByASC = 2;
            parameter.Columns = "*";
            if (!parameter.Where.StartsWith("1="))
            {
                parameter.Where = " CourseName like '%" + parameter.Where + "%' and AppID = '" + versionData.AppID + "'";
            }
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = parameter.getParameterList();
            //System.Data.Common.DbParameter param=
            System.Data.DataSet ds = dbManage.ExecuteProcedure("proc_pageView", list);

            return KingResponse.GetResponse(request, new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = dbManage.FillData<TB_Course>(ds.Tables[0])
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryCourseVersion(KingRequest request)
        {
            PageParameter parameter = JsonHelper.DecodeJson<PageParameter>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (parameter.PageIndex < 1)
            {
                return KingResponse.GetErrorResponse("当前页面数不正确", request);
            }
            if (parameter.PageSize < 1)
            {
                return KingResponse.GetErrorResponse("当前页pageSize不正确", request);
            }
            parameter.OrderColumns = "CreateDateTime";
            parameter.TbNames = "TB_CourseVersion";
            parameter.IsOrderByASC = 2;
            parameter.Columns = "*";
            if (!parameter.Where.StartsWith("1="))
            {
                parameter.Where = " CourseID = '" + parameter.Where + "'";
            }
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = parameter.getParameterList();
            //System.Data.Common.DbParameter param=
            System.Data.DataSet ds = dbManage.ExecuteProcedure("proc_pageView", list);

            return KingResponse.GetResponse(request, new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = dbManage.FillData<TB_CourseVersion>(ds.Tables[0])
            });
        }

        /// <summary>
        /// 查询课程信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse SelectCourseVersion(KingRequest request)
        {
            PageParameter parameter = JsonHelper.DecodeJson<PageParameter>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (parameter.PageIndex < 1)
            {
                return KingResponse.GetErrorResponse("当前页面数不正确", request);
            }
            if (parameter.PageSize < 1)
            {
                return KingResponse.GetErrorResponse("当前页pageSize不正确", request);
            }
            parameter.OrderColumns = "CreateDateTime";
            parameter.TbNames = "TB_CourseVersion";
            parameter.IsOrderByASC = 2;
            parameter.Columns = "*";
            if (!parameter.Where.StartsWith("1="))
            {
                parameter.Where = " CourseName like '%" + parameter.Where + "%'";
            }
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = parameter.getParameterList();
            //System.Data.Common.DbParameter param=
            System.Data.DataSet ds = dbManage.ExecuteProcedure("proc_pageView", list);

            return KingResponse.GetResponse(request, new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = dbManage.FillData<TB_CourseVersion>(ds.Tables[0])
            });
        }

        /// <summary>
        /// 激活课程
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse ActiveCourseVersion(KingRequest request)
        {
            //反序列化，得到客户端提交的数据
            TB_CourseVersion submitData = JsonHelper.DecodeJson<TB_CourseVersion>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空", request);
            }
            if (!submitData.ID.HasValue || submitData.ID.Value == Guid.Empty)
            {
                return KingResponse.GetErrorResponse("课程ID为空", request);
            }
            #endregion
            TB_CourseVersion currentCourse = Select<TB_CourseVersion>(submitData.ID.Value);
            if (currentCourse == null)
            {
                return KingResponse.GetErrorResponse("找不到对应的课程信息", request);
            }
            currentCourse.Disable = true;
            if (Update<TB_CourseVersion>(currentCourse))
            {
                return KingResponse.GetResponse(request, new { Success = true });
            }
            return KingResponse.GetErrorResponse(_operatorError, request);
        }

        /// <summary>
        /// 禁用课程
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse DisableCourseVersion(KingRequest request)
        {
            //反序列化，得到客户端提交的数据
            TB_CourseVersion submitData = JsonHelper.DecodeJson<TB_CourseVersion>(request.Data);
            #region 校验相应的数据有效性
            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空", request);
            }
            if (!submitData.ID.HasValue || submitData.ID.Value == Guid.Empty)
            {
                return KingResponse.GetErrorResponse("课程ID为空", request);
            }
            #endregion
            TB_CourseVersion currentCourse = Select<TB_CourseVersion>(submitData.ID.Value);
            if (currentCourse == null)
            {
                return KingResponse.GetErrorResponse("找不到对应的课程信息", request);
            }
            currentCourse.Disable = false;
            if (Update<TB_CourseVersion>(currentCourse))
            {
                return KingResponse.GetResponse(request, new { Success = true });
            }
            return KingResponse.GetErrorResponse(_operatorError, request);
        }

        /// <summary>
        /// 激活版本
        /// </summary>
        /// <param name="ID">版本ID</param>
        /// <param name="AppID">应用ID</param>
        /// <returns></returns>
        protected bool ActiveCourseVersion(string ID, string CourseID, int Disable)
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            System.Data.SqlClient.SqlParameter param0 = new System.Data.SqlClient.SqlParameter("@ID", ID);
            System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@CourseID", CourseID);
            System.Data.SqlClient.SqlParameter param2 = new System.Data.SqlClient.SqlParameter("@Disable", Disable);
            list.Add(param0);
            list.Add(param1);
            list.Add(param2);
            dbManage.ExecuteProcedure("proc_ActiveCourseVersion", list);
            return dbManage.ErrorMsg == null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse EditCourseVersion(KingRequest request)
        {
            TB_CourseVersion versionData = JsonHelper.DecodeJson<TB_CourseVersion>(request.Data);
            #region 验证相关数据
            if (versionData == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空", request);
            }
            if (!versionData.ID.HasValue || versionData.ID == Guid.Empty)
            {
                return KingResponse.GetErrorResponse("ID为空", request);
            }
            if (string.IsNullOrEmpty(versionData.UpdateURL))
            {
                return KingResponse.GetErrorResponse("更新文件目录为空", request);
            }
            if (string.IsNullOrEmpty(versionData.UpdateMD5))
            {
                return KingResponse.GetErrorResponse("更新文件MD5值为空", request);
            }
            if (!versionData.FirstPageNum.HasValue || versionData.FirstPageNum.Value == 0)
            {
                return KingResponse.GetErrorResponse("课程首页页码值错误", request);
            }
            #endregion
            TB_CourseVersion Version = Select<TB_CourseVersion>(versionData.ID.Value);

            if (Version == null)
            {
                return KingResponse.GetErrorResponse("找不到相关版本信息", request);
            }

            var uc = HttpContext.Current.Request.Cookies["FZKSTBX"];
            if (uc == null)
            {
                Version.Creator = "";
                //return KingResponse.GetErrorResponse("没有登陆信息", request);
            }
            else
            {
                string cookie = uc.Value;
                string[] arrCookie = cookie.Split('&');
                for (var i = 0; i < arrCookie.Length; i++)
                {
                    string[] arr = arrCookie[i].Split('=');
                    if (arr[0] == "UserName")
                    {
                        Version.Creator = arr[1];
                        break;
                    }
                }
            }

            Version.UpdateURL = versionData.UpdateURL;
            Version.UpdateMD5 = versionData.UpdateMD5;
            Version.CompleteMD5 = versionData.CompleteMD5;
            Version.CompleteURL = versionData.CompleteURL;
            Version.Description = versionData.Description;
            Version.ModifyDateTime = DateTime.Now;
            Version.FirstPageNum = versionData.FirstPageNum;
            Version.TryUpdate = versionData.TryUpdate;
            Version.ModuleID = versionData.ModuleID;
            if (Update<TB_CourseVersion>(Version))
            {
                return KingResponse.GetResponse(request,
                                   new
                                   {
                                       Success = true
                                   });
            }
            else
            {
                return KingResponse.GetErrorResponse("修改数据出错" + _operatorError, request);
            }
        }

        /// <summary>
        /// 获取最大的版本
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse SelectTopVersion(KingRequest request)
        {
            TB_CourseVersion versonData = JsonHelper.DecodeJson<TB_CourseVersion>(request.Data);
            #region 校验相应的数据有效性
            if (versonData == null)
            {
                return KingResponse.GetErrorResponse("当前数据不存在", request);
            }
            if (string.IsNullOrEmpty(versonData.CourseID))
            {
                return KingResponse.GetErrorResponse("课程ID为空", request);
            }
            #endregion


            var list = Search<TB_CourseVersion>("CourseID='" + versonData.CourseID + "' order by dbo.fn_version(Version) desc");
            if (list == null)
            {
                return KingResponse.GetResponse(request, new
                {
                    Success = true,
                    value = "0.99.999"
                });
            }

            return KingResponse.GetResponse(request, new
            {
                Success = true,
                value = list[0].Version
            });
        }

        /// <summary>
        /// 获取原始版本
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse SelectFirstVersion(KingRequest request)
        {
            TB_Course versonData = JsonHelper.DecodeJson<TB_Course>(request.Data);
            #region 校验相应的数据有效性
            if (versonData == null)
            {
                return KingResponse.GetErrorResponse("当前数据不存在", request);
            }
            if (string.IsNullOrEmpty(versonData.ID))
            {
                return KingResponse.GetErrorResponse("课程ID为空", request);
            }
            #endregion
            TB_Course CourseData = Select<TB_Course>(versonData.ID);
            if (CourseData == null)
            {
                return KingResponse.GetErrorResponse("找不到对应的课程", request);
            }
            return KingResponse.GetResponse(request, new
            {
                Success = true,
                value = CourseData.Version
            });
        }

        /// <summary>
        /// 查询启用版本中的最大版本
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse SelectMaxDisableVersion(KingRequest request)
        {
            TB_CourseVersion versonData = JsonHelper.DecodeJson<TB_CourseVersion>(request.Data);
            #region 校验相应的数据有效性
            if (versonData == null)
            {
                return KingResponse.GetErrorResponse("当前数据不存在", request);
            }
            if (string.IsNullOrEmpty(versonData.CourseID))
            {
                return KingResponse.GetErrorResponse("找不到应用程序ID", request);
            }
            #endregion
            IList<TB_CourseVersion> CourseList = Search<TB_CourseVersion>("CourseID='" + versonData.CourseID + "' and Disable=1", " version desc");
            if (CourseList == null)
            {
                return KingResponse.GetErrorResponse("找不到对应的版本", request);
            }
            if (CourseList.Count > 0)
            {
                return KingResponse.GetResponse(request, CourseList[0]);
            }
            else
            {
                return KingResponse.GetErrorResponse("找不到对应的版本", request);
            }
        }

        /// <summary>
        /// 删除课程，包含删除版本
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal KingResponse DeleteCourse(KingRequest request)
        {
            //throw new NotImplementedException();
            TB_Course versonData = JsonHelper.DecodeJson<TB_Course>(request.Data);
            #region 校验相应的数据有效性
            if (versonData == null)
            {
                return KingResponse.GetErrorResponse("当前数据不存在", request);
            }
            if (string.IsNullOrEmpty(versonData.ID))
            {
                return KingResponse.GetErrorResponse("找不到应用程序ID", request);
            }
            #endregion
            TB_Course CourseData = Select<TB_Course>(versonData.ID);
            if (CourseData == null)
            {
                return KingResponse.GetErrorResponse("找不到对应的课程", request);
            }
            if (!CourseData.Disable.Value)
            {
                //取消删除前判断是否启用
                //return KingResponse.GetErrorResponse("课程启用状态无法删除！", request);
            }
            IList<TB_CourseVersion> cvList = Search<TB_CourseVersion>("CourseID='" + CourseData.ID + "' and disable=1");
            if (cvList != null)
            {
                //取消删除前判断是否有课程在使用
                //return KingResponse.GetErrorResponse("还有课程正在使用，无法删除！", request);
            }
            List<System.Data.Common.DbParameter> paramlist = new List<System.Data.Common.DbParameter>();
            System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@CourseID", versonData.ID);
            paramlist.Add(param);
            System.Data.DataSet ds = ExecuteProcedure("proc_DeleteCourse", paramlist);
            if (ds.Tables[ds.Tables.Count - 1].Rows[0]["result"].ToString() != "true")
            {
                return KingResponse.GetErrorResponse("删除失败。", request);
            }
            else
            {
                return KingResponse.GetResponse(request, "删除成功！");
            }
        }
    }
}
