using System;
using System.Web;
using System.Text.RegularExpressions;
using FXT.DataCenter.Infrastructure.Common.Common;

namespace FXT.DataCenter.Infrastructure.Common.CommonWeb
{
    /// <summary>
    /// ������ز�����
    /// </summary>
    public class RequestHelper
    {
        /// <summary>
        /// ���������ַ���ֵ
        /// </summary>
        /// <param name="name">����</param>
        /// <returns>ֵ</returns>
        public static string GetString(string name)
        {
            if (HttpHelper.CurrentRequest[name] == null)
            {
                return "";
            }
            return HttpHelper.CurrentRequest[name];
        }

        /// <summary>
        /// ��������Intֵ
        /// </summary>
        /// <param name="name">����</param>
        /// <returns>ֵ</returns>
        public static int GetInt(string name)
        {
            return TryParseHelper.StrToInt32(HttpHelper.CurrentRequest[name], 0);
        }

        /// <summary>
        /// ��ò�ѯ�ַ�����ֵ
        /// </summary>
        /// <param name="name">����</param>
        /// <returns>ֵ</returns>
        public static string GetQueryString(string name)
        {
            if (HttpHelper.CurrentRequest.QueryString[name] == null)
            {
                return "";
            }
            return HttpHelper.CurrentRequest.QueryString[name];
        }

        /// <summary>
        /// ��ò�ѯ�ַ����е�Intֵ
        /// </summary>
        /// <param name="name">����</param>
        /// <returns>ֵ��ת��ʧ��ʱ��Ĭ��ֵΪ0��</returns>
        public static int GetQueryInt(string name)
        {
            return GetQueryInt(name, 0);
        }

        /// <summary>
        /// ��ò�ѯ�ַ����е�Intֵ
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="defaultValue">ת��ʧ��ʱ��Ĭ��ֵ</param>
        /// <returns>ֵ</returns>
        public static int GetQueryInt(string name, int defaultValue)
        {
            return TryParseHelper.StrToInt32(HttpHelper.CurrentRequest.QueryString[name], defaultValue);
        }

        /// <summary>
        /// �����վ�ĸ�URL
        /// </summary>
        /// <returns>��վ�ĸ�URL</returns>
        public static string GetBaseUrl()
        {
            if (HttpHelper.CurrentRequest.ApplicationPath == "/")
            {
                return "http://" + RequestHelper.GetServerString("HTTP_HOST");
            }
            return "http://" + RequestHelper.GetServerString("HTTP_HOST") + HttpHelper.CurrentRequest.ApplicationPath;
        }

        /// <summary>
        /// ����Html��Ǻ���������
        /// </summary>
        /// <param name="content">����</param>
        /// <returns>���˺������</returns>
        public static string FilterHtml(string content)
        {
            string text = content.Trim();

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text, "<\\/?[^>]+>", "");	// ȥ�����е�html���
            text = Regex.Replace(text, "[\\s]{2,}", "");	// ���������ϵĿո�
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", "");	//���滻&nbsp;

            return text;
        }

        /// <summary>
        /// �滻Html��Ǻ���������
        /// </summary>
        /// <param name="content">����</param>
        /// <returns>�滻�������</returns>
        public static string ReplaceHtml(string content)
        {
            string text = content.Trim();

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text, "[\\s]{2,}", " ");	// ���������ϵĿո�
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	// �滻<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//���滻&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags

            return text;
        }

        /// <summary>
        /// ����ָ���ķ�����������Ϣ
        /// </summary>
        /// <param name="name">������������</param>
        /// <returns>������������Ϣ</returns>
        public static string GetServerString(string name)
        {
            if (HttpHelper.CurrentRequest.ServerVariables[name] == null)
            {
                return "";
            }
            return HttpHelper.CurrentRequest.ServerVariables[name].ToString();
        }

        /// <summary>
        /// �ж��Ƿ�����������������
        /// </summary>
        /// <returns>�Ƿ�����������������</returns>
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
        /// ��õ�ǰҳ�������
        /// </summary>
        /// <returns>��ǰҳ�������</returns>
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
        /// ��õ�ǰҳ��ͻ��˵�IP
        /// </summary>
        /// <returns>��ǰҳ��ͻ��˵�IP</returns>
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
        /// ȡ�ÿͻ�����ʵIP������д�����ȡ��һ����������ַ
        /// </summary>
        /// <param name="forwarded"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        private static string GetIPAddress(string forwarded, string remote)
        {
            //forwarded�� HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
            //remote �� HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
            string result = String.Empty;
            result = forwarded;
            if (result != null && result != String.Empty)
            {
                //�����д���  
                if (result.IndexOf(".") == -1)        //û�С�.���϶��Ƿ�IPv4��ʽ  
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //�С�,�������ƶ������ȡ��һ������������IP��  
                        result = result.Replace("  ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];        //�ҵ����������ĵ�ַ  
                            }
                        }
                    }
                    else if (IsIPAddress(result))  //������IP��ʽ  
                        return result;
                    else
                        result = null;        //�����е�����  ��IP��ȡIP  
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
        /// �ж��Ƿ���IP��ַ��ʽ 0.0.0.0
        /// </summary>
        /// <param name="str1">���жϵ�IP��ַ</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);

            return regex.IsMatch(str1);
        }

        /// <summary>
        /// �Ƿ�Ϊip
        /// </summary>
        /// <param name="ip">Ҫ��֤��IP��ַ</param>
        /// <returns>boolֵ</returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }

        /// <summary>
        /// �����û��ϴ����ļ�
        /// </summary>
        /// <param name="path">����·��</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpHelper.CurrentRequest.Files.Count > 0)
            {
                HttpHelper.CurrentRequest.Files[0].SaveAs(path);
            }
        }
        #region ��ȡ�ͻ�����������ͼ��汾��
        /// <summary>
        /// ��ȡ�ͻ�����������ͼ��汾��
        /// </summary>
        /// <returns></returns>
        public static string GetClientBrowserVersions()
        {
            string browserVersions = string.Empty;
            HttpBrowserCapabilities hbc = HttpContext.Current.Request.Browser;
            string browserType = hbc.Browser.ToString();     //��ȡ���������
            string browserVersion = hbc.Version.ToString();    //��ȡ�汾��
            browserVersions = browserType + "_" + browserVersion;
            return browserVersions;
        }
        #endregion
        #region 32λΨһ��ʶ��
        /// <summary>
        /// ��¼
        ///32λΨһ��ʶ��
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
