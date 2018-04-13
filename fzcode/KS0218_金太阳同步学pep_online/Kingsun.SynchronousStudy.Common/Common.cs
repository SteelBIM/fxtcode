using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Kingsun.SynchronousStudy.Common
{
    public class Common
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strpsw">要加密的字符串</param>
        /// <returns>加密结果</returns>
        public static string pswToSecurity(string strpsw)
        {
            if (strpsw.Length != 0)
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strpsw, "MD5").Substring(8, 16);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 转换为对Excel标题有效的文本
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertToLegalText(string str)
        {
            string tmpStr = str;
            char[] illegalChars = new char[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|' };
            foreach (char item in illegalChars)
            {
                tmpStr = tmpStr.Replace(item, ' ');
            }
            return tmpStr;
        }

        /// <summary>
        /// 小题得分类型
        /// </summary>
        public enum MinScoreType
        {
            /// <summary>
            /// 小题0分制（T012，T013,T022）
            /// </summary>
            Zero = 0,
            /// <summary>
            /// 小题百分制（T001，T002）
            /// </summary>
            Full = 1,
            /// <summary>
            /// 小题平均分制(大部分题型)
            /// </summary>
            Average = 2
        }

        public static string WebRequestPostOrGet(string sUrl, string sParam)
        {
            WriteTxt(sUrl, "Base");


            byte[] bt = System.Text.Encoding.UTF8.GetBytes(sParam);

            Uri uriurl = new Uri(sUrl);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uriurl);//HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + (url.IndexOf("?") > -1 ? "" : "?") + param);
            req.Method = "Post";
            req.Timeout = 120 * 1000;
            req.ContentType = "application/x-www-form-urlencoded;";
            req.ContentLength = bt.Length;

            using (Stream reqStream = req.GetRequestStream())//using 使用可以释放using段内的内存
            {
                reqStream.Write(bt, 0, bt.Length);
                reqStream.Flush();
            }
            try
            {
                using (WebResponse res = req.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理 

                    Stream resStream = res.GetResponseStream();

                    StreamReader resStreamReader = new StreamReader(resStream, System.Text.Encoding.UTF8);

                    string resLine;

                    System.Text.StringBuilder resStringBuilder = new System.Text.StringBuilder();

                    while ((resLine = resStreamReader.ReadLine()) != null)
                    {
                        resStringBuilder.Append(resLine + System.Environment.NewLine);
                    }

                    resStream.Close();
                    resStreamReader.Close();

                    WriteTxt(resStringBuilder.ToString(), "Base");
                    return resStringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;//url错误时候回报错
            }
        }

        /// <summary>
        /// 记录bug，以便调试
        /// </summary>
        public static bool WriteTxt(string str, string type)
        {
            try
            {
                string LogPath = HttpContext.Current.Server.MapPath("/err_log/");
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                FileStream FileStream = new FileStream(System.Web.HttpContext.Current.Server.MapPath("/err_log//vma_" + type + DateTime.Now.ToLongDateString() + "_.txt"), FileMode.Append);
                StreamWriter StreamWriter = new StreamWriter(FileStream);
                //开始写入
                StreamWriter.WriteLine(str);
                StreamWriter.WriteLine("------------------------------------" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //清空缓冲区
                StreamWriter.Flush();
                //关闭流
                StreamWriter.Close();
                FileStream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #region 根据题型获取小题得分类型
        /// <summary>
        /// 根据题型获取小题得分类型
        /// </summary>
        /// <param name="questionType"></param>
        /// <returns></returns>
        public static MinScoreType getMinScoreType(string questionType)
        {
            switch (questionType.Trim())
            {
                case "T001":
                case "T002":
                    return MinScoreType.Full;
                case "T012":
                case "T013":
                case "T022":
                    return MinScoreType.Zero;
                case "T003":
                case "T004":
                case "T005":
                case "T006":
                case "T007":
                case "T008":
                case "T009":
                case "T010":
                case "T011":
                case "T014":
                case "T015":
                case "T016":
                case "T017":
                case "T018":
                case "T019":
                case "T021":
                case "T023":
                case "T024":
                case "T025":
                case "T026":
                case "T027":
                case "T028":
                    return MinScoreType.Average;
                default:
                    return MinScoreType.Average;
            }
        }
        #endregion

        #region "检测是否为有效邮件地址格式"
        /// <summary>
        /// 检测是否为有效邮件地址格式
        /// </summary>
        /// <param name="strIn">输入邮件地址</param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region "检测是否为有效的电话"
        public static bool IsValidPhone(string phone)
        {
            Regex rcode = new Regex(@"^[1][3-8]+\d{9}$|^(0\d{2,3}-?|\(0\d{2,3}\))?[1-9]\d{4,7}(-\d{1,8})?$");
            if (!rcode.IsMatch(phone))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region "检测是否为有效的手机号码"
        public static bool IsValidCellPhone(string phone)
        {
            Regex rcode = new Regex(@"^[1][3-8]+\d{9}$");
            if (!rcode.IsMatch(phone))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 获取文件MD5值
        public static string GetMD5HashFromFile(string fileFullName)
        {
            try
            {
                FileStream file = new FileStream(fileFullName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch
            {
                //throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
            return "";
        }
        #endregion
    }
}
