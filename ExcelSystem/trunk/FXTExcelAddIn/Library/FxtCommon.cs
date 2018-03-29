using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.International.Converters.PinYinConverter;

namespace FXTExcelAddIn
{
    public class FxtCommon
    {

        public const string Url_DataCenter = "https://data.yungujia.com";
        public const string API_Datacenter = "https://api.fxtcn.com/wdc/dc/active";
        public const string SignName = "70A6A39A-4823-4B94-B834-EA13780FCB34";

        public static string About
        {
            get
            {
                return "房讯通数据团队Excel插件\n" ;
            }
        }
        public static bool IsNumeric(string value)
        {
            if (value.Length <= 0) return false;
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public static string GetPinyinFirst(string str)
        {
            string r = GetPinyin(str);
            if (r.Length == 0) return "";
            return r.Substring(0, 1);
        }
        /// <summary> 
        /// 汉字转化为拼音
        /// </summary> 
        /// <param name="str">汉字</param> 
        /// <returns>全拼</returns> 
        public static string GetPinyin(string str)
        {
            string r = string.Empty;
            if (Dict.PyDict.ContainsKey(str))
            {
                r = Dict.PyDict[str];
            }
            else
            {
                foreach (char obj in str)
                {
                    try
                    {
                        ChineseChar chineseChar = new ChineseChar(obj);
                        //因为一个汉字可能有多个读音，pinyins是一个集合
                        var pinyins = chineseChar.Pinyins;
                        //下面的方法只是简单的获得了集合中第一个非空元素
                        foreach (var pinyin in pinyins)
                        {
                            if (pinyin != null)
                            {
                                //拼音的最后一个字符是音调
                                r += pinyin.Substring(0, pinyin.Length - 1);
                                break;
                            }
                        }
                    }
                    catch
                    {
                        r += obj.ToString();
                    }
                }
            }
            return r;
        }

        /// <summary> 
        /// 汉字转化为拼音首字母
        /// </summary> 
        /// <param name="str">汉字</param> 
        /// <returns>首字母</returns> 
        public static string GetFirstPinyin(string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }
        /// <summary>
        /// 检查时间合法的字符串参数
        /// </summary>
        /// <returns></returns>        
        public static string TimeString(string posts)
        {
            DateTime dt = DateTime.Now;
            //这里改为数组，原来用indexof会引起相似名称的参数冲突 kevin
            string[] tmp = null;
            string tmpstr = posts;
            if (!posts.StartsWith("http://") && !posts.StartsWith("?") && !posts.StartsWith("&"))
            {
                tmpstr = "&" + posts;
            }
            tmp = tmpstr.Split(new char[] { '?', '&' });
            List<string> tmpkey = new List<string>();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i].IndexOf("=") > 0)
                {
                    tmpkey.Add(tmp[i].Split('=')[0]);
                }
            }
            if (!tmpkey.Contains<string>("strdate"))
            {
                posts += "&strdate=" + HttpUtility.UrlEncode(dt.ToString());
            }
            if (!tmpkey.Contains<string>("strcode"))
            {
                posts += "&strcode=" + GetMd5("123" + dt.ToString() + "321");
            }
            //加上当前IP
            posts += "&sourceip=" + GetIPAddress();
            if (posts.StartsWith("http://") && posts.IndexOf("&") > 0 && posts.IndexOf("?") < 0)
            {
                posts = posts.Substring(0, posts.IndexOf("&")) + "?" + posts.Substring(posts.IndexOf("&") + 1); ;
            }
            return posts;
        }

        public static string GetIPAddress()
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
        /// <param name="forwarded">获得IP的参数</param>
        /// <param name="remote"></param>
        /// <returns></returns>
        public static string GetIPAddress(string forwarded, string remote)
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

        public const string WcfLoginMd5Key = "fxtlogin*$^0314";

        /// <summary>
        /// 获得登录Api中的Code,即对时间加密
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string GetLoginCodeMd5(string pwd)
        {
            string strmd5 = pwd + WcfLoginMd5Key;
            strmd5 = GetMd5(strmd5);
            return strmd5;
        }


        /// <summary>
        /// 进行MD5效验
        /// </summary>
        /// <param name="strmd5"></param>
        /// <returns></returns>
        public static string GetMd5(string strmd5)
        {
            byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
            byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            //FileStream fs= new FileStream(filepath,FileMode.Open,FileAccess.Read);
            encodedBytes = md5.ComputeHash(md5Bytes);
            string nn = BitConverter.ToString(encodedBytes);
            nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
            nn = nn.ToLower();//根据需要转化成小写
            //fs.Close();
            return nn;
        }


        /// <summary>
        /// 调用API POST数据，并返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static string APIPostBack(string url, string posts)
        {
            return APIPostBack(url, posts, false, "application/json");
        }
        public static string APIPostBack(string url, string posts, bool check, string contentType)
        {
            //检查参数，登录接口不需要
            if (check)
                posts = TimeString(posts);
            byte[] postData = Encoding.UTF8.GetBytes(posts);
            //找退出原因
            //LogHelper.Info(url + posts);
            WebClient client = new WebClient();

            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            byte[] responseData = null;
            string result = "";
            try
            {
                responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client.Dispose();
            }
            return result;
        }
    }
}
