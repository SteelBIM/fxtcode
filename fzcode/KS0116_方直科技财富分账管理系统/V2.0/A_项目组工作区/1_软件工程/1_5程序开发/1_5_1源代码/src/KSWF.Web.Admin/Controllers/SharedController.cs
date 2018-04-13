using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KSWF.Web.Admin.Controllers
{
    public class SharedController : Controller
    {
        //
        // GET: /Shared/
          [AuthenticationAttribute(IsCheck = false)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
