using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Framework.Utils
{
    public class NetHelper
    {
        public static string GetRequestClientAddress()
        {
            string address = null;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null) // using proxy 
            {
                address = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString(); // Return real client IP. 
            }
            else// not using proxy or can't get the Client IP 
            {
                address = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(); //While it can't get the Client IP, it will return proxy IP. 
            }
            return address;
        }
    }
}
