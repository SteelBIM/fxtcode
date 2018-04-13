using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kingsun.SynchronousStudy.App.Common
{
    public class ApiResponse
    {
       
        public bool Success { get; set; }
        public object data { get; set; }
        public string Message { get; set; }
    }
}