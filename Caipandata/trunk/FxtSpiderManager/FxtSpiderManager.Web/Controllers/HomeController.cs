using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtSpiderManager.Common;


/***
 * 作者:  李晓东
 * 时间:  2013.12.4
 * 摘要:  创建 HomeController 默认页类
 * **/
namespace FxtSpiderManager.Web.Controllers
{
    
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return null;
        }

    }
}
