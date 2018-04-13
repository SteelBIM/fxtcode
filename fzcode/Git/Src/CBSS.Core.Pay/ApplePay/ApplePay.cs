using CBSS.Core.Log;
using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Core.Pay.ApplePay
{
    public class ApplePay
    {
        static string filepath = "Config/PayConfig.xml";
        private static string _applePaySandboxUrl = XMLHelper.GetAppSetting(filepath, "ApplePay", "ApplePaySandboxUrl");

        public static string RequestAppleService(string strCorona, string url, int isSanBox)
        {
            try
            {
                string postDataStr = strCorona;
                //提交服务器
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postDataStr.Length;

                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                if (!string.IsNullOrEmpty(retString))
                {
                    Dictionary<string, object> json = (Dictionary<string, object>)JsonConvertHelper.ToJson(retString);
                    object value;

                    json.TryGetValue("status", out value);
                    if (value.ToString() == "0")
                    {
                        object receipt;
                        json.TryGetValue("receipt", out receipt);
                        Dictionary<string, object> receiptData = (Dictionary<string, object>)receipt;

                        string bundle_id = receiptData["bundle_id"].ToString();

                        if (bundle_id.Contains("com.kingsunsoft"))
                        {
                            var obj = new { Success = true, isSanBox = isSanBox };
                            return JsonConvertHelper.ToJson(obj);
                        }
                    }
                    else if (value.ToString() == "21007")
                    {
                        if (!url.Contains("sandbox"))
                        {
                            isSanBox = 1;
                            url = _applePaySandboxUrl;
                            return RequestAppleService(strCorona, url, 1);
                        }
                        else
                        {
                            var obj = new { Success = false, isSanBox = isSanBox };
                            return JsonConvertHelper.ToJson(obj);
                        }
                    }
                    else
                    {
                        var obj = new { Success = false, isSanBox = isSanBox };
                        return JsonConvertHelper.ToJson(obj);
                    }
                }
                var obj1 = new { Success = false, isSanBox = isSanBox };
                return JsonConvertHelper.ToJson(obj1);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "苹果订单插入异常", ex);
                var obj = new { Success = false, isSanBox = isSanBox };
                return JsonConvertHelper.ToJson(obj);
            }
        }
    }
}
