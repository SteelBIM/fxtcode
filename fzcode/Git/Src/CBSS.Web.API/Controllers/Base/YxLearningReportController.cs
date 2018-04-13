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
        /// 根据书籍Id和班级Id查询单元学习人数
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetYxUnitLearningByBookId(string inputStr)
        {
            UnitLearningListByBookId submitData;
            tbxService.VerifyParam<UnitLearningListByBookId>(inputStr, out submitData);

            APIResponse htm = new APIResponse();
            switch (submitData.Version)
            {
                case "V1":
                    htm = tbxService.GetYxUnitLearningByBookId(submitData.BookId, submitData.ClassId, submitData.PageNumber);
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

#pragma warning disable CS1572 // XML 注释中有“request”的 param 标记，但是没有该名称的参数
        /// <summary>
        /// 根据班级Id、册别、目录统计趣配音学习情况
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]

        public APIResponse GetYxVideoDetailsByModuleId(string inputStr)
        {
            VideoDetailsByModuleId submitData;
            tbxService.VerifyParam<VideoDetailsByModuleId>(inputStr, out submitData);

            APIResponse htm = new APIResponse();
            switch (submitData.Version)
            {
                case "V1":
                    htm = tbxService.GetYxVideoDetailsByModuleId(submitData.BookId, submitData.ClassId, submitData.FirstTitleID, submitData.SecondTitleID);
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
        public APIResponse GetYxClassStudyDetailsByClassId(string inputStr)
        {
            UnitLearningListByBookId submitData;
            tbxService.VerifyParam<UnitLearningListByBookId>(inputStr, out submitData);

            APIResponse htm = new APIResponse();
            switch (submitData.Version)
            {
                case "V1":
                    htm = tbxService.GetYxClassStudyDetailsByClassId(submitData.ClassId, submitData.BookId, submitData.VideoNumber, submitData.PageNumber,4);
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