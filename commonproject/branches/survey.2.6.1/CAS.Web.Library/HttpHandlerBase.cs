using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAS.Web.Library
{
    /// <summary>
    /// 一般处理程序基类
    /// 所有一般处理程序，都要继承IRequiresSessionState接口以读取session
    /// 除登录处理外，都需要判断登录状态，如未登录，统一返回提示。
    /// </summary>
    public class HttpHandlerBase:IHttpHandler,System.Web.SessionState.IRequiresSessionState
    {
        public bool IsLogined {
            get {
                return CheckLogin();
            }
        }

        public bool HasPrivilege
        {
            get
            {
                if (!IsLogined) return false;
                return true;
            }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            
        }   
  
        public bool CheckLogin()
        {
            HttpContext context = HttpContext.Current;
            bool isLogined = true;
            if (!isLogined)
            {
                //返回未登录的json数据
                string json = CAS.Common.JSONHelper.GetJson(null,0,"未登录",null);
                context.Response.Write(json);
                return false;
            }
            return true;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}