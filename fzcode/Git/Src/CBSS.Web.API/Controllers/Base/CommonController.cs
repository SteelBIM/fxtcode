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
    /// <summary>
    /// 通用（基础数据）
    /// </summary>
    public partial class BaseController
    {
        static ICfgmanagerService cfgmanagerService = new CfgmanagerService();
        static ITbxService tbxService = new TbxService();
        static IIBSService ibsService = new IBSService();
        static IPayOrder ipayOrder = new PayOrderBLL();
        static RedisHashHelper redis = new RedisHashHelper("Tbx");
        static RedisListHelper redisList = new RedisListHelper("Tbx");
        static Repository _repository = new Repository("Tbx");


        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetBookSecretKey(string inputStr)
        {
            var param = inputStr.ToObject<EnglishResourceModel>();
            var result = tbxService.GetBookSecretKey(param.bookId);
            return result;
        }
        /// <summary>
        /// 14 获取课本目录 √
        /// </summary>
        [HttpPost]
        public static APIResponse GetBookCatalog(string inputStr)
        {
            BookID input;
            var verifyResult = tbxService.VerifyParam<BookID>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var res = tbxService.GetBookCatalog(Convert.ToInt32(input.bookID)).CatalogRecursive(0);
            return APIResponse.GetResponse(res);
        }

        /// <summary>
        /// 15 获取目录模块 √
        /// </summary>
        [HttpPost]
        public static APIResponse CheckModulePermission(string inputStr)
        {
            CheckModulePermissionModel model;
            var verifyResult = tbxService.VerifyParam<CheckModulePermissionModel>(inputStr, out model, new List<string> { "userID" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var res = tbxService.CheckModulePermission(model.moduleIds, model.appID, model.userID);
            return APIResponse.GetResponse(res);
        }

        /// <summary>
        /// 15 获取目录模块 √
        /// </summary>
        [HttpPost]
        public static APIResponse GetCatalogModule(string inputStr)
        {
            MarketBookCatalogID input;
            var verifyResult = tbxService.VerifyParam<MarketBookCatalogID>(inputStr, out input, new List<string> { "userID" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var res = tbxService.GetCatalogModule(Convert.ToInt32(input.marketBookCatalogID), input.appID, input.userID);
            return APIResponse.GetResponse(res);
        }

        //------------------以下科目选择相关接口-------------------
        /// <summary>
        /// 10 获取科目，版本 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetMarketClassify(string inputStr)
        {
            ParentIDAndAppID input;
            var verifyResult = tbxService.VerifyParam<ParentIDAndAppID>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var listClassify = tbxService.GetV_AppMarketClassifyList(x => x.AppID == input.appID).Judge(input.parentID);

            if (listClassify.Count == 1)
            {
                var listBook = tbxService.GetAppMarketBooks(listClassify.First().MarketClassifyID, input.appID.ToString());
                return APIResponse.GetResponse(listBook);
            }
            else
            {
                return APIResponse.GetResponse(listClassify);
            }
        }

        /// <summary>
        /// 11 获取书本 √
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetBooks(string inputStr)
        {
            ClassifyID input;
            var verifyResult = tbxService.VerifyParam<ClassifyID>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var list = tbxService.GetMarketBooks(input.classifyID);

            return APIResponse.GetResponse(list);
        }

        /// <summary>
        /// 5 获取学校信息 √
        /// </summary>
        /// <returns></returns>
        public static APIResponse GetSchoolInfo(string inputStr)
        {
            AreaID input;
            var verifyResult = tbxService.VerifyParam<AreaID>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var js = ibsService.GetAreaSchRelationByAreaId(Convert.ToInt32(input.areaID));
            return APIResponse.GetResponse(js.AreaSchList);
        }
    }
}

