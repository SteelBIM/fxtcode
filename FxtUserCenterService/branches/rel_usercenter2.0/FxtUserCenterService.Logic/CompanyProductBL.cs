using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.DataAccess;
using FxtUserCenterService.Entity;
using CAS.Entity;

namespace FxtUserCenterService.Logic
{
    public class CompanyProductBL
    {
        /// <summary>
        /// 根据公司id和产品code获取信息(caoq 2013-7-12)
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <param name="producttypecode">产品code</param>
        /// <param name="signname">公司标识</param>
        /// <param name="isvalid">是否有效产品 1:仅查询有效产品</param>
        /// <returns></returns>
        public static CompanyProduct GetInfo(int companyid, int producttypecode, string signname, int isvalid)
        {
            List<CompanyProduct> prolist = CompanyProductDA.Get(companyid, (producttypecode > 0 ? producttypecode.ToString() : ""), signname, 1);
            return ((prolist != null && prolist.Count() > 0) ? prolist.FirstOrDefault() : null);
        }

        /// <summary>
        /// 根据公司id获取所有产品信息(caoq 2013-11-23)
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <param name="producttypecode">产品code</param>
        /// <param name="signname">公司标识</param>
        /// <param name="isvalid">是否有效产品 1:仅查询有效产品</param>
        /// <returns></returns>
        public static List<CompanyProduct> GetList(int companyid, int[] producttypecode, string signname, int isvalid)
        {
            string pro = (producttypecode == null || producttypecode.Length == 0) ? "" : string.Join(",", producttypecode.Select(i => i.ToString()).ToArray());
            return CompanyProductDA.Get(companyid, pro, signname, isvalid);
        }
    }
}

