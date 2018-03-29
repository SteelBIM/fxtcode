using System;
using System.IO;
using System.Net;
using System.Text;

/***********************************************************
 * 功能：用户中心调用类
 *  
 * 创建：魏贝
 * 时间：2015/05
***********************************************************/

namespace FXT.VQ.UserService
{
    /// <summary>
    /// 数据中心调用类
    /// </summary>
    internal static class ServiceHelper
    {
        /// <summary>
        /// 调用API POST数据，并返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        /// <returns></returns>
        internal static string APIPostBack(string posts)
        {
            try
            {
                string url = ConfigSettings.mUserCenterServiceURL;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";
                byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(posts);
                request.ContentLength = postdata.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(postdata, 0, postdata.Length);
                newStream.Close();

                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();//得到结果

                return content;
            }
            catch
            {
                throw;
            }
        }

    }
}
