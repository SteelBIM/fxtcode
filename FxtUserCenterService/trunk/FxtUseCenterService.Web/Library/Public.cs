using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Entity;
using CAS.Common;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using CAS.Entity.CASEntity;

namespace FxtUseCenterService.Web.Library
{
    public class Public
    {
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="d">账号</param>
        /// <param name="um">原密码:desc</param>
        /// <param name="np">新密码：明文</param>
        /// <returns></returns>
        public static WCFJsonData GetAPIPwdCheckResult(string np, string token, HttpContext context)
        {
            string username = SessionHelper.Get<string>("username");

            WCFJsonData jsonData = new WCFJsonData();
            //判断访问是否合法
            if (token == SessionHelper.Get<string>("token") && !string.IsNullOrEmpty(username))
            {
                if (!string.IsNullOrEmpty(np))
                {
                    string mapPath = context.Server.MapPath("/upload/token/" + token + ".token");
                    initSessionAndCache(mapPath, token);

                    string password = EncryptHelper.GetMd5(EncryptHelper.PasswordToText(SessionHelper.Get<string>("password")) + ConstCommon.WcfPassWordMd5Key);
                    string apiUrl = WebCommon.GetConfigSetting("fxtusercenterservice");
                    WCFSInfo sinfo = new WCFSInfo();
                    sinfo.appid = (int)EnumHelper.Codes.SysTypeCodeUserCenter;
                    sinfo.appkey = WebCommon.GetConfigSetting("ucappkey");
                    sinfo.apppwd = WebCommon.GetConfigSetting("ucapppwd");
                    sinfo.functionname = "usereight";
                    sinfo.signname = WebCommon.GetConfigSetting("signname");
                    WCFUInfo uinfo = new WCFUInfo();
                    WCFAPPInfo appInfo = new WCFAPPInfo();
                    appInfo.systypecode = (int)EnumHelper.Codes.SysTypeCodePMB;
                    uinfo.username = username;
                    var funinfo = new { username = username, oldpassword = password, password = np };

                    string txt = WCFAPIHelper.GenerateWCFAPIParameters(sinfo, uinfo, appInfo, funinfo);
            
                    string content = string.Empty;
                    string pattern = @"[a-zA-Z]+\S+[0-9]+|[0-9]+[a-zA-Z]+";
                    //验证是否为正常密码
                    if (np.Length < 6 || np.Length > 20 || !Regex.IsMatch(np, pattern))
                    {
                        jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.SimplePassWord, "您输入的密码过于简单");
                    }
                    else
                    {

                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(apiUrl);
                        request.ContentType = "application/json";
                        request.Method = "POST";
                        MemoryStream memory = new MemoryStream();
                        byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(txt);
                        request.ContentLength = postdata.Length;
                        Stream newStream = request.GetRequestStream();
                        newStream.Write(postdata, 0, postdata.Length);
                        newStream.Close();

                        HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                        StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                        content = reader.ReadToEnd();//得到结果
                        jsonData = JSONHelper.JSONToObject<WCFJsonData>(content);
                        if (jsonData.returntype == 1)
                        {
                            //密码修改成功，清空Session
                            SessionHelper.Add<string>("username", "");
                        }
                    }
                }
                else
                {
                    jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.None, "密码为空");
                }
            }
            else
            {

                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问(10010)");
            }

            return jsonData;
        }

        /// <summary>
        /// 访问密码修改页面，初始化Session与Cache
        /// </summary>
        /// <param name="username"></param>
        private static bool initSessionAndCache(string mapPath, string token)
        {
            bool isExsts = false;
           string username = "";
           string password = "";
            //删除token
            if (File.Exists(mapPath))
            {
                string line = string.Empty;
                using (StreamReader sr = new StreamReader(mapPath))
                {
                    
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    line = sr.ReadLine();
                }
                string[] lines = line.Split(',');
                username = lines[0];
                password = lines[1];
                //缓存用户名
                SessionHelper.Add<string>("username",username );
                //缓存token
                SessionHelper.Add<string>("token", token);
                //缓存密码
                SessionHelper.Add<string>("password", password);
                File.Delete(mapPath);
                isExsts = true;
            }
            return isExsts;
        }
    }
}