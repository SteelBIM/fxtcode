using CBSS.Framework.Contract.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
//using System.Web.Mvc;
using CBSS.Core.Utility;
using CourseActivate.Web.API.Model;
using CBSS.IBS.Contract.IBSResource;
using CBSS.Framework.Contract.Enums;
using CBSS.Tbx.Contract.DataModel;
using CourseActivate.Web.API.Filter;
using CBSS.Tbx.Contract.ViewModel;

namespace CBSS.Web.API.Controllers
{
    /// <summary>
    /// 初中英语记录
    /// </summary>
    //[CompressAttribute]
    public partial class BaseController
    {
      
        /// <summary>
        /// 判断是否新目标模块
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        private static bool IsRJXMBModule(Module module)
        {
            return (module.ModelID == 8 || module.ModelID == 9 || module.ModelID == 10 || module.ModelID == 11);
        }
      

        [HttpPost]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BaseController.GetEnglishGetTopicSetByID(string)”的 XML 注释
        public static APIResponse GetEnglishGetTopicSetByID(string input)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BaseController.GetEnglishGetTopicSetByID(string)”的 XML 注释
        {
            var param = input.ToObject<EnglishSpokenModel>();
            return ibsService.GetTopicSetById(param.id);
        }
        /// <summary>
        /// 获取口训做题结果
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetStuTopicSetAnswer(string inputStr)
        {
            var model = inputStr.ToObject<Rds_UserTopicSetAnswerModel>();
            if (model.id <= 0 || model.studentId <= 0)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.请求参数有误);
            }
            return tbxService.GetStuTopicSetAnswer(model);
        }
        /// <summary>
        /// 口训提交
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse SubmitTopicSetAnswer(string inputStr)
        {
            var answer = inputStr.ToObject<Rds_UserTopicSetAnswerModel>();
            if (answer.id <= 0 || answer.studentId <= 0)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.请求参数有误);
            }
            return tbxService.SubmitTopicSetAnswer(answer);
        }

        #region 新增报告
        /// <summary>
        /// 新增单词跟读报告 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static APIResponse AddWordReadReport(string inputStr)
        {
            Rds_UserWordReadRecord input;
            var verifyResult = tbxService.VerifyParam<Rds_UserWordReadRecord>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            int cataId = GetParentCatalogID(input.CatalogID, input.ModuleID);
            input.CatalogID = cataId;
            string key = input.UserID + "_" + input.CatalogID + "_" + input.ModuleID;
            var response = tbxService.AddReport<Rds_UserWordReadRecord>(input, typeof(Rds_UserWordReadRecord).Name, key
                , "AvgScore", TypeOfReport.WordRead);
            return response;
        }

        /// <summary>
        /// 新增单词听写报告 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static APIResponse AddWordDictationReadReport(string inputStr)
        {
            Rds_UserWordDictationRecord input;
            var verifyResult = tbxService.VerifyParam<Rds_UserWordDictationRecord>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            int cataId = GetParentCatalogID(input.CatalogID, input.ModuleID);
            input.CatalogID = cataId;
            string key = input.UserID + "_" + input.CatalogID + "_" + input.ModuleID;

            var response = tbxService.AddReport<Rds_UserWordDictationRecord>(input, typeof(Rds_UserWordDictationRecord).Name, key
                , "Score", TypeOfReport.WordDictation);
            return response;
        }


        private static int GetParentCatalogID(int catalogId, int moduleID)
        {
            var module = tbxService.GetModuleList(o => o.ModuleID == moduleID).FirstOrDefault();
            if (module == null)
            {
                throw new Exception("未找到模块:" + moduleID);
            }

            if (IsRJXMBModule(module))
            {
                var cata = tbxService.GetMarketBookCatalogsList(o => o.MarketBookCatalogID == catalogId).FirstOrDefault();
                if (cata == null)
                {
                    throw new Exception("未找到新目标英语目录:" + catalogId);
                }

                var pCata = tbxService.GetMarketBookCatalogsList(o => o.MarketBookCatalogID == cata.ParentCatalogID).FirstOrDefault();
                if (pCata == null)
                {
                    //return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到目录, LogLevelEnum.Error, new Exception("未找到新目标英语目录" + input.CatalogID + "的父级目录"));
                    throw new Exception("未找到新目标英语目录" + catalogId + "的父级目录");
                }
                catalogId = pCata.MODBookCatalogID.Value;
            }
            return catalogId;
        }

        /// <summary>
        /// 新增课文朗读报告 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static APIResponse AddArticleReadReport(string inputStr)
        {
            Rds_UserArticleReadRecord input;
            var verifyResult = tbxService.VerifyParam<Rds_UserArticleReadRecord>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }
                       
            string key = input.UserID + "_" + input.CatalogID + "_" + input.ModuleID;
            var response = tbxService.AddArticleReport(input, typeof(Rds_UserArticleReadRecord).Name, key
                , "AvgScore", TypeOfReport.ArticleRead);
            return response;
        }
        #endregion

        #region 分享报告
        /// <summary>
        /// 分享单词跟读报告 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static APIResponse ShareWordReadReport(string inputStr)
        {
            Rds_WordReadShare input;
            List<string> ignoreParams = new List<string>();
            ignoreParams.Add("ShareID");
            var verifyResult = tbxService.VerifyParam<Rds_WordReadShare>(inputStr, out input, ignoreParams);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }
            Guid key = Guid.NewGuid();
            input.ShareID = key;
            input.Words.ForEach(x => x.ID = Guid.NewGuid());
            var response = tbxService.ShareReport<Rds_WordReadShare>(input, typeof(Rds_WordReadShare).Name, key.ToString());
            return response;
        }

        /// <summary>
        /// 分享单词听写报告 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static APIResponse ShareWordDictationReport(string inputStr)
        {
            Rds_UserWordDictationShare input;
            List<string> ignoreParams = new List<string>();
            ignoreParams.Add("ShareID");
            var verifyResult = tbxService.VerifyParam<Rds_UserWordDictationShare>(inputStr, out input, ignoreParams);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }
            Guid key = Guid.NewGuid();
            input.ShareID = key;
            input.Words.ForEach(x => x.ID = Guid.NewGuid());
            var response = tbxService.ShareReport<Rds_UserWordDictationShare>(input, typeof(Rds_UserWordDictationShare).Name, key.ToString());
            return response;
        }
        /// <summary>
        /// 分享课文朗读报告 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static APIResponse ShareArticleReadReport(string inputStr)
        {
            Rds_ArticleReadShare input;
            List<string> ignoreParams = new List<string>();
            ignoreParams.Add("ShareID");
            var verifyResult = tbxService.VerifyParam<Rds_ArticleReadShare>(inputStr, out input, ignoreParams);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }
            Guid key = Guid.NewGuid();
            input.ShareID = key;
            input.Sentences.ForEach(x => x.ID = Guid.NewGuid());
            var response = tbxService.ShareReport<Rds_ArticleReadShare>(input, typeof(Rds_ArticleReadShare).Name, key.ToString());
            return response;
        }
        #endregion

        #region 获取报告

        /// <summary>
        /// 获取单词跟读最优报告 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static APIResponse GetWordReadReport(string inputStr)
        {
            InputReportPar input;
            var verifyResult = tbxService.VerifyParam<InputReportPar>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            int cataId = GetParentCatalogID(input.CatalogID, input.ModuleID);

            var response = tbxService.GetReport<Rds_UserWordReadRecord, Rds_UserWordReadRecordItem, UserWordReadRecord, UserWordReadRecordItem>(input.UserID
                , cataId, input.ModuleID, typeof(Rds_UserWordReadRecord).Name, "Words", "UserWordDictationRecordID");
            return response;
        }

        /// <summary>
        /// 获取单词听写最优报告 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static APIResponse GetWordDictationReadReport(string inputStr)
        {
            InputReportPar input;
            var verifyResult = tbxService.VerifyParam<InputReportPar>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            int cataId = GetParentCatalogID(input.CatalogID, input.ModuleID);

            var response = tbxService.GetReport<Rds_UserWordDictationRecord, Rds_UserWordDictationRecordItem, UserWordDictationRecord, UserWordDictationRecordItem>(input.UserID
                , cataId, input.ModuleID, typeof(Rds_UserWordDictationRecord).Name, "Words", "UserWordReadRecordID");
            return response;
        }

        /// <summary>
        /// 获取课文朗读最优报告 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static APIResponse GetArticleReadReport(string inputStr)
        {
            InputReportPar input;
            var verifyResult = tbxService.VerifyParam<InputReportPar>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var response = tbxService.GetReport<List<Rds_UserArticleReadRecord>, Rds_UserSentenceRecordItem, UserArticleReadRecord, UserSentenceRecordItem>(input.UserID
                , input.CatalogID, input.ModuleID, typeof(Rds_UserArticleReadRecord).Name, "Sentences", "UserArticleReadRecordID");
            return response;
        }
        #endregion

    }
}
