using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/***
 * 作者:  李晓东
 * 时间:  2013.12.4
 * 摘要:  创建 ErrorController 错误类
 * **/
namespace FxtSpiderManager.Web.Controllers
{
    public class ErrorController : Controller
    {
        //Get 默认错误
        public ActionResult Index()
        {
            return View();
        }

        //403
        public ActionResult NoAccess()
        {
            Response.Write("403");
            return null;
        }

        //404
        public ActionResult FileNotFound()
        {
            Response.Write("404");
            return null;
        }

    }
}
