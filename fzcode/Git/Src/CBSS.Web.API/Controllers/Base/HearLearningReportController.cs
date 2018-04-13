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
        /// 根据书籍Id和班级Id查询单元学习人数(老同步学GetUnitLearningByBookId方法)
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponse GetUnitHearLearningByBookId(string inputStr)
        {
            UnitLearningListByBookId submitData;
            tbxService.VerifyParam<UnitLearningListByBookId>(inputStr, out submitData);

            APIResponse htm = new APIResponse();
            int ModelType = 3;
            if (!string.IsNullOrEmpty(submitData.Channel.ToString()))
            {
                ModelType = 3;
            }
            else if (submitData.Channel == 301)
            {
                ModelType = 6;
            }
            switch (submitData.Version)
            {
                case "V1":
                    htm = tbxService.GetUnitLearningByBookId(submitData.BookId, submitData.ClassId, submitData.PageNumber, ModelType.ToString());
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
        public APIResponse GetHearResourcesByModuleId(string inputStr)
        {
            VideoDetailsByModuleId submitData ;
            tbxService.VerifyParam<VideoDetailsByModuleId>(inputStr, out submitData);
            int ModelType = 3;
            if (!string.IsNullOrEmpty(submitData.Channel.ToString()))
            {
                ModelType = 3;
            }
            else if (submitData.Channel == 301)
            {
                ModelType = 6;
            }
            APIResponse htm = new APIResponse();
            IBS_ClassUserRelation userClassList = ibsService.GetClassUserRelationByClassOtherId(submitData.ClassId, 1);
            switch (submitData.Version)
            {
                case "V1":
                    htm = tbxService.GetHearResourcesByModuleId(submitData.BookId, submitData.ClassId, submitData.FirstTitleID, submitData.SecondTitleID, userClassList,ModelType.ToString());
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
        [HttpPost]
        public APIResponse GetHearLearningClassStudyDetailsByClassId(string inputStr)
        {
            UnitLearningListByBookId submitData ;
            tbxService.VerifyParam<UnitLearningListByBookId>(inputStr, out submitData);
           
            int ModelType = 3;
            if (!string.IsNullOrEmpty(submitData.Channel.ToString()))
            {
                ModelType = 3;
            }
            else if (submitData.Channel == 301)
            {
                ModelType = 6;
            }
            APIResponse htm = new APIResponse();
            IBS_ClassUserRelation userClassList = ibsService.GetClassUserRelationByClassOtherId(submitData.ClassId, 1);
            switch (submitData.Version)
            {
                case "V1":
                    htm = tbxService.GetHearLeaningClassStudyDetailsByClassId( submitData.BookId, submitData.FirstTitleID, submitData.SecondTitleID, submitData.FirstModularID, submitData.VideoNumber, submitData.PageNumber, userClassList,ModelType.ToString(), submitData.AppID);
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