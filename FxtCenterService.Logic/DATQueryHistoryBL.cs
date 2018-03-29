using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Common;
using CAS.Entity.DBEntity;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity.FxtProject;
using FxtCenterService.DataAccess;
using Newtonsoft.Json.Linq;
using FxtCenterService.Common;
using Newtonsoft.Json;

namespace FxtCenterService.Logic
{
    public class DATQueryHistoryBL
    {
        public static int Add(DATQueryHistory model)
        {
            return DATQueryHistoryDA.Add(model);
        }
        public static int Update(DATQueryHistory model)
        {
            return DATQueryHistoryDA.Update(model);
        }
        //批量更新
        public static int UpdateMul(DATQueryHistory model, int[] ids)
        {
            return DATQueryHistoryDA.UpdateMul(model, ids);
        }
        public static int Delete(int id)
        {
            return DATQueryHistoryDA.Delete(id);
        }

        public static DATQueryHistory GetDATQueryHistoryByPK(int id)
        {
            return DATQueryHistoryDA.GetDATQueryHistoryByPK(id);
        }
        /// <summary>
        /// 获取自动估价记录
        /// </summary>
        /// <param name="search"></param>
        /// <param name="username">账号</param>
        /// <returns></returns>
        public static List<DATQueryHistory> GetDATQueryHistoryList(SearchBase search, string username, string wxopenid, JObject objSinfo, JObject objInfo)
        {
            List<DATQueryHistory> result = DATQueryHistoryDA.GetDATQueryHistoryList(search, username, wxopenid);
            if (result.Count>0)
            {
                string returnStr = FxtCenterWebCommon.UserCenterApiPost(FxtCenterWebCommon.UserCenterApiPostData(objSinfo, objInfo["appinfo"]["systypecode"].ToString(), "userten", new
                {
                    username = string.Join(",", result.Select(o => o.userid).Distinct())
                }));
                JObject returnJson = JObject.Parse(returnStr);
                if (returnJson.Value<int>("returntype") != 1)
                {
                    throw new Exception(string.Format("调用接口异常,返回信息:{0},状态值:{1}", returnJson.Value<string>("returntext"), returnJson.Value<string>("returntype")));
                }
                JArray jsonReturnNames = JArray.Parse(returnJson["data"].ToString());
                if (jsonReturnNames.Count > 0)
                {
                    Dictionary<string,string> userNames =  new Dictionary<string,string>();
                    foreach (var item in jsonReturnNames)
	                {
		                if (!userNames.Keys.Contains(item.Value<string>("username")))
	                    {
		                    userNames.Add(item.Value<string>("username").ToUpper(),item.Value<string>("truename"));
	                    }
	                }
                    result.ForEach(o=>{
                        if (userNames.Keys.Contains(o.userid.ToUpper()))
                        {
                            o.truename = userNames[o.userid.ToUpper()];
                        }
                    });
                }
            }
            return result;
        }


        /// <summary>
        /// 标准化楼盘楼栋房号匹配
        /// </summary>
        /// <param name="cityid">城市Id</param>
        /// <param name="projectname">楼盘名称</param>
        /// <param name="addresss">地址</param>
        /// <param name="buildingname">楼栋名称</param>
        /// <param name="housename">房号名称</param>
        /// <returns></returns>
        public static DataSet GetMatchingData(int cityid, string projectname, string addresss, string buildingname, string housename, int p_projectid, int p_buildingid, int fxtcompanyid, int typecode)
        {
            return DATQueryHistoryDA.GetMatchingData(cityid, projectname, addresss, buildingname, housename, p_projectid, p_buildingid, fxtcompanyid, typecode);
        }

    }

}
