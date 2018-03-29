using System;
using System.Web;
using System.Text.RegularExpressions;
using FXT.DataCenter.Infrastructure.Common.Common;

namespace FXT.DataCenter.Infrastructure.Common.CommonWeb
{
    /// <summary>
    /// 请求相关操作类
    /// </summary>
    public class RequestHelper
    {
        /// <summary>
        /// 获得请求的字符串值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>值</returns>
        public static string GetString(string name)
        {
            if (HttpHelper.CurrentRequest[name] == null)
            {
                return "";
            }
            return HttpHelper.CurrentRequest[name];
        }

        /// <summary>
        /// 获得请求的Int值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>值</returns>
        public static int GetInt(string name)
        {
            return TryParseHelper.StrToInt32(HttpHelper.CurrentRequest[name], 0);
        }

        /// <summary>
        /// 获得查询字符串的值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>值</returns>
        public static string GetQueryString(string name)
        {
            if (HttpHelper.CurrentRequest.QueryString[name] == null)
            {
                return "";
            }
            return HttpHelper.CurrentRequest.QueryString[name];
        }

        /// <summary>
        /// 获得查询字符串中的Int值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>值（转换失败时，默认值为0）</returns>
        public static int GetQueryInt(string name)
        {
            return GetQueryInt(name, 0);
        }

        /// <summary>
        /// 获得查询字符串中的Int值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="defaultValue">转换失败时的默认值</param>
        /// <returns>值</returns>
        public static int GetQueryInt(string name, int defaultValue)
        {
            return TryParseHelper.StrToInt32(HttpHelper.CurrentRequest.QueryString[name], defaultValue);
        }

        /// <summary>
        /// 获得网站的根URL
        /// </summary>
        /// <returns>网站的根URL</returns>
        public static string GetBaseUrl()
        {
            if (HttpHelper.CurrentRequest.ApplicationPath == "/")
            {
                return "http://" + RequestHelper.GetServerString("HTTP_HOST");
            }
            return "http://" + RequestHelper.GetServerString("HTTP_HOST") + HttpHelper.CurrentRequest.ApplicationPath;
        }

        /// <summary>
        /// 过滤Html标记和其他符号
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns>过滤后的内容</returns>
        public static string FilterHtml(string content)
        {
            string text = content.Trim();

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text, "<\\/?[^>]+>", "");	// 去掉所有的html标记
            text = Regex.Replace(text, "[\\s]{2,}", "");	// 除两个以上的空格
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", "");	//　替换&nbsp;

            return text;
        }

        /// <summary>
        /// 替换Html标记和其他符号
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns>替换后的内容</returns>
        public static string ReplaceHtml(string content)
        {
            string text = content.Trim();

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text, "[\\s]{2,}", " ");	// 除两个以上的空格
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	// 替换<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//　替换&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags

            return text;
        }

        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="name">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string name)
        {
            if (HttpHelper.CurrentRequest.ServerVariables[name] == null)
            {
                return "";
            }
            return HttpHelper.CurrentRequest.ServerVariables[name].ToString();
        }

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpHelper.CurrentRequest.UrlReferrer == null)
            {
                return false;
            }
            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpHelper.CurrentRequest.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string pageName = string.Empty;

            string absolutePath = HttpHelper.CurrentRequest.Url.AbsolutePath;
            try
            {
                pageName = absolutePath.Substring(absolutePath.LastIndexOf("/") + 1).ToLower();
            }
            catch { }

            return pageName;
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            try
            {
                string forwarded = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string remote = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                string IP = GetIPAddress(forwarded, remote);
                return IP;
            }
            catch { return ""; }

        }
        /// <summary>
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址
        /// </summary>
        /// <param name="forwarded"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        private static string GetIPAddress(string forwarded, string remote)
        {
            //forwarded＝ HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
            //remote ＝ HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
            string result = String.Empty;
            result = forwarded;
            if (result != null && result != String.Empty)
            {
                //可能有代理  
                if (result.IndexOf(".") == -1)        //没有“.”肯定是非IPv4格式  
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。  
                        result = result.Replace("  ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];        //找到不是内网的地址  
                            }
                        }
                    }
                    else if (IsIPAddress(result))  //代理即是IP格式  
                        return result;
                    else
                        result = null;        //代理中的内容  非IP，取IP  
                }
            }

            string IpAddress = (result != null && result != String.Empty) ? result : remote;

            if (null == result || result == String.Empty)
                result = remote;
            if (result == null || result == String.Empty)
                result = HttpContext.Current.Request.UserHostAddress;
            return result;
        }
        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);

            return regex.IsMatch(str1);
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip">要验证的IP地址</param>
        /// <returns>bool值</returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }

        /// <summary>
        /// 保存用户上传的文件
        /// </summary>
        /// <param name="path">保存路径</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpHelper.CurrentRequest.Files.Count > 0)
            {
                HttpHelper.CurrentRequest.Files[0].SaveAs(path);
            }
        }
        #region 获取客户端浏览器类型及版本号
        /// <summary>
        /// 获取客户端浏览器类型及版本号
        /// </summary>
        /// <returns></returns>
        public static string GetClientBrowserVersions()
        {
            string browserVersions = string.Empty;
            HttpBrowserCapabilities hbc = HttpContext.Current.Request.Browser;
            string browserType = hbc.Browser.ToString();     //获取浏览器类型
            string browserVersion = hbc.Version.ToString();    //获取版本号
            browserVersions = browserType + "_" + browserVersion;
            return browserVersions;
        }
        #endregion
        #region 32位唯一标识符
        /// <summary>
        /// 登录
        ///32位唯一标识符
        /// </summary>
        /// <returns></returns>
        public static string GetOnlyCode()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            return guid.ToString().Trim();
        }
        #endregion
    }
}
