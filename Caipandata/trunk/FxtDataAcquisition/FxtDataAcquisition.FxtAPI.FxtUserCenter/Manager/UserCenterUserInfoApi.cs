using CAS.Common.MVC4;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataCenterDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager
{
    public static class UserCenterUserInfoApi
    {

        /// <summary>
        /// 获取用户下拉列表
        /// </summary>
        /// <param name="companyId">当前机构ID</param>
        /// <param name="userNames">用多个数组</param>
        /// <param name="keyWord">查询关键字</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="username">当前调用api的username</param>
        /// <param name="signname">当前调用api的signname</param>
        /// <returns></returns>
        public static List<UserInfo> GetUserListByCompanyId(int companyId,string[] userNames,string keyWord,int? valid,int pageIndex,int pageSize,out int count, string loginusername, string loginsignname,List<Apps> appList)
        {
            count = 0;
            StringBuilder sb = new StringBuilder("");
            if (userNames != null)
            {
                foreach (string str in userNames)
                {
                    sb.Append("").Append(str).Append(",");
                }
            }
            keyWord = keyWord.TrimBlank();
            var para = new { username = sb.ToString().Trim(','), keyword = keyWord, valid = Convert.ToString(valid), companyid = companyId, companycode = "", search = new { Page = "true", PageIndex = pageIndex, PageRecords = pageSize }.ToJSONjss() };
            DataCenterResult result = Common.PostUserCenter(loginusername, loginsignname, Common.userseven, para, appList);
            List<UserInfo> list = new List<UserInfo>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = result.data.ParseJSONList<UserInfo>();
            }
            if (list != null && list.Count > 0)
            {
                count = list[0].RecordCount;
            }
            return list;
        }
        /// <summary>
        /// 获取公司开通产品的城市ID
        /// </summary>
        /// <param name="signname"></param>
        /// <param name="productcode"></param>
        /// <param name="loginusername"></param>
        /// <param name="loginsignname"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public static int[] GetCompanyProductCityIds(string signname, int productcode, string loginusername, string loginsignname, List<Apps> appList)
        {
            int[] cityIds = null;
            var para = new { signname = signname, productcode = productcode};
            DataCenterResult result = Common.PostUserCenter(loginusername, loginsignname, Common.cptcityids, para, appList);
            List<UserInfo> list = new List<UserInfo>();
            if (!string.IsNullOrEmpty(result.data))
            {
                List<int> intList = result.data.ParseJSONList<int>();
                if (intList != null && intList.Count >0)
                {
                    cityIds = intList.ToArray();
                }
            }
            return cityIds;
        }

        /// <summary>
        /// 获取公司开通产品模块的城市ID
        /// </summary>
        /// <param name="signname"></param>
        /// <param name="productcode"></param>
        /// <param name="loginusername"></param>
        /// <param name="loginsignname"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public static int[] GetCompanyProductMuduleCityIds(string signname, int productcode, string loginusername, string loginsignname, List<Apps> appList)
        {
            int[] cityIds = null;
            var para = new { signname = signname, productcode = productcode };
            DataCenterResult result = Common.PostUserCenter(loginusername, loginsignname, Common.cptmcityids, para, appList);
            List<UserInfo> list = new List<UserInfo>();
            if (!string.IsNullOrEmpty(result.data))
            {
                List<int> intList = result.data.ParseJSONList<int>();
                if (intList != null && intList.Count > 0)
                {
                    cityIds = intList.ToArray();
                }
            }
            return cityIds;
        }

        public static LoginUser UserLogin(string userName, string password, out List<Apps> appList,out string message)
        {
            appList = new List<Apps>();
            message = "";
            DataCenterResult result = Common.PostUserCenter_Login(userName, password);
            if (result == null || string.IsNullOrEmpty(result.data) || result.returntype != 1)
            {
                LogHelper.Info("用户名或密码错误");
                message = "用户名或密码错误";
                return null;
            }
            LoginUser userObj = new LoginUser();
            JObject jobj = JObject.Parse(result.data);
            string uinfoJson = Convert.ToString(jobj.Value<object>("uinfo"));
            string sinfoJson = Convert.ToString(jobj.Value<object>("sinfo"));
            if (string.IsNullOrEmpty(uinfoJson) || string.IsNullOrEmpty(sinfoJson))
            {
                LogHelper.Info("uinfo||sinfo==null");
                message = "用户名或密码错误";
                return null;
            }
            userObj = uinfoJson.ParseJSONjss<LoginUser>();
            if (userObj==null)
            {
                LogHelper.Info("UserCenter_LoginUserInfo==null");
                message = "用户名或密码错误";
                return null;
            }
            string appsJson = Convert.ToString(jobj["sinfo"].Value<object>("apps"));
            appList = appsJson.ParseJSONList<Apps>();
            userObj.SignName = jobj["sinfo"].Value<string>("signname");
            userObj.UserName = userName;
            userObj.Password = password;
            return userObj;
        }


        public static string GetCode(string signname, string time, string functionname)
        {
            string appPwd = "1300558927";
            string appKey = "1960371990";
            string[] pwdArray = { "1003106", appPwd, signname, time, functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appKey);
            return code;
        }
        public static void trest()
        {

            HttpClient client = new HttpClient();
            string date = DateTime.Now.ToString("20140611195856");
            var para = new
            {
                sinfo = JsonHelp.ToJSONjss(new
                {
                    functionname = "getallotsurveyingproject",
                    appid = "1003106",
                    apppwd = "1300558927",
                    signname = "4106DEF5-A760-4CD7-A6B2-8250420FCB18",
                    time = date,
                    code = GetCode("4106DEF5-A760-4CD7-A6B2-8250420FCB18", date, "getallotsurveyingproject")
                }),
                info = JsonHelp.ToJSONjss(new
                {
                    uinfo = new { username = "admin@fxt", token = "" },
                    appinfo = new
                    {
                        splatype = "android",
                        stype = "yck",
                        version = "4.26",
                        vcode = "1",
                        systypecode = "1003034",
                        channel = "360"
                    },
                    funinfo = new { username = "admin@fxt", cityid = 1 }// new { userid = "3", cityid = 1, allotid = 1, data =obj2.ToJSONjss()}
                })
            };
            string strD = para.ToJSONjss();
            HttpResponseMessage hrm = client.PostAsJsonAsync("http://localhost:50887/mobileapi/runflats", para).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;
        }
    }
}
