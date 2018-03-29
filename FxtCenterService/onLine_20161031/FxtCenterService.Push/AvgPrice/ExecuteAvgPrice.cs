using CAS.Common;
using CAS.Entity.FxtProject;
using FxtCenterService.Logic;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FxtCenterService.Push.AvgPrice
{
    /// <summary>
    /// 预约执行调度类
    /// </summary>
    public class ExecuteAvgPrice : IJob
    {
        private static string url = "http://113.105.101.36:9080/FrameWeb/weixinCore/weixinInvoke";//ConfigurationManager.AppSettings["BNPushUrl"];
        //正式
        //private static string url = "http://121.43.73.197/FrameWeb/weixinCore/weixinInvoke";//ConfigurationManager.AppSettings["BNPushUrl"];
        
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                //LogHelper.Info("开始推送");

                DateTime dt = DateTime.Now;
                string filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/") + "AvgPriceLog.txt";
                //每个月10号 凌晨00:00
                if (dt.Day == 10)
                {
                    //读取日志文件
                    using (FileStream fsRead = File.Open(filePath, FileMode.OpenOrCreate))
                    {
                        int fsLen = (int)fsRead.Length;
                        byte[] heByte = new byte[fsLen];
                        int r = fsRead.Read(heByte, 0, heByte.Length);
                        string txt = System.Text.Encoding.UTF8.GetString(heByte);//文件内容

                        //判断是否第一次推送

                        char[] charData = null;

                        //判断本月是否已推送
                        if (txt.IndexOf(dt.ToString("yyyy-MM")) == -1)
                        {
                            List<DatAvgPricePush> prices = new List<DatAvgPricePush>();
                            //是否第一次推送
                            //if (txt.IndexOf("FirstFush") == -1)
                            //{
                            //    //推送全部
                            //    prices = DATAvgPriceBL.GetAvgPriceList(null);

                            //    LogHelper.Error("推送全部");

                            //    if (Push(prices))
                            //    {
                            //        charData = "FirstFush\r\n".ToCharArray();

                            //        charData = (dt.ToString("yyyy-MM") + " count:" + prices.Count + "\r\n").ToCharArray();

                            //        LogHelper.Error("推送成功 count:" + prices.Count);

                            //        //写入日志
                            //        var byData = new byte[charData.Length];
                            //        Encoder e = Encoding.UTF8.GetEncoder();
                            //        e.GetBytes(charData, 0, charData.Length, byData, 0, true);
                            //        fsRead.Write(byData, 0, byData.Length);
                            //    }
                            //}
                            //else
                            //{
                            //推送半年
                            //DateTime time = DateTime.Now.AddMonths(-6);
                            //推送最后一个月
                            prices = DATAvgPriceBL.GetAvgPriceLastMonthList();
                            int a = ((prices == null || prices.Count <= 0) ? 0 : prices.Count);
                            LogHelper.Info("推送最后一个月,prices=" + a);

                            if (Push(prices))
                            {
                                charData = (dt.ToString("yyyy-MM") + " count:" + prices.Count + "\r\n").ToCharArray();

                                LogHelper.Info("推送成功 count:" + prices.Count);

                                //写入日志
                                var byData = new byte[charData.Length];
                                Encoder e = Encoding.UTF8.GetEncoder();
                                e.GetBytes(charData, 0, charData.Length, byData, 0, true);
                                fsRead.Write(byData, 0, byData.Length);
                            }
                            //}
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "推送城市均价失败:");
            }
        }

        private bool Push(List<DatAvgPricePush> prices)
        {
            #region 推送
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            string center = "funcNo=1100109&msgbody=" + js.Serialize(new { detailList = prices });

            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)HttpWebRequest.Create(strURL);
            //Post请求方式
            request.Method = "POST";
            // 内容类型
            request.ContentType = "application/x-www-form-urlencoded";

            //这是原始代码：
            string paraUrlCoded = center;
            byte[] payload;
            //将URL编码后的字符串转化为字节
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的 ContentLength 
            request.ContentLength = payload.Length;
            //获得请 求流
            Stream writer = request.GetRequestStream();
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            // 关闭请求流
            writer.Close();
            System.Net.HttpWebResponse response;
            // 获得响应流
            response = (System.Net.HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            LogHelper.Info("content=" + content);
            if (!string.IsNullOrEmpty(content))
            {
                var result = js.Deserialize<AvgPricePushResultData>(content);
                if (result != null && result.message.errorCode == 0)
                {
                    return true;
                }
            }
            return false;

            #endregion
        }
    }

    public class AvgPricePushResultData
    {
        public int messcode { get; set; }
        public AvgPricePushMesscode message { get; set; }
    }

    public class AvgPricePushMesscode
    {
        public int errorCode { get; set; }
        public string errorMsg { get; set; }
    }
}
