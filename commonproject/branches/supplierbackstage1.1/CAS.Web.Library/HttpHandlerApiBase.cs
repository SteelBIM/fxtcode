using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;

namespace CAS.Web.Library
{
    public class HttpHandlerApiBase:HttpHandlerBase
    {
        protected string requestAPI;
        public override void ProcessRequest(HttpContext context)
        {
            if (!HasPrivilege) return;
            string type = Public.GetRequest("type");
            requestAPI = Public.GetRequest("url");
            if (!requestAPI.Contains("http://")) requestAPI = Public.APIUrl + requestAPI;
            string result = "";

            
            //GET方法直接使用URL获取返回值
            if (type == "get")
            {
                result = Public.APIGet(requestAPI);
            }
            //POST方法
            else
            {
                string str = context.Request.Form.ToString();
                result = Public.APIPostBack(requestAPI, str, true);
                //退出系统
                if (!string.IsNullOrEmpty(Public.GetRequest("exit")))
                {
                    Public.SetLoginInfo(null);
                }               
            }
            context.Response.Write(result);
            context.Response.End();
        }  
    }
}