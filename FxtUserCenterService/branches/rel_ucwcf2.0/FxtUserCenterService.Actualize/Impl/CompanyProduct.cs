using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtUserCenterService.Logic;
using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Entity;
using FxtUserCenterService.Entity.InheritClass;

namespace FxtUserCenterService.Actualize.Impl
{
    public partial class Implement
    {
        //对外方法名：cp_func_1 参数名：companyid,producttypecode,signname
        public WCFJsonData GetCompanyProduct(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            int companyid = StringHelper.TryGetInt(func.Value<string>("companyid"));
            int producttypecode = StringHelper.TryGetInt(func.Value<string>("producttypecode"));
            string signname =func.Value<string>("signname");

            var cproduct = CompanyProductBL.GetInfo(companyid, producttypecode, signname, 1);

            if (cproduct != null)
                return JSONHelper.GetWcfJson(cproduct, (int)EnumHelper.Status.Success, "成功");
            else
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到机构");
        }
        //对外方法名：cptwo 参数名：companyid
        public WCFJsonData GetCompanyProductList(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            var companyid = StringHelper.TryGetInt(func["companyid"].ToString());

            if (0 >= companyid)
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到机构");    //companyid传入必须是个大于0的值

            var list = CompanyProductBL.GetList(companyid, null, string.Empty, 1);
            var temp = list.Select(query => query.producttypecode).ToArray();
            if (0 < list.Count)
                return JSONHelper.GetWcfJson(temp, (int)EnumHelper.Status.Success, "成功");
            else
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到机构");
        }
        /// <summary>
        /// 对外方法名：cptcityids 参数名：signname,productcode(创建人:曾智磊)
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetCompanyProductCityIds(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            string signname = Convert.ToString(func["signname"]);
            var productcode = StringHelper.TryGetInt(func["productcode"].ToString());

            if (string.IsNullOrEmpty(signname))
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到机构");    //companyid传入必须是个大于0的值
            }
            var list = CompanyProductBL.GetList(0, new int[] { productcode }, signname, 1);

            if (0 < list.Count)
            {
                var temp = list.Select(query => Convert.ToInt32(query.cityid)).ToArray();
                return JSONHelper.GetWcfJson(temp, (int)EnumHelper.Status.Success, "成功");
            }
            else
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "找不到机构");
            }
        }
        
        /// <summary>
        /// 修改产品部分信息:CAS产品LOGO,CAS产品小LOGO，对外显示的产品名称，产品联系电话
        /// 对外方法名：modifyproductpartinfo 
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData UpdateProductPartialInfo(string sinfo, string info)
        {
            try
            {
                JObject objInfo =JObject.Parse(info);
                JObject func = JObject.Parse(Convert.ToString(objInfo["funinfo"]));
                string logoPath = func.Value<string>("logopath");
                string smallLogoPath = func.Value<string>("smalllogopath");
                string bgpic = func.Value<string>("bg_pic");
                string telephone = func.Value<string>("telephone");
                string titleName = func.Value<string>("titlename");
                string homepage = func.Value<string>("homepage");
                string twodimensionalcode = func.Value<string>("twodimensionalcode");
                int companyid = StringHelper.TryGetInt(func.Value<string>("companyid"));
                int systypecode = StringHelper.TryGetInt(func.Value<string>("systypecode"));


                int count = CompanyProductBL.UpdateProductPartialInfo(logoPath, smallLogoPath, telephone, titleName, companyid, systypecode, bgpic, homepage, twodimensionalcode);
                if (0 < count)
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "成功");
                else
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "找不到机构");
            }
            catch (Exception ex)
            {   
                throw ex;
            }
          
        }


        /// <summary>
        /// 根据WebUrl查询产品信息
        /// </summary>
        /// <param name="weburl">网址</param>
        /// <param name="weburl1">备用网址</param>
        /// <returns></returns>
        public WCFJsonData GetProductInfoByWebUrl(string sinfo, string info)
        {
            try
            {
                JObject objInfo = JObject.Parse(info);
                var func = objInfo["funinfo"];
                string weburl = func["weburl"].ToString();
                string weburl1 = func["weburl1"].ToString();


                InheritCompanyProduct model = CompanyProductBL.GetProductInfoByWebUrl(weburl, weburl1);
                if (model == null)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "找不到和您的查询相符的内容或信息");
                }else
	            {
                    return JSONHelper.GetWcfJson(model,(int)EnumHelper.Status.Success, "成功");
	            }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
