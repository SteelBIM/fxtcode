using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtSpiderManager.Common;
using System.Net.Http;
using Newtonsoft.Json;
using FxtSpiderManager.Common.FxtSpider;

/***
 * 作者:  李晓东
 * 时间:  2013.12.5
 * 摘要:  创建 SpiderController 控制器类
 * **/
namespace FxtSpiderManager.Web.Controllers
{
    public class SpiderController : BaseController
    {
        //
        // GET: /Spider/
        [HttpPost]
        public ActionResult Index()
        {
            FxtspiderClient fxtSpiderClient = new FxtspiderClient();
            string aa=fxtSpiderClient.GetDatProject(6, "", "", "", 10, 1);
            //HttpClientHelper.GetAsync(GetUrl("api/Test/GetTest/?Name=aa&Name1=ddd"))
            return Json(aa);
        }
    }
}
