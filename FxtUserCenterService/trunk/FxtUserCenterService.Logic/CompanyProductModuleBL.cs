using CAS.Common;
using FxtUserCenterService.DataAccess;
using FxtUserCenterService.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtUserCenterService.Logic
{
    public class CompanyProductModuleBL
    {
        /// <summary>
        /// 根据公司signname获取所有产品模块信息
        /// </summary>
        /// <param name="signname">公司标识</param>
        /// <param name="producttypecode">产品code</param>
        /// <returns></returns>
        public static List<CompanyProductModule> GetList(string signname, int producttypecode)
        {
            return CompanyProductModuleDA.Get(signname, producttypecode);
        }

        /// <summary>
        /// 查询已开通产品模块权限
        /// zhoub 20160908
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<CompanyProductModule> GetCompanyProductModuleWhetherToOpen(int producttypecode, int cityid, int companyid, int parentmodulecode, int modulecode)
        {
            return CompanyProductModuleDA.GetCompanyProductModuleWhetherToOpen(producttypecode,cityid,companyid,parentmodulecode,modulecode);
        }

        /// <summary>
        /// 根据公司ID和产品code获取公司开通产品模块城市ID(数据中心网站需求)
        /// zhoub 20161026
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="producttypecode"></param>
        /// <returns></returns>
        public static List<CompanyProductModule> GetCompanyProductAndCompanyProductModuleCityIds(SearchBase search,int companyid, int producttypecode)
        {
            return CompanyProductModuleDA.GetCompanyProductAndCompanyProductModuleCityIds(search,companyid, producttypecode);
        }

        /// <summary>
        /// 根据公司ID和产品code获取公司开通产品模块详细信息(数据中心网站需求)
        /// zhoub 20161026
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="producttypecode"></param>
        /// <returns></returns>
        public static List<CompanyProductModule> GetCompanyProductModuleDetails(SearchBase search,int companyid, int producttypecode)
        {
            return CompanyProductModuleDA.GetCompanyProductModuleDetails(search,companyid, producttypecode);
        }
    }
}
