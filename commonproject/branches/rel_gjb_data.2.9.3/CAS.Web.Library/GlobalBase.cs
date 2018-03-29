using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using System.Web.Hosting;
using System.Collections;
using System.Configuration;
using CAS.Entity;

namespace CAS.Web.Library
{
    public class GlobalBase : System.Web.HttpApplication
    {
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            //根据来源域名，从数据库读取相关属性值 重要！！！！！！！
            try
            {
                Public.SetCompanyFxt();
                if (Public.CompanyFxt == null) {
                    Public.ResponseWriteError("找不到API，请联系平台管理员。");
                    return;
                }
                //写日志
                LogHelper.Info(Public.ProductName + " Start");
            }
            catch(Exception ex){
                //写日志
                LogHelper.Info(ex.Message);
            }
            //第一次运行，注册虚拟文件系统，加载调用的控件DLL
            //if (Public.DllPaths != null)
            //{
            //    //basic组件为必须
            //    if (!Public.DllPaths.Contains(Public.DllPath.Basic))
            //    {
            //        Public.DllPaths.Add(Public.DllPath.Basic);
            //    }               
            //}
            //else
            //{
            //    Public.DllPaths = new List<string>();
            //    Public.DllPaths.Add(Public.DllPath.Basic);
            //}
            //for (int i = 0; i < Public.DllPaths.Count; i++)
            //{
            //    string path = Public.DllPaths[i];
            //    VirtualPathProvider pro = new VirtualPathHelper(path);
            //    HostingEnvironment.RegisterVirtualPathProvider(pro);
            //}
            //正式环境使用此项，发布一次更新一次
#if !DEBUG
                Public.StaticVersion = DateTime.Now.ToString("yyyyMMddHHmm");
#endif
        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {
            if (Public.CompanyFxt == null)
            {   
                Public.ResponseWriteError("找不到API，请联系平台管理员。");
                return;
            }
        }

        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {           
            HttpResponse Response = HttpContext.Current.Response;
            HttpRequest Request = HttpContext.Current.Request;
            //防SQL注入处理
            if (Request.RawUrl.IndexOf("/error/") < 0)
            {

                string key = "";
                string getkeys = "";

                if (Request.RawUrl != null)
                {
                    getkeys = Request.RawUrl;
                    if (!SQLFilterHelper.ProcessSqlStr(getkeys, 0, ref key))
                    {
                        Response.Redirect(Public.RootUrl + "error/" + HttpUtility.UrlEncode("非法参数" + key));
                        Response.End();
                        return;
                    }
                }
                if (Request.Form != null)
                {
                    for (int i = 0; i < Request.Form.Count; i++)
                    {
                        getkeys = Request.Form[i];
                        //编辑器文本 kevin
                        if (Request.Form.Keys[i].ToLower() == "txtcontent") continue;
                        if (!SQLFilterHelper.ProcessSqlStr(getkeys, 1, ref key))
                        {
                            Response.Redirect(Public.RootUrl + "error/" + HttpUtility.UrlEncode("非法参数" + key));
                            Response.End();
                            return;
                        }
                    }
                }
            }
        }

        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {

        }

        protected virtual void Session_End(object sender, EventArgs e)
        {

        }

        protected virtual void Application_End(object sender, EventArgs e)
        {

        }
    }
}