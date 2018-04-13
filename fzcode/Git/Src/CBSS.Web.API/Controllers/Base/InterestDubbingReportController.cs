using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.IBS.BLL;
using CBSS.IBS.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using CBSS.IBS.IBLL;
using CBSS.Tbx.IBLL;
using CourseActivate.Web.API.Model;
using CourseActivate.Web.API.SMSService;
using CBSS.Framework.Redis;
using CBSS.Cfgmanager.BLL;
using CBSS.Cfgmanager.IBLL;
using CBSS.Framework.DAL;
using CBSS.Tbx.BLL;
using CBSS.Pay.IBLL;
using CBSS.Pay.BLL;


namespace CBSS.Web.API.Controllers
{
    public partial class BaseController
    {
        /// <summary>
        /// 0.根据老师查询班级信息
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetClassInfoByTeacherId(string inputStr)
        {
            ClassInfoByTeaId submitData;
            tbxService.VerifyParam<ClassInfoByTeaId>(inputStr, out submitData);

            switch (submitData.Version)
            {
                case "V1":
                    var user = ibsService.GetUserInfoByUserId(Convert.ToInt32(submitData.UserId));
                    if (user != null)
                    {
                        string classid = "";
                        List<ClassInfoList> classList = new List<ClassInfoList>();
                        if (user.ClassSchList != null && user.ClassSchList.Count > 0)
                        {
                            user.ClassSchList.ForEach(a =>
                            {
                                var classinfo = ibsService.GetClassUserRelationByClassId(a.ClassID);

                                if (!string.IsNullOrEmpty(classinfo.ClassNum))
                                {
                                    if (
                                        classList.FirstOrDefault(
                                            x => x.Id == classinfo.ClassNum.ToString()) == null)
                                    {

                                        classid += classinfo.ClassNum + ",";
                                        classList.Add(new ClassInfoList
                                        {

                                            Id = classinfo.ClassNum.ToString(),
                                            ClassName = classinfo.ClassName

                                        });
                                    }
                                }
                            });
                            return tbxService.GetClassInfoByTeacherId(submitData.AppID, classid, classList);
                        }
                        else
                        {
                            return APIResponse.GetErrorResponse("班级不存在");
                        }
                    }
                    else
                    {
                        return APIResponse.GetErrorResponse("用户不存在");
                    }
                    break;
                case "V2":
                    break;
                case "V3":
                    break;
                case "V4":
                    break;
                default:
                    return APIResponse.GetResponse("版本不存在！");
            }
            return APIResponse.GetResponse("");
        }


        /// <summary>
        /// 2.根据班级Id查询年级学习人数
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetJuniorGradeNumByClassId(string inputStr)
        {
            JuniorGradeInfoByClassId submitData;
            tbxService.VerifyParam<JuniorGradeInfoByClassId>(inputStr, out submitData);

            APIResponse htm = new APIResponse();
            switch (submitData.Version)
            {
                case "V1":
                    htm = tbxService.GetJuniorGradeNumByClassId(submitData.AppId, submitData.ClassId);
                    break;
                case "V2":
                    break;
                case "V3":
                    break;
                case "V4":
                    break;
                default:
                    return APIResponse.GetErrorResponse("版本不存在！");

            }
            return htm;
        }

        /// <summary>
        /// 根据书籍Id和班级Id查询单元学习人数
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetUnitLearningByBookId(string inputStr) 
        {
            UnitLearningListByBookId submitData;
            tbxService.VerifyParam<UnitLearningListByBookId>(inputStr, out submitData);

            APIResponse htm = new APIResponse();
            switch (submitData.Version)
            {
                case "V1":
                    htm = tbxService.GetUnitLearningByBookId(submitData.BookId, submitData.ClassId, submitData.PageNumber);
                    break;
                case "V2":
                    break;
                case "V3":
                    break;
                case "V4":
                    break;
                default:
                    htm = APIResponse.GetErrorResponse("版本不存在！");
                    break;
            }
            return htm;
        }


        /// <summary>
        /// 根据班级Id、册别、目录统计趣配音学习情况
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponse GetVideoDetailsByModuleId(string inputStr)
        {
            VideoDetailsByModuleId submitData;
            tbxService.VerifyParam<VideoDetailsByModuleId>(inputStr, out submitData);

            APIResponse htm = new APIResponse();
            switch (submitData.Version)
            {
                case "V1":
                    htm = tbxService.GetVideoDetailsByModuleId(submitData.BookId, submitData.ClassId, submitData.FirstTitleID, submitData.SecondTitleID);
                    break;
                case "V2":
                    break;
                case "V3":
                    break;
                case "V4":
                    break;
                default:
                    htm = APIResponse.GetErrorResponse("版本不存在！");
                    break;
            }
            return htm;
        }

        /// <summary>
        /// 根据书籍ID,班级ID获取班级详细学习情况
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public APIResponse GetClassStudyDetailsByClassId(string inputStr)
        {
            UnitLearningListByBookId submitData;
            tbxService.VerifyParam<UnitLearningListByBookId>(inputStr, out submitData);

            APIResponse htm = new APIResponse();
            switch (submitData.Version)
            {
                case "V1":
                    htm = tbxService.GetClassStudyDetailsByClassId(submitData.ClassId, submitData.BookId, submitData.VideoNumber, submitData.PageNumber,1);
                    break;
                case "V2":
                    break;
                case "V3":
                    break;
                case "V4":
                    break;
                default:
                    htm = APIResponse.GetErrorResponse("版本不存在！");
                    break;
            }
            return htm;
        }
    }
}