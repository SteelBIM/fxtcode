using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Search.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {

        public ActionResult Index(string inputStr, int? pageIndex)
        {
            return View();
        }

        //public LandCase _landCase;

        ////
        //// GET: /Search/Search/
        //public SearchController(LandCase landCase)
        //{
        //    this._landCase = landCase;
        //}
       
        //public ActionResult Index(string inputStr, int? pageIndex)
        //{

        //    var query = new List<SearchViewMode>();

        //    if (!string.IsNullOrWhiteSpace(inputStr))
        //    {
        //        var result = new List<SearchViewMode>();
        //        this.ViewBag.inputStr = inputStr;
        //        GoLucene.SearchFromIndexData(inputStr, out result);
        //        var fxtcompanyids = _landCase.GetAccessFxtCompanyId(Passport.Current.FxtCompanyId, Passport.Current.CityId);

        //        query = result.Where(m => fxtcompanyids.Any(n => n == m.FxtCompanyId)).ToList();
        //    }

            

        //    var viewModel = query.ToPagedList( pageIndex ?? 1, 15 );

        //    return View(viewModel);
        //}

      
        //public ActionResult UpdateIndex()
        //{
        //    try
        //    {
        //        GoLucene.DeleteIndex();
        //        GoLucene.CreateIndexByData();
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(ex.Message);
        //    }

        //}

        public ActionResult Welcome()
        {
            return View();
        }

    }
}
