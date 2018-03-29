using FXT.VQ.UserService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/***********************************************************
 * 功能：获取用户中数据
 *  
 * 创建：魏贝
 * 时间：2015/12
***********************************************************/

namespace FXT.VQ.UserService
{
    public class UserDataServices
    {
        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public static string GetCompanyInfoBySignName(string signName, out string outJson)
        {
            SurveyApi sa = new SurveyApi("companyfive");
            sa.info.funinfo = new
            {
                signname = signName
            };
            string json = sa.GetJsonString();
            outJson = json;

            return ServiceHelper.APIPostBack(json);
        }
    }
}
