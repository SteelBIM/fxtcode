using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.DataAccess;
using FxtUserCenterService.Entity;
using CAS.Common;

namespace FxtUserCenterService.Logic
{
    public class CompanyBL
    {
        public static CompanyInfo GetCompanyBySignName(string signName)
        {
            return CompanyDA.GetCompanyBySignName(signName);
        }

        public static CompanyInfo Get(string companycode)
        {
            return CompanyDA.Get(companycode);
        }

        public static CompanyInfo Get(int companyid)
        {
            return CompanyDA.Get(companyid);
        }

        public static int update(string companycode, string wxid, string wxname)
        {
            return CompanyDA.update(companycode, wxid, wxname);
        }

        /// <summary>
        /// 通过productTypeCode、key搜索
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyInfo(SearchBase search, int productTypeCode, string key)
        {
            return CompanyDA.GetCompanyInfo(search, productTypeCode, key);
        }
        public static List<CompanyInfo> CompanyList()
        {
            return CompanyDA.CompanyList();
        }

         /// <summary>
        /// 获得公司信息通过signname
        /// </summary>
        /// <param name="systypecode">产品code</param>
        /// <param name="signname">公司标示</param>
        /// <param name="appid">appid</param>
        /// <param name="apppwd">apppwd</param>
        /// <returns></returns>
        public static CompanyInfo GetCompanyInfoBySignName(int systypecode, string signname, int appid, string apppwd, string functionname)
        {
            return CompanyDA.GetCompanyInfoBySignName(systypecode, signname, appid, apppwd, functionname);
        }
    }
}
