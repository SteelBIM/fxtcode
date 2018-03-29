using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface ICompanyProduct_Module
    {
        /// <summary>
        /// 根据公司id和产品code获得相应的产品模块
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        //IQueryable<CompanyProduct_Module> GetAccessedModules(string companyId, string productCode, string userName);

        List<int> GetAccessedModules(string companyId, string userName);

        /// <summary>
        /// 根据公司id和产品code获取开通的城市，同时判断该账号的城市权限
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        //IQueryable<CompanyProduct_Module> GetAccessedCities(string companyId, string productCode, string userName);

        List<CompanyProduct_Module> FxtUserCenterService_GetCPM(int fxtcompanyid, int productTypeCode);

        /// <summary>
        /// 根据城市ID返回城市名称
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        string GetCityName(int cityId);

        SYS_City GetProvinceName(int cityId);

        IEnumerable<SYS_City> GetProvinceNames(IEnumerable<int> cityIds);

        /// <summary>
        /// 根据用户名获取用户产品的开通城市
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        //IQueryable<SYS_City> GetAccessedCity(string userName);

        /// <summary>
        /// 获取客户产品开通的所有城市
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        //IQueryable<SYS_City> GetProductCities(int fxtcompanyid, int producttypecode);

        /// <summary>
        /// 获取所有城市,为城市列表不限
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IQueryable<SYS_City> GetCities();

        string GetCodeName(int code);

        /// <summary>
        /// 批量获取模块名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Dictionary<string, string> GetCodeNames(IEnumerable<int> codes);

        /// <summary>
        /// 批量获取城市名称
        /// </summary>
        /// <param name="cityIds"></param>
        /// <returns></returns>
        Dictionary<string, string> GetCityNames(List<int> cityIds);
    }
}
