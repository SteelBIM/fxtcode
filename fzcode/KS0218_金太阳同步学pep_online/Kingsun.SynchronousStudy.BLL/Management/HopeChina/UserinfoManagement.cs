using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Kingsun.DB;
using System.Web;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class UserinfoManagement : BaseManagement
    {
        ///// <summary>
        ///// 新增课程信息
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public KingResponse AddUserinfo(KingRequest request)
        //{
        //    //反序列化，得到客户端提交的数据
        //    //TbUserinfo Userinfo = JsonHelper.DecodeJson<TbUserinfo>(request.Data);

        //    TbUserInfo submitData = JsonHelper.DecodeJson<TbUserInfo>(request.Data);

        //    #region 校验相应的数据有效性

        //    if (submitData == null)
        //    {
        //        return KingResponse.GetErrorResponse("信息为空", request);
        //    }

        //    #endregion

        //    clientuserinfo = uums.GetUserInfoByID("ks0218", submitData.Userid);
        //    if (clientuserinfo != null)
        //    {
        //        //验证用户的电话号码
        //        if (!string.IsNullOrEmpty(clientuserinfo.Telephone))
        //        {
        //            submitData.AccountName = clientuserinfo.Telephone;
        //        }
        //        else
        //        {
        //            submitData.AccountName = clientuserinfo.UserName;
        //        }
        //    }
        //    bool result = true;
        //    try
        //    {
        //        result = Insert<TbUserInfo>(submitData);
        //        if (!result)
        //        {
        //            return KingResponse.GetErrorResponse("插入数据出错！", request);
        //        }
        //        return KingResponse.GetResponse(request, "插入数据完成");
        //    }
        //    catch
        //    {
        //        return KingResponse.GetErrorResponse("插入数据出错！", request);
        //    }
        //}

        
        public KingResponse UpdUserFinish(KingRequest request)
        {
            //反序列化，得到客户端提交的数据
            //TbUserinfo Userinfo = JsonHelper.DecodeJson<TbUserinfo>(request.Data);

            TbUserInfo submitData = JsonHelper.DecodeJson<TbUserInfo>(request.Data);

            #region 校验相应的数据有效性

            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("信息为空", request);
            }
            if (string.IsNullOrEmpty(submitData.Userid))
            {
                return KingResponse.GetErrorResponse("信息为空", request);
            }
            #endregion

            try
            {
                var userinfo = Select<TbUserInfo>(submitData.Userid);
                if (userinfo != null)
                {
                    userinfo.IsFinish = true;
                    userinfo.Finishtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 

                    var result = Update(userinfo);
                    if (!result)
                    {
                        return KingResponse.GetErrorResponse("插入数据出错！", request);
                    }
                    return KingResponse.GetResponse(request, "插入数据完成");
                }

                return KingResponse.GetErrorResponse("插入数据出错！", request);
            }
            catch
            {
                return KingResponse.GetErrorResponse("插入数据出错！", request);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryUserinfo(KingRequest request)
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
            parameter.OrderColumns = "Createtime";
            parameter.TbNames = "ITSV_Base.[FZ_SynchronousStudy].dbo.[tb_Userinfo]";
            parameter.IsOrderByASC = 1;
            parameter.Columns = "*";
            if (parameter.Where == null)
                parameter.Where = "";
            //IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = parameter.getParameterList();
            System.Data.DataSet ds = ExecuteProcedure("proc_pageView", list);
            DataTable dt = new DataTable();
            //查询到用户后 把用户的比赛成绩查询出来并且把用户的多个成绩合并一行,添加到新的datatable(我也不想这样,都怪sql写起来也麻烦,开发友好度差太多了)
            if (ds != null)
            {
                dt.Columns.Add("Userid");
                dt.Columns.Add("Username");
                dt.Columns.Add("AccountName");
                dt.Columns.Add("Parentname");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Teachername");
                dt.Columns.Add("Teacherphone");
                dt.Columns.Add("Schoolname");
                dt.Columns.Add("Period");
                dt.Columns.Add("Grade");
                dt.Columns.Add("Isjoin");
                dt.Columns.Add("IsFinish");
                dt.Columns.Add("Createtime");
                dt.Columns.Add("Finishtime");
                dt.Columns.Add("ArticleId"); //次数
                int tasktime = 0;
                var time = AppSetting.GetValue("TaskTime");

                int.TryParse(time, out tasktime);
                for (int i = 1; i <= tasktime; i++)
                {

                    dt.Columns.Add("Tasktime" + i); //次数
                    dt.Columns.Add("Taskvalue" + i); //分数
                    dt.Columns.Add("Filepath" + i); //路径
                    dt.Columns.Add("Completime" + i); //分数提交时间
                }
                var userlist = FillData<TbUserInfo>(ds.Tables[0]);
                ScoreManagement socreManagement = new ScoreManagement();
                //并行,其实并么有什么用 数据量并不大
                System.Threading.Tasks.Parallel.ForEach(userlist, item =>
                {
                    DataRow dr = dt.NewRow();
                    dr["Userid"] = item.Userid;
                    dr["Username"] = item.Username;
                    dr["AccountName"] = item.AccountName;
                    dr["Parentname"] = item.Parentname;
                    dr["Phone"] = item.Phone;
                    dr["Teachername"] = item.Teachername;
                    dr["Teacherphone"] = item.Teacherphone;
                    dr["Schoolname"] = item.Schoolname;
                    string text = string.Empty;
                    switch (item.Period.Trim())
                    {
                        case "F1":
                            text = "小学1~2年级(F)";
                            break;
                        case "F2":
                            text = "小学3~4年级(F)";
                            break;
                        case "F3":
                            text = "小学5~6年级(F)";
                            break;
                        case "F4":
                            text = "初中(F)";
                            break;
                        case "F5":
                            text = "高中(F)";
                            break;
                        case "F6":
                            text = "大学(F)";
                            break;
                        default:
                            text = "错误";
                            break;
                    }
                    dr["Period"] = text;
                    dr["Grade"] = item.Grade;
                    dr["Isjoin"] = item.Isjoin;
                    dr["IsFinish"] = item.IsFinish;
                    dr["Createtime"] = item.Createtime;
                    dr["Finishtime"] = item.Finishtime;

                    var socreList = socreManagement.QueryScoreByUserId(item.Userid);
                    if (socreList != null)
                    {
                        if (socreList.Count > 0)
                        {
                            for (int i = 0; i < socreList.Count; i++)
                            {
                                var scoreitem = socreList[i];
                                if (i < tasktime)
                                {
                                    dr["ArticleId"] = scoreitem.Articleid; //次数
                                    dr["Tasktime" + (i + 1)] = i + 1; //次数
                                    dr["Taskvalue" + (i + 1)] = scoreitem.Score; //分数
                                    dr["Filepath" + (i + 1)] = scoreitem.Filepath; //路径
                                    dr["Completime" + (i + 1)] = scoreitem.Createtime; //分数提交时间
                                }
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                });
            }
            else
            {
                dt = new DataTable();
            }
            //把datatable转换为list 最后返回json.dt序列化报错
            var result = (from row in dt.AsEnumerable()
                          select new
                          {
                              Userid = row.Field<string>("Userid"),
                              Username = row.Field<string>("Username"),
                              AccountName = row.Field<string>("AccountName"),
                              Parentname = row.Field<string>("Parentname"),
                              Phone = row.Field<string>("Phone"),
                              Teachername = row.Field<string>("Teachername"),
                              Teacherphone = row.Field<string>("Teacherphone"),
                              Schoolname = row.Field<string>("Schoolname"),
                              Period = row.Field<string>("Period"),
                              Grade = row.Field<string>("Grade"),
                              Isjoin = row.Field<string>("Isjoin"),
                              IsFinish = row.Field<string>("IsFinish"),
                              Createtime = row.Field<string>("Createtime"),
                              Finishtime = row.Field<string>("Finishtime"),

                              //分数信息 暂时只返回三个记录 
                              ArticleId = row.Field<string>("ArticleId"),
                              Tasktime1 = row.Field<string>("Tasktime1"),
                              Taskvalue1 = row.Field<string>("Taskvalue1"),
                              Filepath1 = row.Field<string>("Filepath1"),
                              Completime1 = row.Field<string>("Completime1"),
                              Tasktime2 = row.Field<string>("Tasktime2"),
                              Taskvalue2 = row.Field<string>("Taskvalue2"),
                              Filepath2 = row.Field<string>("Filepath2"),
                              Completime2 = row.Field<string>("Completime2"),
                              Tasktime3 = row.Field<string>("Tasktime3"),
                              Taskvalue3 = row.Field<string>("Taskvalue3"),
                              Filepath3 = row.Field<string>("Filepath3"),
                              Completime3 = row.Field<string>("Completime3")

                          }).ToList();
            return KingResponse.GetResponse(request, new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = result
            });
        }

        /// <summary>
        /// 根据条件返回数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryUserinfoById(KingRequest request)
        {
            TbUserInfo parameter = JsonHelper.DecodeJson<TbUserInfo>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (parameter.Userid == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            var list = Select<TbUserInfo>(parameter.Userid);
            if (list != null)
            {
                return KingResponse.GetResponse(request, list);
            }

            return KingResponse.GetErrorResponse("当前查询发生异常", request);
        }

        public KingResponse StarPageByUserId(KingRequest request)
        {
            TbUserInfo submitData = JsonHelper.DecodeJson<TbUserInfo>(request.Data);
            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("信息为空", request);
            }
            if (string.IsNullOrEmpty(submitData.Userid))
            {
                return KingResponse.GetErrorResponse("信息为空", request);
            }
            try
            {
                var registerStarTime = AppSetting.GetValue("RegisterStarTime").ToIntOrZero();
                var registerEndTime = AppSetting.GetValue("RegisterEndTime").ToIntOrZero();
                var matchStarTime = AppSetting.GetValue("MatchStarTime").ToIntOrZero();
                var matchEndTime = AppSetting.GetValue("MatchEndTime").ToIntOrZero();
                var dateNow = DateTime.Now.ToString("yyyyMMdd").ToIntOrZero();

                var userinfo = Select<TbUserInfo>(submitData.Userid);
              
                //报名未开始
                if (dateNow < registerStarTime)
                {
                    return KingResponse.GetResponse(request, "0");
                }
                //报名期间 验证是否已报名
                if (dateNow >= registerStarTime && dateNow <= registerEndTime)
                {
                    if (userinfo == null)
                    {
                        //未报名
                        return KingResponse.GetResponse(request, "1");
                    }
                    //已报名
                    return KingResponse.GetResponse(request, "2");
                }
                //报名结束比赛未开始
                if (dateNow > registerEndTime && dateNow < matchStarTime)
                {
                    if (userinfo == null)
                    {
                        //未报名 错过报名时间
                        return KingResponse.GetResponse(request, "4");
                    }
                    //已报名
                    return KingResponse.GetResponse(request, "3");
                }
                //比赛期间 验证是否已报名
                if (dateNow >= matchStarTime && dateNow <= matchEndTime)
                {
                    if (userinfo == null)
                    {
                        //未报名 错过报名时间
                        return KingResponse.GetResponse(request, "1");
                    }
                    //已报名
                    return KingResponse.GetResponse(request, "5");
                }

                if (dateNow > matchEndTime)
                {
                    if (userinfo != null)
                    {
                        userinfo.IsFinish = true;
                        Update<TbUserInfo>(userinfo);
                        //已报名
                        return KingResponse.GetResponse(request, "5");
                    }
                }
                return KingResponse.GetResponse(request, "4");
            }
            catch
            {
                return KingResponse.GetErrorResponse("数据出错！", request);
            }
        }

        public List<T> ToList<T>(DataTable obj) where T : new()
        {
            List<T> list = new List<T>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            try
            {
                foreach (DataRow row in obj.Rows)
                {
                    T local = Activator.CreateInstance<T>();
                    foreach (PropertyInfo info in properties)
                    {
                        if (obj.Columns.Contains(info.Name))
                        {
                            if (row[obj.Columns[info.Name]] != DBNull.Value)
                            {
                                info.SetValue(local, row[obj.Columns[info.Name]], null);
                            }
                            else
                            {
                                info.SetValue(local, null, null);
                            }
                        }
                    }
                    list.Add(local);
                }
            }
            catch (Exception)
            {

            }
            return list;
        }
    }
}
