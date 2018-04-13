using System;
using System.Collections.Generic;
using System.Web;

namespace Kingsun.SynchronousStudy.Pay.lib
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}