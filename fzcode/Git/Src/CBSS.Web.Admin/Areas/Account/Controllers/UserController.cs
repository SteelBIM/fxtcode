using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBSS.Account.Contract;
using CBSS.Web.Admin.Common;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Web;

namespace CBSS.Web.Admin.Areas.Account.Controllers
{
    //[Permission(EnumBusinessPermission.AccountManage_User)]
    public class UserController : ControllerBase
    {
        //
        // GET: /Account/User/

        public ActionResult Index(UserRequest request)
        {
            if (CheckActionName("User_View") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            ViewBag.Add = action.Add;
            ViewBag.Edit = action.Edit;
            ViewBag.Del = action.Del;
            return View();
        }


        public JsonResult GetUserList( int pagesize, int pageindex, string mobile,string loginname)
        {
            UserRequest request = new UserRequest();
            request.LoginName = loginname;
            request.Mobile = mobile;
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            int total = 0;
            IEnumerable<Sys_User> userlist = this.AccountService.GetUserList(out total, request);
            if (userlist != null && userlist.Count() > 0)
            {
                List<Sys_Role> rolelist = AccountService.GetRoleList().ToList();
                foreach (Sys_User row in userlist)
                    row.Password = GetRoleName(row.RoleId,rolelist);
            }
          
            return Json(new { total = total, rows = userlist });
        }

        private string GetRoleName(int roleid, List<Sys_Role> rolelist)
        {
            string rolename = "";
            if (rolelist != null && rolelist.Count > 0)
            {
                List<Sys_Role> role = rolelist.Where(r => r.ID == roleid).ToList();
                if (role != null && role.Count > 0)
                    rolename = role[0].Name;
            }
            return rolename;
        }

        public ActionResult UploadDemo(int id)
        {
            this.ViewBag.RoleId = new SelectList(this.AccountService.GetRoleList(), "ID", "Name");
            var model = this.AccountService.GetUser(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult UploadDemo(int id, FormCollection collection)
        {
            var model = this.AccountService.GetUser(id);
            //this.TryUpdateModel<Sys_User>(model);
            //this.AccountService.SaveUser(model);
            return this.RefreshParent();
        }

        //
        // GET: /Account/User/Create

        public ActionResult Create()
        {
            this.ViewBag.RoleId = new SelectList(this.AccountService.GetRoleList(), "ID", "Name");

            var model = new Sys_User();
            //model.Password = "111111"; //设置默认密码
            return View("Edit", model);

        }

        //
        // POST: /Account/User/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Sys_User();
            this.TryUpdateModel<Sys_User>(model);
            //model.Password = "111111";
            model.Password = SecurityHelper.MD5(model.Password);

            try
            {
                this.AccountService.SaveUser(model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);

                var roles = this.AccountService.GetRoleList();
                this.ViewBag.RoleId = new SelectList(roles, "ID", "Name");

                return View("Edit", model);
            }

            return this.RefreshParent();
        }

        //
        // GET: /Account/User/Edit/5

        public ActionResult Edit(int id)
        {
            this.ViewBag.RoleId = new SelectList(this.AccountService.GetRoleList(), "ID", "Name");
            var model = this.AccountService.GetUser(id);

            //var roles = this.AccountService.GetRoleList();
            //this.ViewBag.RoleId = new SelectList(roles, "ID", "Name", string.Join(",", model.Roles.Select(r => r.ID)));

            return View(model);
        }


        //
        // POST: /Account/User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.AccountService.GetUser(id);
            this.TryUpdateModel<Sys_User>(model);
            this.AccountService.SaveUser(model);
            return this.RefreshParent();
        }

        // POST: /Account/User/Delete/5

        [HttpPost]
        public int Delete(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                List<int> listid = new List<int>();
                foreach (string row in ids.Split(','))
                    listid.Add(int.Parse(row.ToString()));

                if (listid != null && listid.Count > 0)
                    this.AccountService.DeleteUser(listid);
            }
            return 1;
        }
    }
}
