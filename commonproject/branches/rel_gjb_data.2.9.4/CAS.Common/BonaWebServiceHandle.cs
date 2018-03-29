using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
//using CAS.Entity.BonaServiceEntity;
using System.Reflection;

namespace CAS.Common
{
    /// <summary>
    /// 博纳服务处理类
    /// </summary>
    public static class BonaWebServiceHandle
    {
        #region 私用方法
        private static string GetParameter(Dictionary<string, string> dic)
        {
            StringBuilder strb = new StringBuilder();

            foreach (var key in dic.Keys)
            {
                strb.AppendFormat("&{0}={1}", key, dic[key]);
            }

            return strb.ToString().Substring(1);
        }

        //取配置文件appsetting值
        private static string GetConfigSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        #endregion

        /// <summary>
        /// post远程服务
        /// </summary>
        /// <param name="param">键值对参数</param>
        /// <returns>Json</returns>
        public static string WebServiceApiMethodOfPost(Dictionary<string, string> param,string Configbonaip = "bonaip", string ConfigApp = "bonadata")
        {
            string result = string.Empty;
            string paramStr = GetParameter(param);

            try
            {
                byte[] bs = Encoding.UTF8.GetBytes(paramStr);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(GetConfigSetting(Configbonaip) + GetConfigSetting(ConfigApp));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                request.ContentLength = bs.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);

                }
                using (WebResponse wr = request.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理
                    StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    result = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;

        }

        /// <summary>
        /// 返回结果实体
        /// </summary>
        /// <typeparam name="T">RowDataType</typeparam>
        /// <param name="json">源json</param>
        /// <returns></returns>
        public static T GetReturnDataByJson<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

        }

        /// <summary>
        /// 实体转条件参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetParamEntity(object entity)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();

            var type = entity.GetType();
            PropertyInfo[] propertyInfo = type.GetProperties();

            for (int i = 0; i < propertyInfo.Length; i++)
            {
                PropertyInfo info = propertyInfo[i];
                string value = (string)info.GetValue(entity, null);

                param.Add(info.Name, value);
            }


            return param;
        }


        /// <summary>
        /// 上传文件到bona服务器
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string UploadBonaService(string filePath)
        {
            WebClient webClient = new WebClient();
            webClient.QueryString["Weixin"] = filePath;
            //webClient.Headers.Add("Content-Type", "multipart/form-data;boundary=" + DateTime.Now.Ticks.ToString("x"));//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webClient.UploadFile(GetConfigSetting("bonaip") + GetConfigSetting("bonauploadpath"), "POST", filePath);
            //byte[] responseData = webClient.UploadData(GetConfigSetting("bonauploadpath"), bs);
            //byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  

            string result = Encoding.UTF8.GetString(responseData);//解码  
            LogHelper.Info("返回值://" + result.ToJson());
            return result;
        }
    }
}
