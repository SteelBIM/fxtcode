using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.Framework.Contract.Enums;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace CBSS.Web.API.Controllers
{
    /// <summary>
    /// 资源
    /// </summary>
    public partial class BaseController
    {
        [HttpPost]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BaseController.GetResource()”的 XML 注释
        public static APIResponse GetResource()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BaseController.GetResource()”的 XML 注释
        {
            return APIResponse.GetResponse("");

        }

 
        public static APIResponse GetPrimaryEnglishResource(string inputStr)
 
        {
            var model = inputStr.ToObject<EnglishResourceModel>();

            return APIResponse.GetResponse(tbxService.GetModResource(model));
        }

        /// <summary>
        /// 初中英语资源
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetEnglishResource(string inputStr)
        {
            try
            {
                var param = inputStr.ToObject<EnglishResourceModel>();

                if (param.moduleID > 0)
                {
                    var module = tbxService.GetModuleList(o => o.ModuleID == param.moduleID).FirstOrDefault();

                    switch ((MODSourceTypeEnum)module.MODSourceType)
                    {
                        case MODSourceTypeEnum.Article:
                            return tbxService.GetArticle(param);//课文也是逐句精读的资源

                        case MODSourceTypeEnum.EBook:
                            return tbxService.GetEBook(param);

                        case MODSourceTypeEnum.Word:
                            if (IsRJXMBModule(module))//新目标单词
                            {
                                return tbxService.GetRJXMBWords(param);
                            }
                            return tbxService.GetEWords(param);

                        case MODSourceTypeEnum.Listen:
                            if (!param.type.HasValue || param.bookId <= 0)
                            {
                                return APIResponse.GetErrorResponse(ErrorCodeEnum.请求参数有误);
                            }
                            if (param.type == 0)
                            {
                                return tbxService.GetListen(param);
                            }
                            else
                            {
                                return tbxService.GetExercise(param);
                            };

                        case MODSourceTypeEnum.FollowRead:
                            return tbxService.GetFollowRead(param);

                        default:
                            return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到资源);
                    }

                }
                else
                {
                    return APIResponse.GetErrorResponse(ErrorCodeEnum.未获取到模块ID);
                }
            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.接口请求异常, LogLevelEnum.Error, ex);
            }
        }


        [HttpPost]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BaseController.GetEnglishGetTopicSets(string)”的 XML 注释
        public static APIResponse GetEnglishGetTopicSets(string input)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BaseController.GetEnglishGetTopicSets(string)”的 XML 注释
        {
            var param = input.ToObject<EnglishSpokenModel>();
            return ibsService.GetTopicSets(param.page, param.rowNum, param.type);
        }
    }
}
