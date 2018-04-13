using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.Framework.Contract.Enums;
using CBSS.IBS.Contract;
using CBSS.IBS.Contract.IBSResource;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.API.Controllers
{
    /// <summary>
    /// 说说看记录
    /// </summary>
    public partial class UserRecordController
    {
        [HttpPost]
        public static APIResponse GetHearResourceRecord()
        {
            return APIResponse.GetResponse("");
        }

        [HttpPost]
        public static APIResponse GetListenSpeakAchievement(string inputStr)
        {
            var param = inputStr.ToObject<v_ModuleInfo>();
            if (param == null)
            {
                return APIResponse.GetErrorResponse("当前信息为空");
            }
            if (string.IsNullOrEmpty(param.UserID) || string.IsNullOrEmpty(param.BookID) || string.IsNullOrEmpty(param.FirstTitleID) || string.IsNullOrEmpty(param.FirstModularID))
            {
                return APIResponse.GetErrorResponse("当前信息有误");
            }
            try
            {

                EnglishResourceModel model = new EnglishResourceModel();
                int FirstModuleID = param.FirstModularID.ToInt();
                var module = tbxService.GetModuleList(o => o.ModuleID == FirstModuleID).FirstOrDefault();
                var mo = tbxService.GetModel(module.ModelID);
                model.bookId = param.BookID.ToInt();
                model.SecondTitleID = param.SecondTitleID;
                model.FirstTitleID = param.FirstTitleID.ToInt();
                model.moduleID = param.FirstModularID.ToInt();
                model.type = (int)MODSourceTypeEnum.HearResource;
                var hearResources=redis.Get<List<v_ListSubModules_0>>("HearResources_" + param.BookID, param.FirstTitleID + "_" + mo.OldModelID);
                if (hearResources == null)
                {
                    hearResources = new List<v_ListSubModules_0>();
                    var respon = tbxService.GetModResource(model);
                    var data = respon.Data as List<v_HearResources>;
                    data.ForEach(a =>
                    {
                        v_ListSubModules_0 item = new v_ListSubModules_0();
                        item.SecondModularID = a.SecondModularID;
                        item.ModularName = EnumHelper.GetEnumDesc<SecondModularEnum>(a.SecondModularID);
                        item.ModularEN = a.ModularEN;
                        item.RepeatNumber = a.RepeatNumber;
                        item.SecondTitleID = a.SecondTitleID;
                        item.SerialNumber = a.SerialNumber;
                        item.TextSerialNumber = a.TextSerialNumber;
                        hearResources.Add(item);
                    });
                    redis.Set("HearResources_" + param.BookID, param.FirstTitleID + "_" +mo.OldModelID, hearResources);
                   
                }

                if (!string.IsNullOrEmpty(param.SecondTitleID))
                {
                    hearResources = hearResources.Where(o => o.SecondTitleID == int.Parse(param.SecondTitleID)).ToList();

                }
                // sql = string.Format(sql, where);
                //     DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql).Tables[0];
                if (hearResources != null && hearResources.Any())
                {
                    List<v_SecondModule> subModuleList=tbxService.GetHearResource(param, hearResources).ToList();
                     var obj = new { SubModules = subModuleList };
                    return APIResponse.GetResponse(obj);
                }
                return APIResponse.GetErrorResponse("没有更多数据");
            } catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ex.Message);
            }
        }
        [HttpPost]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserRecordController.SummitOralMarks(string)”的 XML 注释
        public static APIResponse SummitOralMarks(string inputStr)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserRecordController.SummitOralMarks(string)”的 XML 注释
        {
            var submitData = inputStr.ToObject<v_ModuleInfo>();
            if (submitData == null)
            {
                return APIResponse.GetErrorResponse("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID) || string.IsNullOrEmpty(submitData.BookID))
            {
                return APIResponse.GetErrorResponse("当前信息有误");
            }
            if (submitData.Achievement == null)
            {
                return APIResponse.GetErrorResponse("当前信息有误");
            }
            int ModelType = 3;
            //Channel:渠道编号（303：同步学人教PEP，301：人教优学）
            //ModelType：redis模块类别（1:趣配音,2:单元测试，3：说说看，4:优学趣配音,5:优学单元测试，6：优学说说看）
            if (!string.IsNullOrEmpty(submitData.Channel.ToString()))
            {
                ModelType = 3;
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
                        return APIResponse.GetErrorResponse("分数有误");
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
                        where += " and UserID = " + submitData.UserID.ToInt();
                        where += " and BookID = " + submitData.BookID.ToInt();
                        where += " and FirstTitleID = " + submitData.FirstTitleID.ToInt();
                        if (!string.IsNullOrEmpty(submitData.SecondTitleID))
                        {
                            where += " and SecondTitleID = " +submitData.SecondTitleID.ToInt();
                        }
                        where += " and SerialNumber = " +submitData.Achievement[i].SerialNumber;
                        where += " and TextSerialNumber = " + submitData.Achievement[i].TextSerialNumber;
                        IList<TB_HearResources> resourcesList = tbxService.GetUserHearResourcesList(where).ToList();
                        if (resourcesList != null && resourcesList.Count > 0)
                        {
                            playTimes = resourcesList.Count + 1;
                        }
                        sign = true;
                    }
                    TB_HearResources resourcesInfo = new TB_HearResources();
                    resourcesInfo.UserID = submitData.UserID.ToInt();
                    resourcesInfo.BookID = submitData.BookID.ToInt();
                    resourcesInfo.FirstTitleID = submitData.FirstTitleID.ToInt();
                    resourcesInfo.SecondTitleID =submitData.SecondTitleID.ToInt();
                    resourcesInfo.SerialNumber =(int)submitData.Achievement[i].SerialNumber;
                    resourcesInfo.TextSerialNumber = (int)submitData.Achievement[i].TextSerialNumber;
                    resourcesInfo.TotalScore = submitData.Achievement[i].Score;
                    resourcesInfo.VideoFileID = submitData.Achievement[i].AudioFileID;
                    resourcesInfo.PlayTimes = playTimes;
                    resourcesInfo.AverageScore = averageScore;
                    resourcesInfo.State = 1;
                    resourcesInfo.IsEnableOss = submitData.IsEnableOss;
                    resourcesInfo.CreateTime = DateTime.Now;

                    bool insertResult = tbxService.AddResources(resourcesInfo);
                    if (!insertResult)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (SerialNumber != submitData.Achievement[i].SerialNumber)
                        {
                            SerialNumber = submitData.Achievement[i].SerialNumber;
                            EnglishResourceModel model = new EnglishResourceModel();
                            model.moduleID = submitData.FirstModularID.ToInt();
                            model.bookId = submitData.BookID.ToInt();
                            model.catalogueId = submitData.FirstTitleID.ToInt();
                            model.SecondTitleID = submitData.SecondTitleID;
                            model.FirstTitleID = submitData.FirstTitleID.ToInt();
                            model.FirstModularID = submitData.FirstModularID.ToInt();
                            model.SecondModularID = submitData.SecondModularID.ToInt();
                            var respon = tbxService.GetHearResource(model);
                            List<v_ListSubModules_0> list = new List<v_ListSubModules_0>();
                           
                            if (respon.Success)
                            {
                                var data = respon.Data as List<v_HearResources>;
                                data.ForEach(a =>
                                {
                                    v_ListSubModules_0 item = new v_ListSubModules_0();
                                    item.SecondModularID = a.SecondModularID;
                                    item.ModularName = EnumHelper.GetEnumDesc<SecondModularEnum>(a.SecondModularID);
                                    item.ModularEN = a.ModularEN;
                                    item.RepeatNumber =a.RepeatNumber;
                                    item.SecondTitleID = a.SecondTitleID;
                                    item.SerialNumber = a.SerialNumber;
                                    item.TextSerialNumber = a.TextSerialNumber;
                                    list.Add(item);
                                });
                            }
                            else
                            {
                                return APIResponse.GetErrorResponse("暂无资源！");
                            }

                            
                            if (!string.IsNullOrEmpty(submitData.SecondTitleID) && submitData.SecondTitleID != "0")
                            {
                                list = list.Where(a => a.SecondTitleID == submitData.SecondTitleID.ToInt()).ToList();
                            }
                            list = list.Where(a => a.SerialNumber == SerialNumber).ToList();

                            v_ListSubModules_0 video = list.FirstOrDefault();
                            if (video != null)
                            {
                                v_RedisVideoInfo rvi = new v_RedisVideoInfo
                                {
                                    BookId = submitData.BookID,
                                    UserId = submitData.UserID,
                                    FirstTitleID = submitData.FirstTitleID,
                                    SecondTitleID = submitData.SecondTitleID,
                                    VideoNumber = SerialNumber.ToString(),
                                    TotalScore = averageScore.ToString(),
                                    CreateTime = DateTime.Now.ToString(),
                                    VideoTitle = video.ModularName,
                                    ModuleType = ModelType.ToString(),
                                    FirstModularID = submitData.FirstModularID,
                                    SecondModularID = video.SecondModularID.ToString()
                                };

                                //说说看消息队列
                                redisList.LPush("LearningReportQueue",rvi.ToJson());
                            }
                        }
                    }
                }
                if (flag)
                {
                    return APIResponse.GetResponse("信息保存成功");
                }
                else
                {
                    return APIResponse.GetErrorResponse("信息插入异常");
                }
            }
            catch (Exception ex)
            {
                return  APIResponse.GetErrorResponse(ex.Message);
            }
        }

#pragma warning disable CS1572 // XML 注释中有“request”的 param 标记，但是没有该名称的参数
        /// <summary>
        /// 根据 BookID、FirstTitleID、SecondTitleID、FirstModuleID、SecondModuleID 获取模块下听说信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
#pragma warning restore CS1572 // XML 注释中有“request”的 param 标记，但是没有该名称的参数
#pragma warning disable CS1573 // 参数“inputStr”在“UserRecordController.GetListenSpeakList(string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        public static APIResponse GetListenSpeakList(string inputStr)
#pragma warning restore CS1573 // 参数“inputStr”在“UserRecordController.GetListenSpeakList(string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            var submitData = inputStr.ToObject<v_ModuleInfo>();
            if (submitData == null)
            {
                return APIResponse.GetErrorResponse("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.BookID) || string.IsNullOrEmpty(submitData.FirstTitleID) || string.IsNullOrEmpty(submitData.FirstModularID) || string.IsNullOrEmpty(submitData.SecondModularID))
            {
                return APIResponse.GetErrorResponse("当前信息有误");
            }
            try
            {
                EnglishResourceModel model = new EnglishResourceModel();
                model.bookId = submitData.BookID.ToInt();
                model.FirstTitleID =submitData.FirstTitleID.ToInt();
                model.SecondTitleID = submitData.SecondTitleID;
                model.FirstModularID = submitData.FirstModularID.ToInt();
                model.SecondModularID = submitData.SecondModularID.ToInt();
                model.moduleID= submitData.FirstModularID.ToInt();
                var respon = tbxService.GetHearResource(model);
                List<v_ListenSpeakInfo> hr = new List<v_ListenSpeakInfo>();

                var data = respon.Data as List<v_HearResources>;
                data.ForEach(a =>
                {
                    v_ListenSpeakInfo item = new v_ListenSpeakInfo();
                    item.Content = a.TextDesc;
                    item.Audio = a.AudioFrequency;
                    item.Image = a.Picture;
                    item.SerialNumber = a.SerialNumber;
                    item.AdditionInfo = a.RoleName;
                    item.TextSerialNumber =a.TextSerialNumber;
                    hr.Add(item);
                });
                if (hr != null && hr.Count > 0)
                {
                    int repeatNum = 3;
                    int finishedTimes;
                    tbxService.GetUserHearResourcesInfo(submitData, hr, out finishedTimes);
                    //IList<TB_UserHearResources> resourcesList = bm.Search<TB_UserHearResources>(questString);
                    //if (resourcesList != null && resourcesList.Count > 0)
                    //{
                    //    finishedTimes = Convert.ToInt32(resourcesList[0].PlayTimes);
                    //}
                    var obj = new { RequiredTimes = repeatNum, FinishedTimes = finishedTimes, List = hr };
                    return APIResponse.GetResponse(obj);
                }
                return APIResponse.GetErrorResponse("没有更多数据");
            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ex.Message);
            }
        }

        [HttpPost]
        public static APIResponse GetListenSpeakAchievementByWeb(string inputStr)
        {
            var submitData = inputStr.ToObject<v_ModuleInfo>();
            if (submitData == null)
            {
                return APIResponse.GetErrorResponse("当前信息为空");
            }
            var user = ibsService.GetUserInfoByUserId(submitData.UserID.ToInt());
            Tb_UserInfo tbuser = null;
            if (user != null)
            {
                tbuser = new Tb_UserInfo();
                tbuser.UserID = user.UserID.ToInt();
                tbuser.TrueName = user.TrueName;
                tbuser.UserName = user.UserName;
                tbuser.UserRoles = user.UserRoles;
                tbuser.TelePhone = user.TelePhone;
                tbuser.NickName = user.TrueName;
                tbuser.IsUser = user.IsUser;
                tbuser.isLogState = user.isLogState;
                tbuser.IsEnableOss = user.IsEnableOss;
                tbuser.CreateTime = user.Regdate;
                tbuser.AppId = user.AppID;
                tbuser.UserImage = user.UserImage;
            }
            EnglishResourceModel model = new EnglishResourceModel();
            model.bookId = submitData.BookID.ToInt();
            model.catalogueId = submitData.FirstTitleID.ToInt();
            if (string.IsNullOrEmpty(submitData.SecondTitleID)&& submitData.SecondTitleID!="0")
            {
                model.catalogueId = submitData.SecondTitleID.ToInt();
            }
            model.type = (int)MODSourceTypeEnum.HearResource;
            model.FirstTitleID = submitData.FirstTitleID.ToInt();
            model.SecondTitleID = submitData.SecondTitleID;
            model.FirstModularID = submitData.FirstModularID.ToInt();
            model.SecondModularID = submitData.SecondModularID.ToInt();
            var respon = tbxService.GetHearResource(model);
            List<v_ListSubModules_0> list = new List<v_ListSubModules_0>();

            if (respon.Success)
            {
                var data = respon.Data as List<v_HearResources>;
                data.ForEach(a =>
                {
                    v_ListSubModules_0 item = new v_ListSubModules_0();
                    item.SecondModularID = a.SecondModularID;
                    item.ModularName = EnumHelper.GetEnumDesc<SecondModularEnum>(a.SecondModularID);
                    item.ModularEN = a.ModularEN;
                    item.RepeatNumber = a.RepeatNumber;
                    item.SecondTitleID = a.SecondTitleID;
                    item.SerialNumber = a.SerialNumber;
                    item.TextSerialNumber = a.TextSerialNumber;
                    list.Add(item);
                });

                var resource = tbxService.GetHearResourceByWeb(submitData.UserID, submitData.BookID.ToInt(), submitData.FirstTitleID.ToInt(), submitData.SecondTitleID, list);
                var ds = new
                {
                    Success = true,
                    ResourcesList = resource,
                    UserInfo = tbuser
                };
                return APIResponse.GetResponse(ds.ToJson());
            }

            return APIResponse.GetErrorResponse("");

        }
    }
}
