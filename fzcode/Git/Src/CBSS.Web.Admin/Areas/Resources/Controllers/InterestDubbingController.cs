using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBSS.Core.Utility;

namespace CBSS.Web.Admin.Areas.ResourcesManager.Controllers
{
    public class InterestDubbingController : ControllerBase
    {
        //
        // GET: /ResourcesManager/InterestDubbing/

        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult SaveAsExcel()
        //{
        //    var oFile = Request.Files["txt_file"];
        //    string name = "";
        //    if (oFile != null)
        //    {
        //        name = ExcelHelper.GetNewFileName(oFile.FileName, Server.MapPath("~\\Upload\\Excel\\"), oFile);
        //    }
        //    return Json(name);
        //}

        //public JsonResult ImportVideoDetails()
        //{
        //    var oFile = Request.Files["txt_file"];
        //    string name = "";
        //    int bookId = 0;
        //    string bookName = "";
        //    if (oFile != null)
        //    {
        //        name = ResourcesService.ImportVideoDetails(oFile.FileName, Server.MapPath("~\\Upload\\Excel\\"), oFile, bookId, bookName);
        //    }
        //    return Json(name);
        //}

    }
}
