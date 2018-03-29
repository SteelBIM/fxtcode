using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Logic;
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
            string signname = func.Value<string>("signname");
            string companycode = func.Value<string>("companycode");

            var cproduct = CompanyProductBL.GetInfo(companyid, producttypecode, signname, 1, companycode);

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
        /// 对外方法名：cpthree 参数名：CompanyProduct.field (companyid\producttypecode\cityid 主键字段必传)
        /// 20150810 wb 增加产品授权
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData SetOpenProducts(string sinfo, string info)
        {
            var funinfoString = JObject.Parse(info)["funinfo"].ToJson();

            CompanyProduct cpentity = null;
            cpentity = JSONHelper.JSONToObject<CompanyProduct>(funinfoString);

            //验证
            if (cpentity == null)
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "参数有误");
            }

            if (cpentity.companyid <= 0 || cpentity.producttypecode <= 0 || cpentity.cityid == null)
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "请传入公司ID、产品ID、城市ID");
            }

            //执行
            int res = CompanyProductBL.SetOpenProduct(cpentity);

            if (0 < res)
            {
                return JSONHelper.GetWcfJson(1, (int)EnumHelper.Status.Success, "成功");
            }
            else
            {
                return JSONHelper.GetWcfJson(0, (int)EnumHelper.Status.Failure, "失败");
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
                JObject objInfo = JObject.Parse(info);
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
                }
                else
                {
                    return JSONHelper.GetWcfJson(model, (int)EnumHelper.Status.Success, "成功");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据已有客户配置-开通城市数据查询权限(VQ专用)
        /// zhoub 20160527
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData CompanyProductAdd(string sinfo, string info)
        {
            try
            {
                DateTime overDate = DateTime.Now;
                var func = JObject.Parse(info)["funinfo"];
                int addcompanyid = StringHelper.TryGetInt(func.Value<string>("addcompanyid")); ;
                int addproducttypecode = StringHelper.TryGetInt(func.Value<string>("addproducttypecode")); ;
                //int existingcompanyid = StringHelper.TryGetInt(func.Value<string>("existingcompanyid")); ;
                //int existingproducttypecode = StringHelper.TryGetInt(func.Value<string>("existingproducttypecode"));
                string cityids = func.Value<string>("cityids");
                if (addproducttypecode == 0 || string.IsNullOrEmpty(cityids) || addcompanyid == 0)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "参数错误");
                }
                CompanyInfo companyInfo = CompanyBL.Get(addcompanyid);
                if (companyInfo == null)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "新增公司不存在");
                }
                else
                {
                    addcompanyid = companyInfo.companyid;
                }

                CompanyProduct fxtCompanyProduct = new CompanyProduct();
                List<CompanyProduct> listCompanyProduct = CompanyProductBL.GetCompanyProductByCodeAndCompanyIdAndCityId(addproducttypecode, addcompanyid);
                var list = CompanyProductBL.GetCompanyProductByCodeAndCompanyIdAndCityId(addproducttypecode, 25).Where(w => w.parentproducttypecode > 0);
                Dictionary<string, CompanyProduct> dict = new Dictionary<string, CompanyProduct>();
                List<string> existsCityIdList = new List<string>();
                //先清空所有权限
                foreach (CompanyProduct cp in listCompanyProduct)
                {
                    cp.valid = 0;
                    CompanyProductBL.CompanyProductUpdate(cp);
                    dict.Add(cp.cityid.ToString(), cp);
                    existsCityIdList.Add(cp.cityid.ToString());
                }
                //如果还未开通产品，默认从房讯通公司里提取数据
                if (listCompanyProduct == null || listCompanyProduct.Count == 0)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "添加失败，请先开通产品权限.");
                }
                CompanyProduct pro = listCompanyProduct.OrderByDescending(obd => obd.overdate).FirstOrDefault();
                pro.valid = 1;
                if (!string.IsNullOrEmpty(func.Value<string>("overdate")))
                {
                    pro.overdate = Convert.ToDateTime(func.Value<string>("overdate"));
                }

                List<string> tempCityIdList = cityids.Split(',').ToList();
                //新增
                foreach (string ci in tempCityIdList)
                {
                    if (!ci.Equals("0"))
                    {
                        if (!existsCityIdList.Contains(ci))
                        {
                            fxtCompanyProduct = list.Where(wh => wh.cityid == Convert.ToInt32(ci)).OrderByDescending(ob => ob.overdate).FirstOrDefault();
                            pro.companyid = addcompanyid;
                            pro.producttypecode = addproducttypecode;
                            pro.cityid = Convert.ToInt32(ci);
                            pro.parentproducttypecode = fxtCompanyProduct.parentproducttypecode;
                            pro.parentshowdatacompanyid = fxtCompanyProduct.parentshowdatacompanyid;
                            pro.SetAvailableFields(new string[] { });
                            CompanyProductBL.CompanyProductAdd(pro);
                        }
                    }
                }
                //更新
                foreach (string ci in tempCityIdList)
                {
                    if (!ci.Equals("0"))
                    {
                        if (existsCityIdList.Contains(ci))
                        {
                            fxtCompanyProduct = list.Where(wh => wh.cityid == Convert.ToInt32(ci)).OrderByDescending(ob => ob.overdate).FirstOrDefault();
                            CompanyProduct c = dict[ci];
                            c.parentproducttypecode = fxtCompanyProduct.parentproducttypecode;
                            c.parentshowdatacompanyid = fxtCompanyProduct.parentshowdatacompanyid;
                            c.valid = 1;
                            c.SetAvailableFields(new string[] { "parentproducttypecode", "valid", "parentshowdatacompanyid" });
                            CompanyProductBL.CompanyProductUpdate(c);
                        }
                    }
                }
                //开通流量配置
                CompanyProductBL.FlowControlConfig(addcompanyid);
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "添加成功");
            }
            catch (Exception ex)
            {
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "添加失败");
            }
        }

        /// <summary>
        /// 根据已有客户配置-开通城市数据查询权限(估价宝专用)
        /// zhoub 20160527
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GjbCompanyProductAdd(string sinfo, string info)
        {
            try
            {
                DateTime overDate = DateTime.Now;
                var func = JObject.Parse(info)["funinfo"];
                int addcompanyid = StringHelper.TryGetInt(func.Value<string>("addcompanyid")); ;
                int addproducttypecode = StringHelper.TryGetInt(func.Value<string>("addproducttypecode")); ;
                //int existingcompanyid = StringHelper.TryGetInt(func.Value<string>("existingcompanyid")); ;
                //int existingproducttypecode = StringHelper.TryGetInt(func.Value<string>("existingproducttypecode"));
                string cityids = func.Value<string>("cityids");
                if (addproducttypecode == 0 || string.IsNullOrEmpty(cityids) || addcompanyid == 0)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "参数错误");
                }
                CompanyInfo companyInfo = CompanyBL.Get(addcompanyid);
                if (companyInfo == null)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "新增公司不存在");
                }
                else
                {
                    addcompanyid = companyInfo.companyid;
                }

                //CompanyProduct fxtCompanyProduct = new CompanyProduct();
                List<CompanyProduct> listCompanyProduct = CompanyProductBL.GetCompanyProductByCodeAndCompanyIdAndCityId(addproducttypecode, addcompanyid);
                //var list = CompanyProductBL.GetCompanyProductByCodeAndCompanyIdAndCityId(addproducttypecode, 25).Where(w => w.parentproducttypecode > 0);
                Dictionary<string, CompanyProduct> dict = new Dictionary<string, CompanyProduct>();
                List<string> existsCityIdList = new List<string>();
                //先清空所有权限
                foreach (CompanyProduct cp in listCompanyProduct)
                {
                    cp.valid = 0;
                    CompanyProductBL.CompanyProductUpdate(cp);
                    dict.Add(cp.cityid.ToString(), cp);
                    existsCityIdList.Add(cp.cityid.ToString());
                }
                //如果还未开通产品，默认从房讯通公司里提取数据
                if (listCompanyProduct == null || listCompanyProduct.Count == 0)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "添加失败，请先开通产品权限.");
                }
                CompanyProduct pro = listCompanyProduct.OrderByDescending(obd => obd.overdate).FirstOrDefault();
                pro.valid = 1;
                if (!string.IsNullOrEmpty(func.Value<string>("overdate")))
                {
                    pro.overdate = Convert.ToDateTime(func.Value<string>("overdate"));
                }

                List<string> tempCityIdList = cityids.Split(',').ToList();
                //新增
                foreach (string ci in tempCityIdList)
                {
                    if (!ci.Equals("0"))
                    {
                        if (!existsCityIdList.Contains(ci))
                        {
                            pro.companyid = addcompanyid;
                            pro.producttypecode = addproducttypecode;
                            pro.cityid = Convert.ToInt32(ci);
                            pro.parentproducttypecode = 1003002;
                            pro.parentshowdatacompanyid = addcompanyid;
                            pro.SetAvailableFields(new string[] { });
                            CompanyProductBL.CompanyProductAdd(pro);
                        }
                    }
                }
                //更新
                foreach (string ci in tempCityIdList)
                {
                    if (!ci.Equals("0"))
                    {
                        if (existsCityIdList.Contains(ci))
                        {
                            CompanyProduct c = dict[ci];
                            c.parentproducttypecode = 1003002;
                            c.parentshowdatacompanyid = addcompanyid;
                            c.valid = 1;
                            c.SetAvailableFields(new string[] { "parentproducttypecode", "valid", "parentshowdatacompanyid" });
                            CompanyProductBL.CompanyProductUpdate(c);
                        }
                    }
                }
                //开通流量配置
                CompanyProductBL.FlowControlConfig(addcompanyid);
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "添加成功");
            }
            catch (Exception ex)
            {
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "添加失败");
            }
        }


        /// <summary>
        /// 查询公司是否包含该产品和城市
        /// zhoub 20160914
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GetCompanyProductByCompanyidAndProductTypeCode(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];
            int companyid = StringHelper.TryGetInt(func["companyid"].ToString());
            int producttypecode = StringHelper.TryGetInt(func["producttypecode"].ToString());
            int cityid = StringHelper.TryGetInt(func["cityid"].ToString());

            var companyList = CompanyProductBL.GetCompanyProductByCompanyidAndProductTypeCode(companyid, producttypecode, cityid).Select(s => new
            {
                parentproducttypecode = s.parentproducttypecode,
                parentshowdatacompanyid = s.parentshowdatacompanyid,
                companyid = s.companyid,
                isdeletetrue = s.isdeletetrue,
                isexporthose = s.isexporthose
            });

            return JSONHelper.GetWcfJson(companyList, (int)EnumHelper.Status.Success, "成功");
        }
    }
}
