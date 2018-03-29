using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtCenterService.Contract;
using System.ServiceModel.Activation;
using CAS.Entity;
using CAS.Common;
using System.Xml;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using FxtCenterService.Actualize;
using System.Web;
using OpenPlatform.Framework.FlowMonitor;
using FxtCenterService.Logic;
//using FxtOpenClient;
using CAS.Entity.FxtUserCenter;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace FxtCenterService.Actualize
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataCenterService : IDataCenterService
    {
        public static bool bRPCInitState = false;
        private static int count = 0;
        private static string mtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        private static string serverid = HttpContext.Current.Request.ServerVariables.Get("Local_Addr").ToString();
        private static object o = new object();
        public WCFJsonData Entrance(string sinfo, string info)
        {
            DateTime starttime = DateTime.Now;
            string serialno = string.Empty;//唯一标示
            int oldFxtcompanyid = 0;
            int sysTypeCode = 0;
            Guid guid = Guid.NewGuid();
            int userCenterTime = 0;//用户中心验证执行时间
            int cityTime = 0;//城市权限验证执行时间
            int overflowTime = 0;//流量控制执行时间
            int getDataTime = 0;//查询时间执行时间
            string time = string.Empty;
            ExecuteTimeLog returnlog = new ExecuteTimeLog();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            WCFJsonData jsonData = new WCFJsonData();
            UserCheck company = new UserCheck();
            string funtionname = string.Empty;
            try
            {
                //LogHelper.Info(info);
                JObject objSinfo = JObject.Parse(sinfo);
                JObject objinfo = JObject.Parse(info);

                JObject funinfo = objinfo["funinfo"] as JObject;
                if (funinfo == null)
                {
                    funinfo = new JObject();
                }
                JObject appinfo = objinfo["appinfo"] as JObject;
                JObject uinfo = objinfo["uinfo"] as JObject;
                funtionname = objSinfo.Value<string>("functionname");
                if (funinfo.GetValue("serialno") == null)
                {
                    funinfo.Add(new JProperty("serialno", Guid.NewGuid().ToString()));
                }
                serialno = funinfo.Value<string>("serialno");

                lock (o)
                {
                    DateTime mmtime = DateTime.Now;
                    if (mtime == mmtime.ToString("yyyy-MM-dd HH:mm"))
                    {
                        count++;
                    }
                    else
                    {
                        count++;
                        if (funtionname != "addexecutetimelog" && funtionname != "addexecutetimecountlog")
                        {
                            new Task(AddExecuteTimeCountLog, new ExecuteTimeCountLog()
                            {
                                executetime = mmtime,
                                total = count,
                                serverid = serverid
                            }).Start();
                        }

                        count = 0;
                        mtime = mmtime.ToString("yyyy-MM-dd HH:mm");
                    }
                }

                time = objSinfo.Value<string>("time");//调用者传过来的执行时间

                string appid = objSinfo.Value<string>("appid");
                //SearchBase search = FxtGjbApiCommon.InitSearBase(funinfo);//初始化SearchBase
                string returntext = string.Empty;
                int returntype = 0;

                #region 安全验证
                if (!string.IsNullOrEmpty(appid) || !string.IsNullOrEmpty(objSinfo.Value<string>("apppwd"))
                    || DataCenterCommon.VerifyAppidAndFunctionnanmeIsMatch(appid, funtionname))//appi,与functionname不包含返回
                {
                    string securityJson = WebCommon.ApiSecurityStringOfJson(objSinfo, funtionname, objinfo);
                    //LogHelper.Info(securityJson);
                    // FxtGjbApiCommon.WcfApiMethodOfPost(Constant.WCFUserCenterService, securityJson);

                    Stopwatch swcompany = new Stopwatch();
                    swcompany.Start();

                    try
                    {
                        string signnamekey = "GetCompanyBySignName_" + objSinfo.Value<string>("signname");
                        company = CacheHelper.Get<UserCheck>(signnamekey);
                        LogHelper.SaveLog("安全验证");
                        if (company == null)
                        {
                            LogHelper.SaveLog("安全验证company开始");
                            company = WebCommon.FxtUserCenterService_GetCompanyBySignName(securityJson, out returntext, out returntype);
                            LogHelper.SaveLog(securityJson);
                            LogHelper.SaveLog("returntext=" + returntext);
                            LogHelper.SaveLog("安全验证company结束");
                            CacheHelper.Set(signnamekey, company, 2);
                            LogHelper.SaveLog(securityJson);
                        }
                        else {
                            returntype = 1;
                        }
                        swcompany.Stop();
                        TimeSpan ts2 = swcompany.Elapsed;
                        //LogHelper.Info(funtionname + "用户中心验证执行时间：" + ts2.TotalMilliseconds + "ms.Guid:" + guid);
                        userCenterTime = (int)ts2.TotalMilliseconds;
                    }
                    catch (Exception ex)
                    {
                        swcompany.Stop();
                        userCenterTime = -1;
                        throw ex;
                    }

                    //LogHelper.Info(company.ToJson());
                    if (company != null && returntype == 1)
                    {
                        LogHelper.SaveLog("安全验证companyreturntype");
                        //if (( funtionname == "projectdropdownlistmcas" || funtionname == "buildingbaseinfolistmcas" || funtionname == "housedropdownlistmcas" || funtionname == "gpdinfo"))
                        //{
                        //    LogHelper.Info(string.Format("sinfo:{0}\r\ninfo:{1}\r\n", sinfo, info));
                        //}

                        company.username = uinfo.Value<string>("username");
                        company.producttypecode = appinfo.Value<int>("systypecode");

                        oldFxtcompanyid = company.companyid;
                        sysTypeCode = company.producttypecode;
                        #region 根据cityzipcode关联城市
                        if (funtionname.ToLower().Equals("projectlistforzipcode") || funtionname.ToLower().Equals("caselistbyareaanduse"))
                        {
                            if (funinfo.Property("cityzipcode") != null && !string.IsNullOrWhiteSpace(funinfo.Value<string>("cityzipcode")))
                            {
                                int cityzipcode;
                                int.TryParse(funinfo.Value<string>("cityzipcode"),out cityzipcode);
                                var citylist = FxtCenterService.DataAccess.SYSAreaDA.GetSYSCityList(0, cityzipcode);
                                if (citylist.Count > 0 && funinfo.Property("cityid") == null)
                                {
                                    funinfo.Add("cityid", citylist.FirstOrDefault().cityid);
                                }
                                else if (citylist.Count > 0 && funinfo.Property("cityid") != null)
                                {
                                    funinfo.Property("cityid").Value = citylist.FirstOrDefault().cityid;
                                }
                            }
                        }
                        #endregion
                        //公司产品城市权限验证
                        int cityId = 0;
                        if (int.TryParse(funinfo.Value<string>("cityid"), out cityId))
                        {
                            //LogHelper.Info(funtionname + ":" + company.companyid + ":" + company.producttypecode + ":" + cityId);
                            Stopwatch swcp = new Stopwatch();
                            swcp.Start();
                            CompanyProduct cp = CompanyProductModuleBL.IsCompanyProductCity(company.companyid, company.producttypecode, cityId, objSinfo, objinfo);

                            swcp.Stop();
                            //LogHelper.Info(funtionname + "城市权限验证执行时间：" + ts2.TotalMilliseconds + "ms.Guid:" + guid);
                            cityTime = (int)swcp.Elapsed.TotalMilliseconds;

                            if (cp == null && funtionname != "garealist")
                            {
                                jsonData.data = "";
                                jsonData.returntype = 6;
                                jsonData.returntext = "无城市权限！";
                                return jsonData;
                            }
                            else
                            {
                                company.parentproducttypecode = (cp != null && cp.parentproducttypecode.HasValue) ? cp.parentproducttypecode.Value : company.producttypecode;//company.producttypecode;
                                company.parentshowdatacompanyid = (cp != null && cp.parentshowdatacompanyid.HasValue) ? cp.parentshowdatacompanyid.Value : company.companyid;// company.companyid;
                            }
                        }
                        else
                        {
                            company.parentproducttypecode = company.producttypecode;
                            company.parentshowdatacompanyid = company.companyid;
                        }

                        LogHelper.SaveLog("truename=" + company.truename + ",companyid=" + company.companyid + ",parentproducttypecode=" + company.parentproducttypecode + ",parentshowdatacompanyid=" + company.parentshowdatacompanyid);

                        if (!string.IsNullOrEmpty(funtionname))
                        {
                            funtionname = funtionname.Trim();
                            MethodInfo methInfo = DataCenterCommon.GetMethodInfo(funtionname);
                            ParameterInfo[] parameterInfo = methInfo.GetParameters();
                            object[] objParamet;
                            if (parameterInfo.Length > 2)
                            {
                                objParamet = new object[] { funinfo, company, objSinfo, objinfo };
                            }
                            else
                            {
                                objParamet = new object[] { funinfo, company };
                            }
                            //funinfo.Add(new JProperty("serialno", serialno));
                            
                            OverflowAttrbute[] attributes = methInfo.GetCustomAttributes(typeof(OverflowAttrbute), true) as OverflowAttrbute[];
                            //流量控制 
                            if (attributes.Count() > 0)
                            {
                                //int fxtCompanyId = company.companyid;
                                int fxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;

                                Stopwatch swOverflow = new Stopwatch();
                                swOverflow.Start();

                                var isOverflow = Api.Flow.Overflow(fxtCompanyId, DateTime.Now, attributes[0].Type, -1, functionName: funtionname, productTypeCode: company.producttypecode);

                                swOverflow.Stop();
                                //LogHelper.Info(funtionname + "流量控制执行时间：" + ts2.TotalMilliseconds + "ms.Guid:" + guid);
                                overflowTime = (int)swOverflow.Elapsed.TotalMilliseconds;

                                if (isOverflow)
                                {
                                    jsonData.data = "";
                                    jsonData.returntype = 7;
                                    jsonData.returntext = "流量溢出";
                                    return jsonData;
                                }
                            }

                            Stopwatch swInvoke = new Stopwatch();
                            swInvoke.Start();
                            var obj = methInfo.Invoke(null, objParamet);
                            swInvoke.Stop();
                            TimeSpan tsInvoke = swInvoke.Elapsed;
                            getDataTime = (int)tsInvoke.TotalMilliseconds;

                            if (methInfo.ReturnType == typeof(string))
                            {
                                jsonData.data = obj as string;

                                if (obj == null)
                                {
                                    jsonData.returntype = -1;
                                    jsonData.returntext = "获取数据失败";
                                }
                            }
                            else if (methInfo.ReturnType == typeof(JsonDataContainExcuteTimeLog))
                            {
                                JsonDataContainExcuteTimeLog jsoncontainlogobj = obj as JsonDataContainExcuteTimeLog;
                                if (jsoncontainlogobj != null)
                                {
                                    jsonData.data = jsoncontainlogobj.jsondata;
                                    returnlog = jsoncontainlogobj.executetimeLog;
                                }
                                if (obj == null || string.IsNullOrWhiteSpace(jsonData.data))
                                {
                                    jsonData.returntype = -1;
                                    jsonData.returntext = "获取数据失败";
                                }
                            }
                            else
                            {
                                if (obj == null)
                                {
                                    jsonData.returntype = -1;
                                    jsonData.returntext = "获取数据失败";
                                }
                            }                            
                        }
                        else
                        {
                            jsonData.returntype = returntype;
                            jsonData.returntext = returntext;
                        }
                        //if ((funtionname == "projectdropdownlistmcas" || funtionname == "buildingbaseinfolistmcas" || funtionname == "housedropdownlistmcas" || funtionname == "gphnafvqe"))
                        //{

                        
                    }
                    else
                    {
                        LogHelper.SaveLog("returntext2=" + returntext);

                        swcompany.Stop();
                        userCenterTime = -2;

                        jsonData.returntype = returntype;
                        jsonData.returntext = returntext;
                    }
                }
                else
                {
                    LogHelper.SaveLog("returntext=" + returntext);
#if DEBUG
                    jsonData.returntype = returntype;
                    jsonData.returntext = returntext;
#else
                    jsonData.returntype = returntype;
                    jsonData.returntext = returntext;
#endif
                }

                sw.Stop();
                TimeSpan ts3 = sw.Elapsed;

                var tatal = (int)ts3.TotalMilliseconds;

                //if (tatal > 2000)
                //{
                //LogHelper.Info(string.Format("sinfo:{0}\r\ninfo:{1}\r\n{2}Guid:{3}", sinfo, info, funtionname + "执行时间：" + tatal + "ms.", guid));
                string requstParam = string.Format("sinfo:{0}\r\ninfo:{1}\r\n", sinfo, info);

                if (funtionname == "projectdropdownlistmcassdk" || funtionname == "buildingbaseinfolistmcas" || funtionname == "housefloorlistmcas" || funtionname == "housedropdownlistmcas")
                {
                    Task.Run(() => ExecuteTimeLogBL.AddListMySql(new ExecuteTimeLog()
                    {
                        functionname = funtionname,
                        usercentertime = returnlog.usercentertime,
                        cityauthoritytime = returnlog.cityauthoritytime,
                        overflowtime = returnlog.overflowtime,
                        getdatatime = getDataTime,
                        totaltime = tatal,
                        sqltime = returnlog.sqltime,
                        sqlexecutetime = returnlog.sqlexecutetime,
                        ident = guid,
                        requestparam = returnlog.requestparam,
                        addtime = DateTime.Now,
                        code = serialno,
                        serverid = serverid,
                        starttime = starttime,
                        fxtcompanyid = oldFxtcompanyid,
                        systypecode = sysTypeCode,
                        remark = requstParam
                    }));


                    //Task.Run(() => ExecuteTimeLogBL.AddListMySql(new ExecuteTimeLog()
                    //{
                    //    functionname = funtionname,
                    //    usercentertime = userCenterTime,
                    //    cityauthoritytime = cityTime,
                    //    overflowtime = overflowTime,
                    //    getdatatime = getDataTime,
                    //    totaltime = tatal,
                    //    sqltime=returnlog.sqltime,
                    //    sqlexecutetime=returnlog.sqlexecutetime,
                    //    ident = guid,
                    //    time = returnlog.time,
                    //    addtime = DateTime.Now,
                    //    code = serialno,
                    //    serverid = serverid,
                    //    starttime = starttime,
                    //    fxtcompanyid = oldFxtcompanyid,
                    //    systypecode = sysTypeCode
                    //}));
                }
                else if (funtionname != "addexecutetimelog" && funtionname != "addexecutetimecountlog")
                {
                    var model = new ExecuteTimeLog()
                    {
                        functionname = funtionname,
                        usercentertime = userCenterTime,
                        cityauthoritytime = cityTime,
                        overflowtime = overflowTime,
                        getdatatime = getDataTime,
                        totaltime = tatal,
                        ident = guid,
                        time = "",
                        addtime = DateTime.Now,
                        requestparam = requstParam,
                        code = serialno,
                        serverid = serverid,
                        starttime = starttime,
                        fxtcompanyid = oldFxtcompanyid,
                        systypecode = sysTypeCode
                    };

                    //.net framework 4.0
                    //Task.Factory.StartNew(AddExecuteTimeLog, model);
                    //.net framework 4.5
                    Task.Run(() => AddExecuteTimeLog(model));

                    if (tatal > 3000 && (funtionname == "projectdropdownlistmcas" || funtionname == "buildingbaseinfolistmcas" || funtionname == "housedropdownlistmcas" || funtionname == "gphnafvqe")
                        && oldFxtcompanyid == 6 && sysTypeCode == 1003038)
                    {
                        //new Task(sendEmail, model).Start();
                        Task.Run(() => sendEmail(model));
                    }
                }

                #endregion
                if (funtionname == "caselistbyareaanduse")
                {
                    Task.Run(() => { LogHelper.Info("{\"sinfo\":\"" + sinfo.Replace("\"", "\\\"") + "\",\"info\":\"" + info.Replace("\"", "\\\"") + "\"}"); });
                }
                return jsonData;
            }
            catch (Exception ex)
            {
                sw.Stop();
                TimeSpan ts2 = sw.Elapsed;
                var tatal = (int)ts2.TotalMilliseconds;

                //if (tatal > 2000)
                //{
                //LogHelper.Info(string.Format("sinfo:{0}\r\ninfo:{1}\r\n{2}Guid:{3}", sinfo, info, funtionname + "执行时间：" + tatal + "ms.", guid));
                string requstParam = string.Format("sinfo:{0}\r\ninfo:{1}\r\n", sinfo, info);

                if (funtionname != "addexecutetimelog" && funtionname != "addexecutetimecountlog")
                {

                    var model = new ExecuteTimeLog()
                    {
                        functionname = funtionname,
                        usercentertime = userCenterTime,
                        cityauthoritytime = cityTime,
                        overflowtime = overflowTime,
                        getdatatime = getDataTime,
                        totaltime = tatal,
                        ident = guid,
                        time = "error",
                        addtime = DateTime.Now,
                        requestparam = requstParam,
                        code = serialno,
                        serverid = serverid,
                        starttime = starttime,
                        fxtcompanyid = oldFxtcompanyid,
                        systypecode = sysTypeCode
                    };

                    Task.Run(() => AddExecuteTimeLog(model));
                    //new Task(AddExecuteTimeLog, model).Start();

                    if (tatal > 3000 && (funtionname == "projectdropdownlistmcas" || funtionname == "buildingbaseinfolistmcas" || funtionname == "housedropdownlistmcas" || funtionname == "gphnafvqe"))
                    {
                        Task.Run(() => sendEmail(model));
                        //new Task(sendEmail, model).Start();
                    }
                }


                ex.Source += string.Format("sinfo:{0}\r\ninfo:{1}\r\n", sinfo, info);
                LogHelper.Error(ex, funtionname + "执行时间：" + tatal + "ms.");
                jsonData.data = "";
                jsonData.returntext = "";
                jsonData.returntype = -1;
                return jsonData;
            }
        }

        private void AddExecuteTimeLog(object olog)
        {
            ExecuteTimeLog log = olog as ExecuteTimeLog;
            ExecuteTimeLogBL.Add09(log);
        }

        private void AddExecuteTimeCountLog(object olog)
        {
            ExecuteTimeCountLog log = olog as ExecuteTimeCountLog;
            ExecuteTimeCountLogBL.Add09(log);
        }

        private void sendEmail(object olog)
        {
            ExecuteTimeLog log = olog as ExecuteTimeLog;

            string titel = "招行自动估价SDK响应时间" + log.totaltime + "毫秒";
            string content = string.Format("方法名：{0}，用户中心执行时间：{1}ms，城市权限验证执行时间：{2}ms，流量控制执行时间：{3}ms，方法执行时间：{4}ms，总用时：{5}ms,唯一标示：{6}",
                log.functionname, log.usercentertime, log.cityauthoritytime, log.overflowtime, log.getdatatime, log.totaltime,log.code);
            //MailHelper.SendEmail("‍tanl@fxtcn.com", "service@fxtcn.com", titel, content, "service@fxtcn.com", "FxtCn123", "smtp.fxtcn.com", 25);
            //DataCenterService.SendEmail("‍tanql@fxtcn.com", "service@fxtcn.com", "123", "123", "service@fxtcn.com", "FxtCn123", "smtp.fxtcn.com", 25);

            try
            {
                string sendmail = "service@fxtcn.com",
    mailaccount = "service@fxtcn.com",
    mailpassword = "FxtCn123",
    smtpserver = "smtp.fxtcn.com";
                int smtpport = 25;

                MailMessage mailObj = new MailMessage();
                mailObj.From = new MailAddress(mailaccount);//"发送邮箱地址"
                mailObj.To.Add("cmbsdk@fxtcn.com");//"接收邮箱地址"
                mailObj.Subject = titel;// "主题"
                mailObj.Body = content;// "内容"
                mailObj.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpserver;// "邮件服务器地址"
                smtp.Port = smtpport;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(mailaccount, mailpassword);//"登录用户名","登录密码"
                smtp.Send(mailObj);
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="SendTo">收送人的地址</param>
        /// <param name="EmailAddress">我的Email地址</param>
        /// <param name="SendTitle">发送标题</param>
        /// <param name="SendBody">发送的内容</param>
        /// <param name="UserName">我的Email登录名</param>
        /// <param name="Password">我的Email密码</param>
        /// <param name="SmtpServer">Smtp邮件服务器</param>
        /// <param name="SmtpServerPort">Smtp邮件服务器端口</param>
        public static bool SendEmail(string SendTo, string EmailAddress, string SendTitle, string SendBody, string UserName, string Password, string SmtpServer, int SmtpServerPort)
        {
            try
            {

                string sendmail = "service@fxtcn.com",
                                 mailaccount = "service@fxtcn.com",
                 mailpassword = "FxtCn123",
                 smtpserver ="smtp.fxtcn.com";
                int smtpport = 25;
                MailMessage mailObj = new MailMessage();
                mailObj.From = new MailAddress(mailaccount);//"发送邮箱地址"
                mailObj.To.Add("tanql@fxtcn.com");//"接收邮箱地址"
                mailObj.Subject = SendTitle;// "主题"
                mailObj.Body = SendBody;// "内容"
                mailObj.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpserver;// "邮件服务器地址"
                smtp.Port = smtpport;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(mailaccount, mailpassword);//"登录用户名","登录密码"
                smtp.Send(mailObj);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public WCFJsonData UpLoadFile(System.IO.Stream stream, string sinfo, string info)
        {
            WCFJsonData jsonData = new WCFJsonData();
            try
            {
                //LogHelper.Info(info);
                sinfo = HttpUtility.UrlDecode(sinfo).Replace("%20", "+");
                info = HttpUtility.UrlDecode(info).Replace("%20", "+");
                JObject objSinfo = JObject.Parse(sinfo);
                JObject objinfo = JObject.Parse(info);
                JObject funinfo = objinfo["funinfo"] as JObject;
                JObject appinfo = objinfo["appinfo"] as JObject;
                JObject uinfo = objinfo["uinfo"] as JObject;
                string funtionname = objSinfo.Value<string>("functionname");
                string appid = objSinfo.Value<string>("appid");
                //SearchBase search = FxtGjbApiCommon.InitSearBase(funinfo);//初始化SearchBase
                string returntext = string.Empty;
                int returntype = 0;
                UserCheck company = new UserCheck();

                #region 安全验证
                if (!string.IsNullOrEmpty(appid) || !string.IsNullOrEmpty(objSinfo.Value<string>("apppwd"))
                    || DataCenterCommon.VerifyAppidAndFunctionnanmeIsMatch(appid, funtionname))//appi,与functionname不包含返回
                {
                    string securityJson = WebCommon.ApiSecurityStringOfJson(objSinfo, funtionname, objinfo);
                    //LogHelper.Info(securityJson);
                    // FxtGjbApiCommon.WcfApiMethodOfPost(Constant.WCFUserCenterService, securityJson);
                    company = WebCommon.FxtUserCenterService_GetCompanyBySignName(securityJson, out returntext, out returntype);
                    //LogHelper.Info(company.ToJson());
                    if (company != null && returntype == 1)
                    {
                        company.username = uinfo.Value<string>("username");
                        if (!string.IsNullOrEmpty(funtionname))
                        {
                            funtionname = funtionname.Trim();
                            MethodInfo methInfo = DataCenterCommon.GetMethodInfo(funtionname);
                            ParameterInfo[] parameterInfo = methInfo.GetParameters();
                            object[] objParamet = { stream, funinfo, company };
                            jsonData.data = methInfo.Invoke(null, objParamet) as string;
                        }
                        else
                        {
                            jsonData.returntype = returntype;
                            jsonData.returntext = returntext;
                        }
                    }
                    else
                    {
                        jsonData.returntype = returntype;
                        jsonData.returntext = returntext;
                    }
                }
                else
                {
#if DEBUG
                    jsonData.returntype = returntype;
                    jsonData.returntext = returntext;
#else
                    jsonData.returntype = returntype;
                    jsonData.returntext = returntext;
#endif
                }
                #endregion

                //LogHelper.Info(jsonData.data);
                //LogHelper.Info(jsonData.returntype.ToString());
                //LogHelper.Info(jsonData.returntext);

                return jsonData;
            }
            catch (Exception ex)
            {
                ex.Source += string.Format("sinfo:{0}\r\ninfo:{0}\r\n", sinfo, info);
                LogHelper.Error(ex + "----" + string.Format("sinfo:{0}\r\ninfo:{0}\r\n", sinfo, info));
                jsonData.data = "";
                jsonData.returntext = "errorjsonData";
                jsonData.returntype = -1;
                return jsonData;
            }
        }
        ///// <summary>
        ///// 读取SQL语句，注意文件名和传入的参数大小写要匹配
        ///// SQL文件必须修改为“嵌入的资源”
        ///// 如果不用嵌入也可以使用文件方式读取，好处是应用程序不会重启，但明文存放可能引起容易被篡改等安全问题
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        public static string xmlApiConfig(string xmlPath)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string resourceName = xmlPath + ".xml";
            string result = "";
            try
            {
                Stream stream = _assembly.GetManifestResourceStream(resourceName);
                StreamReader myread = new StreamReader(stream);
                result = myread.ReadToEnd();
                myread.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //转为小写，避免sql与前台json大小写不一致
            return result;
        }
    }
}
