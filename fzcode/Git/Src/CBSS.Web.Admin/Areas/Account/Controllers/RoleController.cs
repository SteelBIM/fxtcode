using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBSS.Account.Contract;
using CBSS.Web.Admin.Common;
using CBSS.Core.Utility;
using CBSS.Account.Contract.ViewModel;
using CBSS.Framework.Contract;

namespace CBSS.Web.Admin.Areas.Account.Controllers
{
    //[Permission(EnumBusinessPermission.AccountManage_Role)]
    public class RoleController : ControllerBase
    {
        //
        // GET: /Account/Role/
        public ActionResult Index()
        {
            if (CheckActionName("Role_View") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            ViewBag.Add = action.Add;
            ViewBag.Edit = action.Edit;
            ViewBag.Delete = action.Del;
            return View();
        }

        public JsonResult GetAllRole(int pagesize, int pageindex, string rolename)
        {
            RoleRequest request = new RoleRequest();
            request.RoleName = rolename;
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            int total = 0;
            IEnumerable<Sys_Role> rolelist = this.AccountService.GetRoleList(out total, request);
            return Json(new { total = total, rows = rolelist });
        }

        //
        // GET: /Account/Role/Create

        public ActionResult Create()
        {
            var businessPermissionList = EnumHelper.GetItemValueList<EnumBusinessPermission>();
            this.ViewBag.BusinessPermissionList = new SelectList(businessPermissionList, "Key", "Value");

            var model = new Sys_Role();
            return View("Edit", model);
        }

        //
        // POST: /Account/Role/Create

        #region  获取所有的操作权限(新增时)GetAllAction()
        /// <summary>
        /// 获取所有的操作权限(新增修改时使用)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllAction()
        {
            return Json(this.AccountService.GetAllAction());
        }
        #endregion



        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Sys_Role();
            this.TryUpdateModel<Sys_Role>(model); 
            try
            {
                this.AccountService.SaveRole(model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message); 
                return View("Edit", model);
            } 
            return this.RefreshParent();
        }

        //
        // GET: /Account/Role/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBag.ReloId = id;
            var model = this.AccountService.GetRole(id);
            var businessPermissionList = EnumHelper.GetItemValueList<EnumBusinessPermission>();
            this.ViewBag.BusinessPermissionList = new SelectList(businessPermissionList, "Key", "Value", string.Join(",", ""));
            return View(model);
        }

        //
        // POST: /Account/Role/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.AccountService.GetRole(id);
            this.TryUpdateModel<Sys_Role>(model);
            //model.BusinessPermissionString = collection["BusinessPermissionList"]; 
            try
            {
                this.AccountService.SaveRole(model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return View("Edit", model);
            }
            return this.RefreshParent();
        }

        // POST: /Account/Role/Delete/5

        [HttpPost]
        public int Delete(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                List<int> listid = new List<int>();
                bool flag = true;
                foreach (string row in ids.Split(','))
                {
                    int userCount = AccountService.GetRoleUserNumber(int.Parse(row.ToString()));
                    if (userCount>0)
                    {
                        flag = false;
                    }
                    else
                    {
                        listid.Add(int.Parse(row.ToString()));
                    } 
                }
                if (listid != null && listid.Count > 0)
                    this.AccountService.DeleteRole(listid);
                if (flag==false)//存在被引用的用户
                {
                    return 2;
                }
            }
            return 1;
        }
    }
}
