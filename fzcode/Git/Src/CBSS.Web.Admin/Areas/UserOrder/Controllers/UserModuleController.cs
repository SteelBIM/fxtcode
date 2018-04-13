using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBSS.UserOrder.Contract.DataModel;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.BLL;
using CBSS.UserOrder.Contract.ViewModel;

namespace CBSS.Web.Admin.Areas.UserOrder.Controllers
{
    public class UserModuleController : ControllerBase
    {
        //
        // GET: /UserOrder/UserModule/

        public ActionResult Index()
        {
            if (CheckActionName("UserModule_View") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            ViewBag.Add = action.Add;
            ViewBag.Del = action.Del;
            return View();
        }

        public JsonResult GetUserModulePage(int pagesize, int pageindex, int MarketClassifyId, string MarketBookName,string UserPhone)
        {
            UserModuleRequest request = new UserModuleRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            request.MarketBookName = MarketBookName;
            request.UserPhone = UserPhone;
            #region   获取书籍分类ID集合
            if (MarketClassifyId > 0)
            {
                TreeData treedata = new TreeData();
                List<MarketClassify> MarketClassifyList = TbxService.GetMarketClassifyList(a => 1 == 1).ToList();
                request.MarketClassifyIdList = treedata.GetMarketClassifyID(MarketClassifyId, MarketClassifyList);
                request.MarketClassifyIdList.Add(MarketClassifyId);
            }
            #endregion 

            int totalcount = 0;
            IEnumerable<v_UserModuleItem> list = this.UserOrderService.GetUserUserModuleList(out totalcount, request);
            return Json(new { total = totalcount, rows = list });
        }

        [HttpPost]
        public bool Delete(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                List<int> listid = new List<int>();
                foreach (string row in ids.Split(','))
                    listid.Add(int.Parse(row.ToString()));

                if (listid != null && listid.Count > 0)
                    return UserOrderService.DeleteUserModule(listid);
            }
            return false;
        }


        public ActionResult Create()
        {
            return View("Edit");
        }



        public JsonResult GetUserParticulars(string Iphone)
        {
            return Json(IBSService.GetUserInfoByUserOtherID(Iphone, 1));
        }
        public bool UserAllocationModule(int UserId, string ModuleIds, DateTime StartData, DateTime EndData, string UserPhone, string Remark)
        {
            try
            {
                if (ModuleIds != "")
                {
                    ModuleIds = ModuleIds.TrimEnd(',').TrimStart(',');
                    string[] array = ModuleIds.Split(',');
                    List<UserModuleItem> list = new List<UserModuleItem>();
                    foreach (var item in array)
                    {
                        string[] ids = item.Split('|');
                        list.Add(new UserModuleItem()
                        {
                            UserID = UserId,
                            PayOrderID = CBSS.Core.Utility.SystemDefault.DefaultOrderNo,
                            StartDate = StartData,
                            EndDate = EndData,
                            CreateTime = DateTime.Now,
                            States = 1,
                            Remark = Remark,
                            MarketBookID = Convert.ToInt32(ids[0]),
                            UserPhone= UserPhone,
                            ModuleID = Convert.ToInt32(ids[1])
                        });
                    }
                    if (list.Count() > 0)
                    {
                        return UserOrderService.SaveUserModule(UserId, list);
                    }
                }
                //else
                //{
                //    this.UserOrderService.DelUserModule(UserId);
                //}
                return  true ;
            }
            catch (Exception)
            {
                return  false ;
            }
        }
    }
}
