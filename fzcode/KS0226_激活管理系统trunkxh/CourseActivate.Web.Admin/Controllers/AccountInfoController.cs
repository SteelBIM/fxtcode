
using System.Linq;
using System.Web.Mvc;
using CourseActivate.Web.Admin.Models;
using CourseActivate.Account.BLL;
using Omu.ValueInjecter;
using CourseActivate.Account.Constract.Models;
using CourseActivate.Core.Utility;
namespace CourseActivate.Web.Admin.Controllers
{
    public class AccountInfoController : BaseController
    {
        //
        // GET: /AccountInfo/
        private AccountManager accountManager = new AccountManager();
        public ActionResult Index()
        {
            // User.Identity.Name       

            var account = Manage.SelectSearch<com_master>(o => o.mastername == masterinfo.mastername).FirstOrDefault();

            ComMasterInfo model = new ComMasterInfo();
            model.InjectFrom(account);
            return View(model);
        }

        public ActionResult SaveMasterInfo(ComMasterInfo model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                var account = Manage.SelectSearch<com_master>(o => o.mastername == masterinfo.mastername).FirstOrDefault();
                account.mobile = model.mobile == null ? "" : model.mobile;
                account.email = model.email == null ? "" : model.email;
                account.remark = model.remark == null ? "" : model.remark;

                Manage.CustomUpdateEntity(o=>o.masterid.ToString(),account, o => o.mobile, o => o.email, o => o.remark);
                msg = "保存成功";
            }
            else
            {
                foreach (var errorObj in ModelState.Select(o => o.Value.Errors))
                {
                    foreach (var error in errorObj)
                    {
                        msg += error.ErrorMessage + ";";
                    }
                }
            }
            return Json(msg);
        }

        public JsonResult UpdateUserPassword(string OldPassword, string NewPassword)
        {
            OldPassword = PublicHelp.pswToSecurity(OldPassword);
            var account = Manage.SelectSearch<com_master>(o => o.mastername == masterinfo.mastername).FirstOrDefault();
            if (account == null)
            {
                return Json(KingResponse.GetErrorResponse("非法操作，用户不存在"));
            }
            if (account.password == OldPassword)
            {
                if (base.Update<com_master>(new { password = PublicHelp.pswToSecurity(NewPassword) }, t => t.mastername == account.mastername))
                {

                    return Json(KingResponse.GetResponse(""));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("操作失败"));
                }
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("旧密码不正确"));
            }
        }



    }
}
