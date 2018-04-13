using System;
using System.Collections.Generic;
using System.Web;

namespace CBSS.Core.Pay.WXPay.lib
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}