using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class PayHuaWeiController : ApiController
    { 
        private ApiResponse GetErrorResult(string message)
        {
            return new ApiResponse
            {
                Success = false,
                data = null,
                Message = message
            };
        }

        private ApiResponse GetResult(object Data, string message = "")
        {

            return new ApiResponse
            {
                Success = true,
                data = Data,
                Message = message
            };
        }
    }
}
