using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Mvc;
using KSWF.Core.Utility;
using KSWF.Web.Admin.Models;
using KSWF.WFM.BLL;
using KSWF.WFM.Constract.Models;
namespace KSWF.Web.Admin.Controllers
{
    public class ProductController : BaseController
    {
        ProductManage Pmanage = new ProductManage();
        //
        // GET: /Product/

        public ActionResult Index()
        {
            ViewBag.CatogoryList = KSWF.WFM.BLL.KeyValueManage.GetCatogoryData();
            ViewBag.ChannelList = KSWF.WFM.BLL.KeyValueManage.GetChannleData();
            List<cfg_product> list = base.SelectAll<cfg_product>();
            ViewBag.SubjectList = GetCurrentSubjectList(null, list);
            ViewBag.GradeList = GetCurrentGradeList(null, list);
            ViewBag.VersionList = GetCurrentVersionList(null, null, list);
            ViewBag.currentdate = DateTime.Now.ToString();
            return View();
        }

        public JsonResult CompDepart_ProductPageList(int pagesize, int pageindex, [FromBody]Product pinfo)
        {
            int totalcount = 0;
            PageParameter<cfg_product> param = new PageParameter<cfg_product>();
            param.PageIndex = setpageindex(pageindex, pagesize);
            param.PageSize = pagesize;
            param.OrderColumns = T => T.id;
            List<Expression<Func<cfg_product, bool>>> expression = new List<Expression<Func<cfg_product, bool>>>();
            expression.Add(i => i.delflg == 0 && i.isshevel==1);
            if (pinfo != null)
            {
                if (pinfo.Channel.HasValue)
                {
                    expression.Add(i => i.channel == pinfo.Channel);
                }
                if (pinfo.SubjectID.HasValue)
                {
                    expression.Add(i => i.subjectid == pinfo.SubjectID);
                }
                if (pinfo.VersionID.HasValue)
                {
                    expression.Add(i => i.versionid == pinfo.VersionID);
                }
                if (pinfo.GradeID.HasValue)
                {
                    expression.Add(i => i.gradeid == pinfo.GradeID);
                }
                if (pinfo.CategoryKey.HasValue)
                {
                    expression.Add(i => i.categorykey == pinfo.CategoryKey);
                }
            }
            param.Wheres = expression;
            // param.Where = I => I.delflg == 0;
            IList<cfg_product> list = base.SelectPage<cfg_product>(param, out totalcount);
            return Json(new { total = totalcount, rows = list });
        }

        public JsonResult SubmitData([FromBody]Product productInfo)
        {
            Core.Utility.KingResponse res = new KingResponse();
            cfg_product pinfo = new cfg_product();
            if (productInfo != null)
            {
                if (productInfo.ID.HasValue)
                {
                    pinfo = base.Select<cfg_product>(productInfo.ID.Value.ToString());
                    if (pinfo == null)
                    {
                        res = KingResponse.GetErrorResponse("修改失败,找不到产品信息");
                    }
                    else
                    {
                        pinfo.id = productInfo.ID.Value;
                        pinfo.category = productInfo.Category;
                        pinfo.categorykey = productInfo.CategoryKey;
                        pinfo.channel = productInfo.Channel.Value;
                        pinfo.productname = productInfo.ProductName;
                        pinfo.subjectid = productInfo.SubjectID.Value;
                        pinfo.subject = productInfo.Subject;
                        pinfo.versionid = productInfo.VersionID.Value;
                        pinfo.version = productInfo.Version;
                        pinfo.gradeid = productInfo.GradeID.Value;
                        pinfo.grade = productInfo.Grade;
                        pinfo.productno = productInfo.ProductNo;
                        res = Pmanage.CheckProductInfo(pinfo);
                        if (res.Success)
                        {
                            if (base.Update<cfg_product>(pinfo))
                            {
                                res = KingResponse.GetResponse("修改成功");
                            }
                            else
                            {
                                res = KingResponse.GetErrorResponse("修改失败");
                            }
                        }
                    }
                }
                else
                {
                    pinfo.category = productInfo.Category;
                    pinfo.categorykey = productInfo.CategoryKey;
                    pinfo.channel = productInfo.Channel.Value;
                    pinfo.productname = productInfo.ProductName;
                    pinfo.subjectid = productInfo.SubjectID.Value;
                    pinfo.subject = productInfo.Subject;
                    pinfo.versionid = productInfo.VersionID.Value;
                    pinfo.version = productInfo.Version;
                    pinfo.gradeid = productInfo.GradeID.Value;
                    pinfo.grade = productInfo.Grade;
                    pinfo.productno = productInfo.ProductNo;
                    res = Pmanage.CheckProductInfo(pinfo);
                    if (res.Success)
                    {
                        if (base.Add<cfg_product>(pinfo) > 0)
                        {
                            res = KingResponse.GetResponse("新增成功");
                        }
                        else
                        {
                            res = KingResponse.GetErrorResponse("新增失败");
                        }
                    }
                }
            }
            else
            {
                res = KingResponse.GetErrorResponse("参数错误");
            }
            return Json(res);
        }

        public JsonResult DeleteProduct(string ids)
        {
            KingResponse res = new KingResponse();
            string[] arrayids = ids.Split(',');
            string succ = "";
            string fail = "";
            for (int i = 0; i < arrayids.Length; i++)
            {
                cfg_product pinfo = base.Select<cfg_product>(arrayids[i]);
                if (pinfo != null)
                {
                    pinfo.delflg = 1;
                    if (base.Update<cfg_product>(pinfo))
                    {
                        succ += arrayids[i];
                        // res = KingResponse.GetResponse("删除成功");
                    }
                    else
                    {
                        fail += arrayids[i];
                        // res = KingResponse.GetErrorResponse("删除失败");
                    }
                }
            }
            res = KingResponse.GetResponse(succ + "|" + fail);
            return Json(res);
        }

        public JsonResult GetSearchValue([FromBody]Product pinfo)
        {
            KingResponse res = new KingResponse();
            List<cfg_keyvalue> catogoryList;
            List<cfg_product> list = base.SelectAll<cfg_product>();
            if (pinfo.Channel.HasValue)
            {
                catogoryList = Pmanage.GetCatogoryByChannel(pinfo.Channel.Value);
            }
            else
            {
                catogoryList = KeyValueManage.GetCatogoryData();
            }
            object obj = new
            {
                CatogoryList = catogoryList,
                SubjectList = GetCurrentSubjectList(pinfo.Channel, list),
                GradeList = GetCurrentGradeList(pinfo.Channel, list),
                VersionList = GetCurrentVersionList(pinfo.Channel, pinfo.CategoryKey, list)
            };
            res = KingResponse.GetResponse(obj);
            return Json(res);

        }

        private IList<cfg_keyvalue> GetCurrentSubjectList(int? Channel, IList<cfg_product> list)
        {
            if (Channel.HasValue && Channel != 0)
            {
                var sublist = from l in list
                              where l.channel == Channel && l.delflg == 0
                              group new { l.subjectid, l.subject } by new { l.subjectid, l.subject } into g
                              select (new cfg_keyvalue { Key = g.Key.subjectid.ToString(), Value = g.Key.subject });
                return sublist.ToList();
            }
            else
            {
                var sublist = from l in list
                              where l.delflg == 0
                              group new { l.subjectid, l.subject } by new { l.subjectid, l.subject } into g
                              select (new cfg_keyvalue { Key = g.Key.subjectid.ToString(), Value = g.Key.subject });
                return sublist.ToList();
            }
        }
        private IList<cfg_keyvalue> GetCurrentGradeList(int? Channel, IList<cfg_product> list)
        {
            if (Channel.HasValue && Channel != 0)
            {
                var gradelist = from l in list
                                where l.channel == Channel && l.delflg == 0 && l.gradeid > 0 && l.grade !=null && l.grade!=""
                                group new { l.gradeid, l.grade } by new { l.gradeid, l.grade } into g
                                select (new cfg_keyvalue { Key = g.Key.gradeid.ToString(), Value = g.Key.grade });
                return gradelist.OrderBy(i => i.Key).ToList();
            }
            else
            {
                var gradelist = from l in list
                                where l.delflg == 0 && l.gradeid > 0 && l.grade != null && l.grade != ""
                                group new { l.gradeid, l.grade } by new { l.gradeid, l.grade } into g
                                select (new cfg_keyvalue { Key = g.Key.gradeid.ToString(), Value = g.Key.grade });
                return gradelist.OrderBy(i => i.Key).ToList();
            }
        }
        private IList<cfg_keyvalue> GetCurrentVersionList(int? channel, int? category, IList<cfg_product> list)
        {
            if (category.HasValue && category != 0)
            {
                var versionlist = from l in list
                                  where l.categorykey == category && l.delflg == 0
                                  group new { l.versionid, l.version } by new { l.versionid, l.version } into g
                                  select (new cfg_keyvalue { Key = g.Key.versionid.ToString(), Value = g.Key.version });
                return versionlist.ToList();
            }
            else
            {
                if (channel.HasValue)
                {
                    var versionlist = from l in list
                                      where l.channel == channel && l.delflg == 0
                                      group new { l.versionid, l.version } by new { l.versionid, l.version } into g
                                      select (new cfg_keyvalue { Key = g.Key.versionid.ToString(), Value = g.Key.version });
                    return versionlist.ToList();
                }
                else
                {
                    var versionlist = from l in list
                                      where l.delflg == 0
                                      group new { l.versionid, l.version } by new { l.versionid, l.version } into g
                                      select (new cfg_keyvalue { Key = g.Key.versionid.ToString(), Value = g.Key.version });
                    return versionlist.ToList();
                }
            }
        }


    }

}
