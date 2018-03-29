using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.DataAccess;
using FxtUserCenterService.Entity;
using CAS.Entity;
using FxtUserCenterService.Entity.InheritClass;

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

         /// <summary>
        /// 修改产品部分信息:CAS产品LOGO,CAS产品小LOGO，对外显示的产品名称，产品联系电话(hody 2014-04-24)
        /// </summary>
        /// <param name="logoPath">CAS产品LOGO</param>
        /// <param name="smallLogoPath">CAS产品小LOGO</param>
        /// <param name="telephone">对外显示的产品名</param>
        /// <param name="titleName">产品联系电话</param>
        /// <returns></returns>
        public static int UpdateProductPartialInfo(string logoPath, string smallLogoPath, string telephone, string titleName, int companyid, int systypecode, string bgpic, string homepage, string twodimensionalcode)
        {
            return CompanyProductDA.UpdateProductPartialInfo(logoPath, smallLogoPath, telephone, titleName, companyid, systypecode, bgpic, homepage, twodimensionalcode);
        }

        /// <summary>
        /// 根据WebUrl查询产品信息
        /// </summary>
        /// <param name="weburl">网址</param>
        /// <param name="weburl1">备用网址</param>
        /// <returns></returns>
        public static InheritCompanyProduct GetProductInfoByWebUrl(string weburl, string weburl1)
        {
            return CompanyProductDA.GetProductInfoByWebUrl(weburl, weburl1);
        }
    }
}

