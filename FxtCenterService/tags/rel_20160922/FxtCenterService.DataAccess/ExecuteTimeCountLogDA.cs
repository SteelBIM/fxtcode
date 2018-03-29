using CAS.Common;
using CAS.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess
{
    public class ExecuteTimeCountLogDA : Base
    {


        public static int Add(ExecuteTimeCountLog model)
        {
            GlobleCache.CenterDBCityTable.Reset();
            return InsertFromEntity<ExecuteTimeCountLog>(model);
        }

        public static int Update(ExecuteTimeCountLog model)
        {
            GlobleCache.CenterDBCityTable.Reset();
            return UpdateFromEntity<ExecuteTimeCountLog>(model);
        }

        public static int Add(DateTime dt)
        {
            string sql = string.Format(@"if exists(select executetime from [FxtLog].[dbo].[ExecuteTimeCountLog] 
	                                        where convert(char(17),executetime,120) = convert(char(17),'{0}',120))
                                          begin
		                                        update [FxtLog].[dbo].[ExecuteTimeCountLog] set total = total+1 
		                                        where convert(char(17),executetime,120) = convert(char(17),'{0}',120)
                                          end
                                        else
                                          begin
	                                        insert into [FxtLog].[dbo].[ExecuteTimeCountLog] (executetime,total) values (getdate(),1) 
                                          end", dt.ToString("yyyy-MM-dd HH:mm:ss"));
            return ExecuteNonQuery(sql);
        }

        public static int Add09(ExecuteTimeCountLog model)
        {
            try
            {
                return 1;
                //string config = "http://api.fxtchina.com/datacentertest/dc/active";//测试服

                //string appid = "1003104";//要调用的接口序列号
                //string apppwd = "261445083";//接口密码
                //string appkey = "2036686022";//加密接口安全属性的key
                //string signname = "AFA911EB-3307-4449-AEB9-3CB3E8BC17BA";//商户标示号
                //09
                //string config = "http://api.fxtchina.com/datacenter/dc/active";//测试服

                //string appid = "1003104";//要调用的接口序列号
                //string apppwd = "261445083";//接口密码
                //string appkey = "2036686022";//加密接口安全属性的key
                //string signname = "AFA911EB-3307-4449-AEB9-3CB3E8BC17BA";//商户标示号

                //本地
                string config = "http://192.168.2.30:9997/dc/active";
                string appid = "1003104";//"1";//要调用的接口序列号
                string apppwd = "261445083";//接口密码   本地
                string appkey = "2036686022";//"gjbcqhf$%2014";//加密接口安全属性的key   本地
                string signname = "60765FEC-9156-409D-923E-A12EB53A1D1F";//"fxtcqhf";//商户标示号   本地

                string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
                string functionname = "addexecutetimecountlog";

                object funinfo = model;

                string[] pwdArray = { appid, apppwd, signname, time, functionname };
                string code = EncryptHelper.GetMd5(pwdArray, appkey);

                var par = new
                {
                    sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname }.ToJson(),
                    info = new
                    {
                        appinfo = new { splatype = "win", stype = "gjb", version = "1.0", vcode = "1", systypecode = "1003301", channel = "360" },//本地
                        uinfo = new { username = "wwj@xaty", token = "" },//本地
                        funinfo = funinfo
                    }.ToJson()
                };
                string txt = par.ToJson();

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(config);
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
                //string content = reader.ReadToEnd();//得到结果
                return 1;

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "写入日志出错ExecuteTimeCountLog");
                throw;
            }
        }
    }
}
