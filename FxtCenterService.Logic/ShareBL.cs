using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.FxtDataCenter;
using FxtCenterService.DataAccess;
using Newtonsoft.Json.Linq;
using FxtCenterService.Common;

namespace FxtCenterService.Logic
{
   public class ShareBL
    {
        /// <summary>
        ///  添加
        /// </summary>
        /// <returns></returns>
        public static int AddCompanyShowData(PriviCompanyShowData pcs)
        {
            return ShareDA.AddCompanyShowData(pcs);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public static int UpdateCompanyShowData(PriviCompanyShowData pcs,IEnumerable<string> keys)
        {
            return ShareDA.UpdateCompanyShowData(pcs,keys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public static List<PriviCompanyShowData> GetCompanyShowData(PriviCompanyShowData pcs, JObject objSinfo, JObject objInfo)
        {
            List<PriviCompanyShowData> result = ShareDA.GetCompanyShowData(pcs);
            if (result.Count > 0)
            {
                string returnStr = FxtCenterWebCommon.UserCenterApiPost(FxtCenterWebCommon.UserCenterApiPostData(objSinfo, objInfo["appinfo"]["systypecode"].ToString(), "companytwelve", new
                {
                    companyids = string.Join(",", result.Select(o => o.fxtcompanyid).Distinct())
                }));
                JObject returnJson = JObject.Parse(returnStr);
                if (returnJson.Value<int>("returntype") != 1)
                {
                    throw new Exception(string.Format("调用接口异常,返回信息:{0},状态值:{1}", returnJson.Value<string>("returntext"), returnJson.Value<string>("returntype")));
                }
                JArray jsonReturnNames = JArray.Parse(returnJson["data"].ToString());
                if (jsonReturnNames.Count > 0)
                {
                    List<PriviCompanyShowData> copyList = new List<PriviCompanyShowData>();//防止获取无对应公司的公司查询记录
                    Dictionary<string, string> companyNames = new Dictionary<string, string>();
                    foreach (var item in jsonReturnNames)
                    {
                        if (!companyNames.Keys.Contains(item.Value<string>("companyid")))
                        {
                            companyNames.Add(item.Value<string>("companyid").ToUpper(), item.Value<string>("companyname"));
                        }
                    }
                    result.ForEach(o =>
                    {
                        if (companyNames.Keys.Contains(o.fxtcompanyid.ToString().ToUpper()))
                        {
                            o.CompanyName = companyNames[o.fxtcompanyid.ToString().ToUpper()];
                            copyList.Add(o);
                        }
                    });
                    result = copyList;
                }
            }
            return result;
        }
    }
}
