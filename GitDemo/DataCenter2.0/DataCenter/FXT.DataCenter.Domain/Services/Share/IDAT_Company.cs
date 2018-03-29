using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IDAT_Company
    {
        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="ChineseName">公司名称</param>
        /// <returns></returns>
        DAT_Company GetDAT_CompanyInfo(string ChineseName);

        /// <summary>
        /// 添加公司
        /// </summary>
        /// <param name="ChineseName">公司中文名称</param>
        /// <param name="Protypecode">产品Code</param>
        /// <param name="CityId">城市Id</param>
        /// <returns></returns>
        int AddDAT_Compandy(string ChineseName, int Protypecode, string CityId, int fxtcompanyid);

    }
}
