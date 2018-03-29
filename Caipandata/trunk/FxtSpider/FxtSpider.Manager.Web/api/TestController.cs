using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using FxtSpider.Manager.Common;
using System.Web.Http;

namespace FxtSpider.Manager.Web.api
{
    public class TestController : BaseApiController
    {
        //[ApiException]
        //public string Get(int id)
        //{
        //    throw new NotImplementedException("此方法未执行"); 
        //    return "get";
        //}
        [HttpGet]
        public HttpResponseMessage GetTest(string Name,string Name1)
        {
            //throw new NotImplementedException("此方法未执行");
            return Request.CreateResponse(HttpStatusCode.OK, "hello word!");
        }

        [HttpPost]
        public HttpResponseMessage Posts([FromBody]string test)
        {
            return Request.CreateResponse(HttpStatusCode.OK, string.Format("hello {0}!",test));
        }
    }
}
