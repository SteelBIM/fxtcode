using CAS.Common;
using CAS.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess
{
    public class ExecuteTimeLogDA : Base
    {
        public static int Add(ExecuteTimeLog model)
        {
            GlobleCache.CenterDBCityTable.Reset();

            return InsertFromEntity<ExecuteTimeLog>(model);
        }

        public static int Add09(ExecuteTimeLog model)
        {
            try
            {
                //因为性能问题，暂时注释实现
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
            string functionname = "addexecutetimelog";

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
                LogHelper.Error(ex, "写入日志出错ExecuteTimeLog");
                throw;
            }

//            string sql = @"INSERT INTO [FxtLog].[dbo].[ExecuteTimeLog]
//           ([FunctionName]
//           ,[UserCenterTime]
//           ,[CityAuthorityTime]
//           ,[OverFlowTime]
//           ,[GetDataTime]
//           ,[TotalTime]
//           ,[SqlTime]
//           ,[Ident]
//           ,[Time]
//           ,[AddTime]
//           ,[RequestParam]
//           ,[ServerId]
//           ,[Code]
//           ,StartTime)
//     VALUES
//           (@FunctionName,
//            @UserCenterTime,
//            @CityAuthorityTime,
//            @OverFlowTime,
//            @GetDataTime,
//            @TotalTime,
//            @SqlTime,
//            @Ident,
//            @Time,
//            @AddTime,
//            @RequestParam,
//            @ServerId,
//            @Code,
//            @StartTime
//           )";

//            SqlCommand com = new SqlCommand();

//            com.CommandText = sql;

//            com.Parameters.Add(new SqlParameter() { ParameterName = "@FunctionName", Value = model.functionname });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@UserCenterTime", Value = model.usercentertime });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@CityAuthorityTime", Value = model.cityauthoritytime });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@OverFlowTime", Value = model.overflowtime });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@GetDataTime", Value = model.getdatatime });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@TotalTime", Value = model.totaltime });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@SqlTime", Value = model.sqltime });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@Ident", Value = model.ident });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@Time", Value = model.time });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@AddTime", Value = model.addtime });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@RequestParam", Value = model.requestparam });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@ServerId", Value = model.serverid });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@Code", Value = model.code });
//            com.Parameters.Add(new SqlParameter() { ParameterName = "@StartTime", Value = model.starttime });

//            return ExecuteNonQuery09(com, false);

        }

        //public static Int32 ExecuteNonQuery09(SqlCommand command, bool includeReturnValueParameter)
        //{
        //    string connectionString = "Data Source =202.105.131.244\fxtdb;DataBase = FXTProject;User Id = data_it;Password =it301data.com;Connect Timeout=60";
        //    using (SqlConnection cn = new SqlConnection(connectionString))
        //    {
        //        cn.Open();
        //        command.Connection = cn;
        //        command.CommandTimeout = 120;
        //        int returnValue;
        //        returnValue = command.ExecuteNonQuery();
        //        cn.Close();
        //        if (includeReturnValueParameter)
        //        {
        //            return int.Parse(command.Parameters[RETURN_VALUE_PARAMETER_NAME].Value.ToString());
        //        }
        //        else
        //        {
        //            return returnValue;
        //        }
        //    }
        //}
    }
}
