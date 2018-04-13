using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.BLL;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.Common.Base;
using System.Linq;
using System.Collections;
namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class YxHearResourcesController : ApiController
    {
        YXHearResourcesBLL hearResourcesBLL = new YXHearResourcesBLL();
        ModularManageBLL modularManageBLL = new ModularManageBLL();
        string FiedURL = WebConfigurationManager.AppSettings["FileServerUrl"];
        string SetSuishengting = WebConfigurationManager.AppSettings["rjsst"];
        private BaseManagement bm = new BaseManagement();
        static RedisHashHelper redis = new RedisHashHelper();
        private RedisListHelper redisList = new RedisListHelper();

        /// <summary>
        /// 获取一级模块下，用户在二级模块的得分情况/历史记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetListenSpeakAchievement([FromBody]KingRequest request)
        {
            string where = "1=1";
            ModuleInfo submitData = JsonHelper.DecodeJson<ModuleInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID) || string.IsNullOrEmpty(submitData.BookID) || string.IsNullOrEmpty(submitData.FirstTitleID) || string.IsNullOrEmpty(submitData.FirstModuleID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            try
            {
                var hearResources = redis.Get<List<ListSubModules_0>>("HearResources_" + submitData.BookID, submitData.FirstTitleID + "_" + submitData.FirstModuleID);
                if (hearResources == null)
                {
                    string sql = @"
                    select  a.SecondTitleID,a.SecondModularID,b.ModularName,a.ModularEN,a.RepeatNumber,a.SerialNumber,a.TextSerialNumber
                    from [FZ_HearResources].[dbo].[TB_HearResources_YX] a 
                    left join TB_ModularManage b on  a.FirstModularID=b.SuperiorID and a.SecondModularID=b.ModularID and b.State=1 
                    --left join [FZ_HearResources].[dbo].[TB_UserHearResources_YX] c on a.BookID=c.BookID and a.FirstTitleID=c.FirstTitleID and a.SecondTitleID=c.SecondTitleID 
                    where {0}
                    group by  a.SecondTitleID,a.SecondModularID,b.ModularName,a.ModularEN,a.RepeatNumber,a.SerialNumber,a.TextSerialNumber";
                    where += " and a.BookID = " + ParseInt(submitData.BookID);
                    where += " and a.FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                    where += " and a.FirstModularID = " + ParseInt(submitData.FirstModuleID);

                    sql = string.Format(sql, where);
                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    List<ListSubModules_0> rowList = JsonHelper.DataSetToIList<ListSubModules_0>(ds, 0);
                    redis.Set("HearResources_" + submitData.BookID, submitData.FirstTitleID + "_" + submitData.FirstModuleID, rowList);
                    hearResources = rowList;
                }

                if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                {
                    //  where += " and a.SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                    hearResources = hearResources.Where(o => o.SecondTitleID == int.Parse(submitData.SecondTitleID)).ToList();
                }

                // sql = string.Format(sql, where);
                //     DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql).Tables[0];
                if (hearResources != null && hearResources.Any())
                {
                    #region 目录对象
                    IList<SecondModule> subModuleList = new List<SecondModule>();
                    List<ListSubModules> listSubModules = new List<ListSubModules>();
                    string SecondModularID = "";
                    for (int i = 0; i < hearResources.Count; i++)
                    {
                        if (i == 0)
                        {
                            SecondModularID = hearResources[i].SecondModularID.ToString();
                            listSubModules.Add(new ListSubModules()
                            {
                                SecondModularID = hearResources[i].SecondModularID.ToString(),
                                ModularName = hearResources[i].ModularName.ToString(),
                                ModularEN = hearResources[i].ModularEN.ToString(),
                                RepeatNumber = hearResources[i].RepeatNumber.ToString(),
                                SerialNumber = hearResources[i].SerialNumber.ToString(),
                                TextSerialNumber = hearResources[i].TextSerialNumber.ToString()
                            });
                        }
                        else
                        {
                            if (SecondModularID != hearResources[i].SecondModularID.ToString())
                            {
                                listSubModules.Add(new ListSubModules()
                                {
                                    SecondModularID = hearResources[i].SecondModularID.ToString(),
                                    ModularName = hearResources[i].ModularName.ToString(),
                                    ModularEN = hearResources[i].ModularEN.ToString(),
                                    RepeatNumber = hearResources[i].RepeatNumber.ToString(),
                                    SerialNumber = hearResources[i].SerialNumber.ToString(),
                                    TextSerialNumber = hearResources[i].TextSerialNumber.ToString()
                                });
                                SecondModularID = hearResources[i].SecondModularID.ToString();
                            }
                        }
                    }
                    listSubModules = listSubModules.Distinct(new ListSubModuleCompare()).ToList();
                    #endregion
                    string sqlString = "";
                    string questString = "";
                    #region 未完成模块目标的图标出现小红点，并显示还剩的未完成的目标次数；用户最优5次的成绩
                    foreach (var model in listSubModules)
                    {
                        #region 未完成模块目标的图标出现小红点，并显示还剩的未完成的目标次数

                        sqlString += "SELECT MAX(PlayTimes) FROM [FZ_HearResources].[dbo].[TB_UserHearResources_YX] where 1=1";
                        sqlString += " and UserID = " + ParseInt(submitData.UserID);
                        sqlString += " and BookID = " + ParseInt(submitData.BookID);
                        sqlString += " and FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                        if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                        {
                            sqlString += " and SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                        }
                        sqlString += " and SerialNumber = " + model.SerialNumber;
                        if (model.TextSerialNumber != null)
                        {
                            sqlString += " and TextSerialNumber = " + model.TextSerialNumber;
                        }

                        sqlString += ";";

                        //DataSet dataSet = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sqlString);//反复连接查询改成拼接sql一次性查询
                        //if (string.IsNullOrEmpty(dataSet.Tables[0].Rows[0][0].ToString()))
                        //{
                        //    secondModule.FinishedTimes = 0;
                        //}
                        //else
                        //{
                        //    secondModule.FinishedTimes = Convert.ToInt32(dataSet.Tables[0].Rows[0][0].ToString());
                        //}
                        #endregion

                        #region 用户最优5次的成绩
                        // questString = "";
                        questString += "SELECT TOP 5  CreateTime,AverageScore FROM [FZ_HearResources].[dbo].[TB_UserHearResources_YX] where 1=1";
                        questString += " and UserID = " + ParseInt(submitData.UserID);
                        questString += " and BookID = " + ParseInt(submitData.BookID);
                        questString += " and FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                        if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                        {
                            questString += " and SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                        }
                        questString += " and SerialNumber = " + model.SerialNumber;
                        if (model.TextSerialNumber != null)
                        {
                            questString += " and TextSerialNumber = " + model.TextSerialNumber;
                        }
                        questString += " ORDER BY AverageScore DESC";
                        questString += ";";

                        //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, questString);//反复连接查询改成拼接sql一次性查询
                        //IList<TB_UserHearResources> resourcesList = DataSetToIList<TB_UserHearResources>(ds, 0);
                        //foreach (var re in resourcesList)
                        //{
                        //    SubModuleDetails subModuleDetails = new SubModuleDetails();
                        //    subModuleDetails.Date = re.CreateTime;
                        //    subModuleDetails.AverageScore = re.AverageScore;
                        //    secondModule.History.Add(subModuleDetails);
                        //}
                        // subModuleList.Add(secondModule);
                        #endregion
                    }
                    DataSet dataSet = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sqlString);//反复连接查询改成拼接sql一次性查询
                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, questString);//反复连接查询改成拼接sql一次性查询
                    for (int i = 0; i < listSubModules.Count; i++)
                    {
                        var model = listSubModules[i];
                        SecondModule secondModule = new SecondModule();
                        secondModule.SecondModuleID = model.SecondModularID;
                        secondModule.SecondModuleName = model.ModularName;
                        secondModule.EnglishName = model.ModularEN;
                        secondModule.RequiredTimes = int.Parse(model.RepeatNumber);
                        secondModule.History = new List<SubModuleDetails>();
                        if (string.IsNullOrEmpty(dataSet.Tables[i].Rows[0][0].ToString()))
                        {
                            secondModule.FinishedTimes = 0;
                        }
                        else
                        {
                            secondModule.FinishedTimes = Convert.ToInt32(dataSet.Tables[i].Rows[0][0].ToString());
                        }

                        IList<TB_UserHearResources> resourcesList = DataSetToIList<TB_UserHearResources>(ds, i);
                        foreach (var re in resourcesList)
                        {
                            SubModuleDetails subModuleDetails = new SubModuleDetails();
                            subModuleDetails.Date = re.CreateTime;
                            subModuleDetails.AverageScore = re.AverageScore;
                            secondModule.History.Add(subModuleDetails);
                        }
                        subModuleList.Add(secondModule);
                    }


                    #endregion
                    var obj = new { SubModules = subModuleList };
                    return ObjectToJson.GetResult(obj);
                }
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取一级模块下，用户在二级模块的得分情况/历史记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetListenSpeakAchievementTest()
        {
            string strJson = "{\"BookID\":\"330\",\"SecondTitleID\":\"\",\"FirstTitleID\":\"294163\",\"FirstModuleID\":\"10\",\"UserID\":\"1629529918\"}";
            string where = "1=1";
            ModuleInfo submitData = JsonHelper.DecodeJson<ModuleInfo>(strJson);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID) || string.IsNullOrEmpty(submitData.BookID) || string.IsNullOrEmpty(submitData.FirstTitleID) || string.IsNullOrEmpty(submitData.FirstModuleID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            try
            {
                var hearResources = redis.Get<List<ListSubModules_0>>("HearResources_" + submitData.BookID, submitData.FirstTitleID + "_" + submitData.FirstModuleID);
                if (hearResources == null)
                {
                    string sql = @"
                    select  a.SecondTitleID,a.SecondModularID,b.ModularName,a.ModularEN,a.RepeatNumber,a.SerialNumber,a.TextSerialNumber
                    from [FZ_HearResources].[dbo].[TB_HearResources_YX] a 
                    left join TB_ModularManage b on  a.FirstModularID=b.SuperiorID and a.SecondModularID=b.ModularID and b.State=1 
                    --left join [FZ_HearResources].[dbo].[TB_UserHearResources_YX] c on a.BookID=c.BookID and a.FirstTitleID=c.FirstTitleID and a.SecondTitleID=c.SecondTitleID 
                    where {0}
                    group by  a.SecondTitleID,a.SecondModularID,b.ModularName,a.ModularEN,a.RepeatNumber,a.SerialNumber,a.TextSerialNumber";
                    where += " and a.BookID = " + ParseInt(submitData.BookID);
                    where += " and a.FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                    where += " and a.FirstModularID = " + ParseInt(submitData.FirstModuleID);

                    sql = string.Format(sql, where);
                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    List<ListSubModules_0> rowList = JsonHelper.DataSetToIList<ListSubModules_0>(ds, 0);
                    redis.Set("HearResources_" + submitData.BookID, submitData.FirstTitleID + "_" + submitData.FirstModuleID, rowList);
                    hearResources = rowList;
                }

                if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                {
                    //  where += " and a.SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                    hearResources = hearResources.Where(o => o.SecondTitleID == int.Parse(submitData.SecondTitleID)).ToList();
                }

                // sql = string.Format(sql, where);
                //     DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql).Tables[0];
                if (hearResources != null && hearResources.Any())
                {
                    #region 目录
                    IList<SecondModule> subModuleList = new List<SecondModule>();
                    List<ListSubModules> listSubModules = new List<ListSubModules>();
                    string SecondModularID = "";
                    for (int i = 0; i < hearResources.Count; i++)
                    {
                        if (i == 0)
                        {
                            SecondModularID = hearResources[i].SecondModularID.ToString();
                            listSubModules.Add(new ListSubModules()
                            {
                                SecondModularID = hearResources[i].SecondModularID.ToString(),
                                ModularName = hearResources[i].ModularName.ToString(),
                                ModularEN = hearResources[i].ModularEN.ToString(),
                                RepeatNumber = hearResources[i].RepeatNumber.ToString(),
                                SerialNumber = hearResources[i].SerialNumber.ToString(),
                                TextSerialNumber = hearResources[i].TextSerialNumber.ToString()
                            });
                        }
                        else
                        {
                            if (SecondModularID != hearResources[i].SecondModularID.ToString())
                            {
                                listSubModules.Add(new ListSubModules()
                                {
                                    SecondModularID = hearResources[i].SecondModularID.ToString(),
                                    ModularName = hearResources[i].ModularName.ToString(),
                                    ModularEN = hearResources[i].ModularEN.ToString(),
                                    RepeatNumber = hearResources[i].RepeatNumber.ToString(),
                                    SerialNumber = hearResources[i].SerialNumber.ToString(),
                                    TextSerialNumber = hearResources[i].TextSerialNumber.ToString()
                                });
                                SecondModularID = hearResources[i].SecondModularID.ToString();
                            }
                        }
                    }
                    listSubModules = listSubModules.Distinct(new ListSubModuleCompare()).ToList();
                    #endregion
                    string sqlString = "";
                    string questString = "";
                    #region 未完成模块目标的图标出现小红点，并显示还剩的未完成的目标次数；用户最优5次的成绩

                    foreach (var model in listSubModules)
                    {
                        #region 未完成模块目标的图标出现小红点，并显示还剩的未完成的目标次数

                        sqlString += "SELECT MAX(PlayTimes) FROM [FZ_HearResources].[dbo].[TB_UserHearResources_YX] where 1=1";
                        sqlString += " and UserID = " + ParseInt(submitData.UserID);
                        sqlString += " and BookID = " + ParseInt(submitData.BookID);
                        sqlString += " and FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                        if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                        {
                            sqlString += " and SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                        }
                        sqlString += " and SerialNumber = " + model.SerialNumber;
                        if (model.TextSerialNumber != null)
                        {
                            sqlString += " and TextSerialNumber = " + model.TextSerialNumber;
                        }

                        sqlString += ";";

                        //DataSet dataSet = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sqlString);//反复连接查询改成拼接sql一次性查询
                        //if (string.IsNullOrEmpty(dataSet.Tables[0].Rows[0][0].ToString()))
                        //{
                        //    secondModule.FinishedTimes = 0;
                        //}
                        //else
                        //{
                        //    secondModule.FinishedTimes = Convert.ToInt32(dataSet.Tables[0].Rows[0][0].ToString());
                        //}
                        #endregion

                        #region 用户最优5次的成绩
                        // questString = "";
                        questString += "SELECT TOP 5  CreateTime,AverageScore FROM [FZ_HearResources].[dbo].[TB_UserHearResources_YX] where 1=1";
                        questString += " and UserID = " + ParseInt(submitData.UserID);
                        questString += " and BookID = " + ParseInt(submitData.BookID);
                        questString += " and FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                        if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                        {
                            questString += " and SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                        }
                        questString += " and SerialNumber = " + model.SerialNumber;
                        if (model.TextSerialNumber != null)
                        {
                            questString += " and TextSerialNumber = " + model.TextSerialNumber;
                        }
                        questString += " ORDER BY AverageScore DESC";
                        questString += ";";

                        //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, questString);//反复连接查询改成拼接sql一次性查询
                        //IList<TB_UserHearResources> resourcesList = DataSetToIList<TB_UserHearResources>(ds, 0);
                        //foreach (var re in resourcesList)
                        //{
                        //    SubModuleDetails subModuleDetails = new SubModuleDetails();
                        //    subModuleDetails.Date = re.CreateTime;
                        //    subModuleDetails.AverageScore = re.AverageScore;
                        //    secondModule.History.Add(subModuleDetails);
                        //}
                        // subModuleList.Add(secondModule);
                        #endregion
                    }
                    DataSet dataSet = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sqlString);//反复连接查询改成拼接sql一次性查询
                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, questString);//反复连接查询改成拼接sql一次性查询
                    for (int i = 0; i < listSubModules.Count; i++)
                    {
                        var model = listSubModules[i];
                        SecondModule secondModule = new SecondModule();
                        secondModule.SecondModuleID = model.SecondModularID;
                        secondModule.SecondModuleName = model.ModularName;
                        secondModule.EnglishName = model.ModularEN;
                        secondModule.RequiredTimes = int.Parse(model.RepeatNumber);
                        secondModule.History = new List<SubModuleDetails>();
                        if (string.IsNullOrEmpty(dataSet.Tables[i].Rows[0][0].ToString()))
                        {
                            secondModule.FinishedTimes = 0;
                        }
                        else
                        {
                            secondModule.FinishedTimes = Convert.ToInt32(dataSet.Tables[i].Rows[0][0].ToString());
                        }

                        IList<TB_UserHearResources> resourcesList = DataSetToIList<TB_UserHearResources>(ds, i);
                        foreach (var re in resourcesList)
                        {
                            SubModuleDetails subModuleDetails = new SubModuleDetails();
                            subModuleDetails.Date = re.CreateTime;
                            subModuleDetails.AverageScore = re.AverageScore;
                            secondModule.History.Add(subModuleDetails);
                        }
                        subModuleList.Add(secondModule);
                    }


                    #endregion
                    var obj = new { SubModules = subModuleList };
                    return ObjectToJson.GetResult(obj);
                }
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取一级模块下，用户在二级模块的得分情况/历史记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetListenSpeakAchievementAgo([FromBody]KingRequest request)
        {
            string where = "1=1";
            ModuleInfo submitData = JsonHelper.DecodeJson<ModuleInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID) || string.IsNullOrEmpty(submitData.BookID) || string.IsNullOrEmpty(submitData.FirstTitleID) || string.IsNullOrEmpty(submitData.FirstModuleID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            try
            {
                where += " and BookID = " + ParseInt(submitData.BookID);
                where += " and FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                {
                    where += " and SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                }
                where += " and FirstModularID = " + ParseInt(submitData.FirstModuleID);
                IList<TB_HearResources_YX> hearResourceslist = hearResourcesBLL.GetModuleList(where);
                if (hearResourceslist != null && hearResourceslist.Count > 0)
                {
                    string secondModuleIDStr = "";
                    for (int i = 0, length = hearResourceslist.Count; i < length; i++)
                    {
                        if (i == 0)
                        {
                            secondModuleIDStr += hearResourceslist[i].SecondModularID;
                        }
                        else
                        {
                            if (secondModuleIDStr.IndexOf("" + hearResourceslist[i].SecondModularID + "") < 0)
                            {
                                secondModuleIDStr += "," + hearResourceslist[i].SecondModularID;
                            }
                        }
                    }
                    string[] secondModuleIDArr = secondModuleIDStr.Split(new char[] { ',' });
                    string queryStr = "";
                    IList<SecondModule> subModuleList = new List<SecondModule>();
                    for (int i = 0, length = secondModuleIDArr.Length; i < length; i++)
                    {
                        queryStr = where;
                        queryStr += " and SecondModularID = " + ParseInt(secondModuleIDArr[i]);
                        IList<TB_HearResources_YX> resourceslist = hearResourcesBLL.GetModuleList(queryStr);
                        string queryString = "";
                        queryString = "1=1";
                        queryString += " and SuperiorID = " + ParseInt(submitData.FirstModuleID);
                        queryString += " and ModularID = " + ParseInt(secondModuleIDArr[i]);
                        queryString += " and State = 1";
                        IList<TB_ModularManage> ModuleList = modularManageBLL.GetModuleList(queryString);
                        SecondModule secondModule = new SecondModule();
                        secondModule.SecondModuleID = secondModuleIDArr[i];
                        secondModule.SecondModuleName = ModuleList[0].ModularName;
                        secondModule.EnglishName = resourceslist[0].ModularEN;
                        secondModule.RequiredTimes = resourceslist[0].RepeatNumber == null ? 0 : resourceslist[0].RepeatNumber;
                        secondModule.History = new List<SubModuleDetails>();
                        string questString = "";
                        questString = "SELECT TOP 5 * FROM [FZ_HearResources].[dbo].[TB_UserHearResources_YX] where 1=1";
                        questString += " and UserID = " + ParseInt(submitData.UserID);
                        questString += " and BookID = " + ParseInt(submitData.BookID);
                        questString += " and FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                        if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                        {
                            questString += " and SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                        }
                        questString += " and SerialNumber = " + resourceslist[0].SerialNumber;
                        if (resourceslist[0].TextSerialNumber != null)
                        {
                            questString += " and TextSerialNumber = " + resourceslist[0].TextSerialNumber;
                        }
                        questString += " ORDER BY AverageScore DESC";
                        DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, questString);
                        IList<TB_UserHearResources_YX> resourcesList = DataSetToIList<TB_UserHearResources_YX>(ds, 0);
                        string sqlString = "SELECT MAX(PlayTimes) FROM [FZ_HearResources].[dbo].[TB_UserHearResources_YX] where 1=1";
                        sqlString += " and UserID = " + ParseInt(submitData.UserID);
                        sqlString += " and BookID = " + ParseInt(submitData.BookID);
                        sqlString += " and FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                        if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                        {
                            sqlString += " and SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                        }
                        sqlString += " and SerialNumber = " + resourceslist[0].SerialNumber;
                        if (resourceslist[0].TextSerialNumber != null)
                        {
                            sqlString += " and TextSerialNumber = " + resourceslist[0].TextSerialNumber;
                        }
                        DataSet dataSet = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sqlString);
                        if (string.IsNullOrEmpty(dataSet.Tables[0].Rows[0][0].ToString()))
                        {
                            secondModule.FinishedTimes = 0;
                        }
                        else
                        {
                            secondModule.FinishedTimes = Convert.ToInt32(dataSet.Tables[0].Rows[0][0].ToString());
                        }
                        for (int j = 0, len = resourcesList.Count; j < len; j++)
                        {
                            SubModuleDetails subModuleDetails = new SubModuleDetails();
                            subModuleDetails.Date = resourcesList[j].CreateTime;
                            subModuleDetails.AverageScore = resourcesList[j].AverageScore == null ? 0 : resourcesList[j].AverageScore;
                            secondModule.History.Add(subModuleDetails);
                        }
                        subModuleList.Add(secondModule);
                    }
                    var obj = new { SubModules = subModuleList };
                    return ObjectToJson.GetResult(obj);
                }
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 把文件服务器从临时变成永久的
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ConfirmFileID([FromBody]KingRequest request)
        {
            try
            {
                FileInfo submitData = JsonHelper.DecodeJson<FileInfo>(request.Data);
                if (submitData == null)
                {
                    return ObjectToJson.GetErrorResult("当前信息为空");
                }

                string field = FiedURL + "ConfirmHandler.ashx";
                //get请求
                string dd = JsonHelper.EncodeJson(submitData.AudioFileID);
                string values = "t=[" + dd + "]";
                HttpGet(field, values);

                return ObjectToJson.GetResult(null);
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 根据 BookID、FirstTitleID、SecondTitleID、FirstModuleID、SecondModuleID 获取模块下听说信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetListenSpeakListAgo([FromBody]KingRequest request)
        {
            string where = "1=1";
            ModuleInfo submitData = JsonHelper.DecodeJson<ModuleInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.BookID) || string.IsNullOrEmpty(submitData.FirstTitleID) || string.IsNullOrEmpty(submitData.FirstModuleID) || string.IsNullOrEmpty(submitData.SecondModuleID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            try
            {
                where += " and BookID = '" + submitData.BookID + "'";
                where += " and FirstTitleID = '" + (submitData.FirstTitleID) + "'";
                if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                {
                    where += " and SecondTitleID = '" + (submitData.SecondTitleID) + "'";
                }
                where += " and FirstModularID = '" + (submitData.FirstModuleID) + "'";
                where += " and SecondModularID = '" + (submitData.SecondModuleID) + "'";
                IList<TB_HearResources_YX> hr = bm.Search<TB_HearResources_YX>(where);
                if (hr != null && hr.Count > 0)
                {
                    int? repeatNum = null;
                    int finishedTimes = 0;
                    IList<ListenSpeakInfo> contentList = new List<ListenSpeakInfo>();
                    foreach (var item in hr)
                    {
                        if (item.RepeatNumber != null)
                        {
                            repeatNum = item.RepeatNumber;
                        }
                        ListenSpeakInfo listenSpeakInfo = new ListenSpeakInfo
                        {
                            Content = item.TextDesc,
                            Audio = StringIsNullOrEmpty(item.AudioFrequency),
                            Image = StringIsNullOrEmpty(item.Picture),
                            SerialNumber = item.SerialNumber,
                            TextSerialNumber = item.TextSerialNumber,
                            AdditionInfo = StringIsNullOrEmpty(item.RoleName)
                        };
                        contentList.Add(listenSpeakInfo);
                    }
                    string questString = "1=1";
                    int? textSerialNumber = hr[0].TextSerialNumber;
                    questString += " and UserID = '" + (submitData.UserID) + "'";
                    questString += " and BookID = '" + (submitData.BookID) + "'";
                    questString += " and FirstTitleID = '" + (submitData.FirstTitleID) + "'";
                    if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                    {
                        questString += " and SecondTitleID = '" + (submitData.SecondTitleID) + "'";
                    }
                    questString += " and SerialNumber = " + hr[0].SerialNumber;
                    if (textSerialNumber != null)
                    {
                        questString += " and TextSerialNumber = " + hr[0].TextSerialNumber;
                    }
                    questString += " ORDER BY PlayTimes DESC ";
                    IList<TB_UserHearResources_YX> resourcesList = bm.Search<TB_UserHearResources_YX>(questString);
                    if (resourcesList != null && resourcesList.Count > 0)
                    {
                        finishedTimes = Convert.ToInt32(resourcesList[0].PlayTimes);
                    }
                    var obj = new { RequiredTimes = repeatNum, FinishedTimes = finishedTimes, List = contentList };
                    return ObjectToJson.GetResult(obj);
                }
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 根据 BookID、FirstTitleID、SecondTitleID、FirstModuleID、SecondModuleID 获取模块下听说信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetListenSpeakList([FromBody]KingRequest request)
        {
            string where = "1=1";
            ModuleInfo submitData = JsonHelper.DecodeJson<ModuleInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.BookID) || string.IsNullOrEmpty(submitData.FirstTitleID) || string.IsNullOrEmpty(submitData.FirstModuleID) || string.IsNullOrEmpty(submitData.SecondModuleID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            try
            {
                where += " and BookID = '" + submitData.BookID + "'";
                where += " and FirstTitleID = '" + (submitData.FirstTitleID) + "'";
                if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                {
                    where += " and SecondTitleID = '" + (submitData.SecondTitleID) + "'";
                }
                where += " and FirstModularID = '" + (submitData.FirstModuleID) + "'";
                where += " and SecondModularID = '" + (submitData.SecondModuleID) + "'";
                IList<TB_HearResources_YX> hr = bm.Search<TB_HearResources_YX>(where);
                if (hr != null && hr.Count > 0)
                {
                    int? repeatNum = null;
                    int finishedTimes = 0;
                    IList<ListenSpeakInfo> contentList = new List<ListenSpeakInfo>();
                    foreach (var item in hr)
                    {
                        if (item.RepeatNumber != null)
                        {
                            repeatNum = item.RepeatNumber;
                        }
                        ListenSpeakInfo listenSpeakInfo = new ListenSpeakInfo
                        {
                            Content = item.TextDesc,
                            Audio = StringIsNullOrEmpty(item.AudioFrequency),
                            Image = StringIsNullOrEmpty(item.Picture),
                            SerialNumber = item.SerialNumber,
                            TextSerialNumber = item.TextSerialNumber,
                            AdditionInfo = StringIsNullOrEmpty(item.RoleName)
                        };
                        contentList.Add(listenSpeakInfo);
                    }
                    string questString = " 1=1";
                    int? textSerialNumber = hr[0].TextSerialNumber;
                    questString += " and UserID = '" + (submitData.UserID) + "'";
                    questString += " and BookID = '" + (submitData.BookID) + "'";
                    questString += " and FirstTitleID = '" + (submitData.FirstTitleID) + "'";
                    if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                    {
                        questString += " and SecondTitleID = '" + (submitData.SecondTitleID) + "'";
                    }
                    questString += " and SerialNumber = " + hr[0].SerialNumber;
                    if (textSerialNumber != null)
                    {
                        questString += " and TextSerialNumber = " + hr[0].TextSerialNumber;
                    }
                    //questString += " ORDER BY PlayTimes DESC ";
                    string sql = "select ISNULL(max(PlayTimes),0) from [FZ_HearResources].[dbo].[TB_UserHearResources_YX] where " + questString;
                    finishedTimes = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql));
                    //IList<TB_UserHearResources> resourcesList = bm.Search<TB_UserHearResources>(questString);
                    //if (resourcesList != null && resourcesList.Count > 0)
                    //{
                    //    finishedTimes = Convert.ToInt32(resourcesList[0].PlayTimes);
                    //}
                    var obj = new { RequiredTimes = repeatNum, FinishedTimes = finishedTimes, List = contentList };
                    return ObjectToJson.GetResult(obj);
                }
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 根据 BookID、FirstTitleID、SecondTitleID、FirstModuleID、SecondModuleID 获取模块下听说信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetListenSpeakListTest()
        {
            string where = "1=1";
            string data =
                "{\"UserID\":\"99659497\",\"BookID\":\"168\",\"SecondModuleID\":\"1001\",\"FirstTitleID\":\"276194\",\"IsEnableOss\":1,\"FirstModuleID\":\"10\",\"SecondTitleID\":\"276201\"}";
            ModuleInfo submitData = JsonHelper.DecodeJson<ModuleInfo>(data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.BookID) || string.IsNullOrEmpty(submitData.FirstTitleID) || string.IsNullOrEmpty(submitData.FirstModuleID) || string.IsNullOrEmpty(submitData.SecondModuleID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            try
            {
                where += " and BookID = '" + submitData.BookID + "'";
                where += " and FirstTitleID = '" + (submitData.FirstTitleID) + "'";
                if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                {
                    where += " and SecondTitleID = '" + (submitData.SecondTitleID) + "'";
                }
                where += " and FirstModularID = '" + (submitData.FirstModuleID) + "'";
                where += " and SecondModularID = '" + (submitData.SecondModuleID) + "'";
                IList<TB_HearResources_YX> hr = bm.Search<TB_HearResources_YX>(where);
                if (hr != null && hr.Count > 0)
                {
                    int? repeatNum = null;
                    int finishedTimes = 0;
                    IList<ListenSpeakInfo> contentList = new List<ListenSpeakInfo>();
                    foreach (var item in hr)
                    {
                        if (item.RepeatNumber != null)
                        {
                            repeatNum = item.RepeatNumber;
                        }
                        ListenSpeakInfo listenSpeakInfo = new ListenSpeakInfo
                        {
                            Content = item.TextDesc,
                            Audio = StringIsNullOrEmpty(item.AudioFrequency),
                            Image = StringIsNullOrEmpty(item.Picture),
                            SerialNumber = item.SerialNumber,
                            TextSerialNumber = item.TextSerialNumber,
                            AdditionInfo = StringIsNullOrEmpty(item.RoleName)
                        };
                        contentList.Add(listenSpeakInfo);
                    }
                    string questString = " 1=1";
                    int? textSerialNumber = hr[0].TextSerialNumber;
                    questString += " and UserID = '" + (submitData.UserID) + "'";
                    questString += " and BookID = '" + (submitData.BookID) + "'";
                    questString += " and FirstTitleID = '" + (submitData.FirstTitleID) + "'";
                    if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                    {
                        questString += " and SecondTitleID = '" + (submitData.SecondTitleID) + "'";
                    }
                    questString += " and SerialNumber = " + hr[0].SerialNumber;
                    if (textSerialNumber != null)
                    {
                        questString += " and TextSerialNumber = " + hr[0].TextSerialNumber;
                    }
                    //questString += " ORDER BY PlayTimes DESC ";
                    string sql = "select ISNULL(max(PlayTimes),0) from [FZ_HearResources].[dbo].[TB_UserHearResources_YX] where " + questString;
                    finishedTimes = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql));
                    //IList<TB_UserHearResources> resourcesList = bm.Search<TB_UserHearResources>(questString);
                    //if (resourcesList != null && resourcesList.Count > 0)
                    //{
                    //    finishedTimes = Convert.ToInt32(resourcesList[0].PlayTimes);
                    //}
                    var obj = new { RequiredTimes = repeatNum, FinishedTimes = finishedTimes, List = contentList };
                    return ObjectToJson.GetResult(obj);
                }
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 提交用户的跟读情况
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SummitOralMarks([FromBody]KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ModuleInfo submitData = js.Deserialize<ModuleInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID) || string.IsNullOrEmpty(submitData.BookID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            if (submitData.Achievement == null)
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            int ModelType = 6;
            //Channel:渠道编号（303：同步学人教PEP，301：人教优学）
            //ModelType：redis模块类别（1:趣配音,2:单元测试，3：说说看，4:优学趣配音,5:优学单元测试，6：优学说说看）
            if (!string.IsNullOrEmpty(submitData.Channel.ToString()))
            {
                ModelType = 6;
            }
            else if (submitData.Channel == 301)
            {
                ModelType = 6;
            }
            try
            {
                bool flag = true, sign = false;
                int playTimes = 1, accidentNum = 0;
                string where = "1=1";
                double averageScore = 0;
                double sumScore = 0;
                int? SerialNumber = 0;
                for (int i = 0, length = submitData.Achievement.Count; i < length; i++)
                {
                    if (submitData.Achievement[i].Score > 100)
                    {
                        return ObjectToJson.GetErrorResult("分数有误");
                    }
                    if (submitData.Achievement[i].Score >= 0)
                    {
                        sumScore += submitData.Achievement[i].Score;
                    }
                    else
                    {
                        accidentNum += 1;
                    }
                }
                averageScore = sumScore / (submitData.Achievement.Count - accidentNum);
                for (int i = 0, length = submitData.Achievement.Count; i < length; i++)
                {
                    if (!sign)
                    {
                        where += " and UserID = " + ParseInt(submitData.UserID);
                        where += " and BookID = " + ParseInt(submitData.BookID);
                        where += " and FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                        if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                        {
                            where += " and SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                        }
                        where += " and SerialNumber = " + ParseInt(submitData.Achievement[i].SerialNumber);
                        where += " and TextSerialNumber = " + ParseInt(submitData.Achievement[i].TextSerialNumber);
                        IList<TB_UserHearResources_YX> resourcesList = hearResourcesBLL.GetUserHearResourcesList(where);
                        if (resourcesList != null && resourcesList.Count > 0)
                        {
                            playTimes = resourcesList.Count + 1;
                        }
                        sign = true;
                    }
                    TB_UserHearResources_YX resourcesInfo = new TB_UserHearResources_YX();
                    resourcesInfo.UserID = ParseInt(submitData.UserID);
                    resourcesInfo.BookID = ParseInt(submitData.BookID);
                    resourcesInfo.FirstTitleID = ParseInt(submitData.FirstTitleID);
                    resourcesInfo.SecondTitleID = ParseInt(submitData.SecondTitleID);
                    resourcesInfo.SerialNumber = ParseInt(submitData.Achievement[i].SerialNumber);
                    resourcesInfo.TextSerialNumber = ParseInt(submitData.Achievement[i].TextSerialNumber);
                    resourcesInfo.TotalScore = submitData.Achievement[i].Score;
                    resourcesInfo.VideoFileID = submitData.Achievement[i].AudioFileID;
                    resourcesInfo.PlayTimes = playTimes;
                    resourcesInfo.AverageScore = averageScore;
                    resourcesInfo.State = 1;
                    resourcesInfo.IsEnableOss = submitData.IsEnableOss;
                    resourcesInfo.CreateTime = DateTime.Now;

                    bool insertResult = hearResourcesBLL.AddResources(resourcesInfo);
                    if (!insertResult)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (SerialNumber != submitData.Achievement[i].SerialNumber)
                        {
                            SerialNumber = submitData.Achievement[i].SerialNumber;
                            if (submitData.SecondTitleID != "0" || string.IsNullOrEmpty(submitData.SecondTitleID))
                            {
                                where = string.Format(" AND a.FirstTitleID = '{0}'  AND a.SecondTitleID = '{1}' ", submitData.FirstTitleID, submitData.SecondTitleID);
                            }
                            else
                            {
                                where = string.Format(" AND a.FirstTitleID = '{0}' ", submitData.FirstTitleID);
                            }
                            string sql = string.Format(@"SELECT  a.BookID ,
                                                                b.FirstTitileID ,
                                                                b.FirstTitle ,
                                                                b.SecondTitleID ,
                                                                b.SecondTitle ,
                                                                a.SecondModularID ,
                                                                c.ModularName,
                                                                a.FirstModularID,
                                                                a.SecondModularID
                                                        FROM    FZ_HearResources.dbo.TB_HearResources_YX a
                                                                LEFT JOIN dbo.TB_ModuleConfiguration b ON b.BookID = a.BookID
                                                                LEFT JOIN dbo.TB_ModularManage c ON c.ModularID = a.SecondModularID
                                                        WHERE   b.State = 0
                                                                AND a.BookID = '{0}'
                                                                AND a.SerialNumber = {1}  {2}", submitData.BookID, SerialNumber, where);

                            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                            List<VideoDetails> vdl = JsonHelper.DataSetToIList<VideoDetails>(ds, 0);
                            VideoDetails video = vdl.FirstOrDefault();
                            if (video != null)
                            {
                                RedisVideoInfo rvi = new RedisVideoInfo
                                {
                                    BookId = submitData.BookID,
                                    UserId = submitData.UserID,
                                    FirstTitleID = submitData.FirstTitleID,
                                    FirstTitle = video.FirstTitle,
                                    SecondTitleID = submitData.SecondTitleID,
                                    SecondTitle = video.SecondTitle,
                                    VideoNumber = SerialNumber.ToString(),
                                    TotalScore = averageScore.ToString(),
                                    CreateTime = DateTime.Now.ToString(),
                                    VideoTitle = video.ModularName,
                                    ModuleType = ModelType.ToString(),
                                    FirstModularID = video.FirstModularID.ToString(),
                                    SecondModularID = video.SecondModularID.ToString()
                                };

                                //说说看消息队列
                                redisList.LPush("LearningReportQueue", JsonHelper.DeepEncodeJson(rvi));
                            }
                        }
                    }
                }
                if (flag)
                {
                    return ObjectToJson.GetResult(null, "信息保存成功");
                }
                else
                {
                    return ObjectToJson.GetErrorResult("信息插入异常");
                }
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 提交用户的跟读情况
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SummitOralMarksTest()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ModuleInfo submitData = new ModuleInfo();
            submitData.UserID = "454582054";
            submitData.BookID = "168";
            submitData.Achievement = new List<ListenSpeakInfo>()
            {

            };

            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID) || string.IsNullOrEmpty(submitData.BookID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            if (submitData.Achievement == null)
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            try
            {
                bool flag = true, sign = false;
                int playTimes = 1, accidentNum = 0;
                string where = "1=1";
                double averageScore = 0;
                double sumScore = 0;
                int? SerialNumber = 0;
                for (int i = 0, length = submitData.Achievement.Count; i < length; i++)
                {
                    if (submitData.Achievement[i].Score > 100)
                    {
                        return ObjectToJson.GetErrorResult("分数有误");
                    }
                    if (submitData.Achievement[i].Score >= 0)
                    {
                        sumScore += submitData.Achievement[i].Score;
                    }
                    else
                    {
                        accidentNum += 1;
                    }
                }
                averageScore = sumScore / (submitData.Achievement.Count - accidentNum);
                for (int i = 0, length = submitData.Achievement.Count; i < length; i++)
                {
                    if (!sign)
                    {
                        where += " and UserID = " + ParseInt(submitData.UserID);
                        where += " and BookID = " + ParseInt(submitData.BookID);
                        where += " and FirstTitleID = " + ParseInt(submitData.FirstTitleID);
                        if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                        {
                            where += " and SecondTitleID = " + ParseInt(submitData.SecondTitleID);
                        }
                        where += " and SerialNumber = " + ParseInt(submitData.Achievement[i].SerialNumber);
                        where += " and TextSerialNumber = " + ParseInt(submitData.Achievement[i].TextSerialNumber);
                        IList<TB_UserHearResources_YX> resourcesList = hearResourcesBLL.GetUserHearResourcesList(where);
                        if (resourcesList != null && resourcesList.Count > 0)
                        {
                            playTimes = resourcesList.Count + 1;
                        }
                        sign = true;
                    }
                    TB_UserHearResources_YX resourcesInfo = new TB_UserHearResources_YX();
                    resourcesInfo.UserID = ParseInt(submitData.UserID);
                    resourcesInfo.BookID = ParseInt(submitData.BookID);
                    resourcesInfo.FirstTitleID = ParseInt(submitData.FirstTitleID);
                    resourcesInfo.SecondTitleID = ParseInt(submitData.SecondTitleID);
                    resourcesInfo.SerialNumber = ParseInt(submitData.Achievement[i].SerialNumber);
                    resourcesInfo.TextSerialNumber = ParseInt(submitData.Achievement[i].TextSerialNumber);
                    resourcesInfo.TotalScore = submitData.Achievement[i].Score;
                    resourcesInfo.VideoFileID = submitData.Achievement[i].AudioFileID;
                    resourcesInfo.PlayTimes = playTimes;
                    resourcesInfo.AverageScore = averageScore;
                    resourcesInfo.State = 1;
                    resourcesInfo.IsEnableOss = submitData.IsEnableOss;
                    resourcesInfo.CreateTime = DateTime.Now;

                    bool insertResult = hearResourcesBLL.AddResources(resourcesInfo);
                    if (!insertResult)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (SerialNumber != submitData.Achievement[i].SerialNumber)
                        {
                            SerialNumber = submitData.Achievement[i].SerialNumber;
                            RedisVideoInfo rvi = new RedisVideoInfo
                            {
                                BookId = submitData.BookID,
                                UserId = submitData.UserID,
                                FirstTitleID = submitData.FirstTitleID,
                                SecondTitleID = submitData.SecondTitleID,
                                VideoNumber = SerialNumber.ToString(),
                                TotalScore = averageScore.ToString(),
                                CreateTime = DateTime.Now.ToString(),
                                ModuleType = "6"
                            };

                            //说说看消息队列
                            redisList.LPush("LearningReportQueue", JsonHelper.DeepEncodeJson(rvi));
                        }
                    }
                }
                if (flag)
                {
                    return ObjectToJson.GetResult(null, "信息保存成功");
                }
                else
                {
                    return ObjectToJson.GetErrorResult("信息插入异常");
                }
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 根据BookID获取对应的资源路径
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetBookResources([FromBody] KingRequest request)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            VW_Bookesource submitData = JsonHelper.DecodeJson<VW_Bookesource>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.BookID <= 0)
            {
                return ObjectToJson.GetErrorResult("书籍ID不能为空");
            }
            string sql = "SELECT ResourceUrl FROM [FZ_HearResources].[dbo].[TB_BookResource_YX] WHERE BookID='" + submitData.BookID + "'";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss = "{\"ResourceUrl\":\"" + SetSuishengting + ds.Tables[0].Rows[0]["ResourceUrl"] + "\"}";
                return ObjectToJson.GetResult(js.DeserializeObject(ss), "");
            }
            return ObjectToJson.GetErrorResult("没有更多数据");
        }

        /// <summary>
        /// 根据BookID获取对应的资源路径
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetBookResourcesTest()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            TB_BookResource submitData = new TB_BookResource();// JsonHelper.DecodeJson<TB_BookResource>(request.data);
            submitData.BookID = 168;
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.BookID <= 0)
            {
                return ObjectToJson.GetErrorResult("书籍ID不能为空");
            }
            string sql = "SELECT ResourceUrl FROM [FZ_HearResources].[dbo].[TB_BookResource] WHERE BookID='" + submitData.BookID + "'";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss = "{\"ResourceUrl\":\"" + SetSuishengting + ds.Tables[0].Rows[0]["ResourceUrl"] + "\"}";
                return ObjectToJson.GetResult(js.DeserializeObject(ss), "");
            }
            return ObjectToJson.GetErrorResult("没有更多数据");
        }

        /// <summary>
        /// 转换int型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ParseInt(object obj)
        {
            int reInt = 0;
            if (obj != null)
                int.TryParse(obj.ToString(), out reInt);
            return reInt;
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string StringIsNullOrEmpty(string str)
        {
            string result = "";
            if (string.IsNullOrEmpty(str))
            {
                return result;
            }
            return str;
        }

        /// <summary>
        /// get 请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
    }

    

    
}
