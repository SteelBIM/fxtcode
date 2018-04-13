
using System.Linq;
using System.Web.Mvc;
using KSWF.Web.Admin.Models;
using KSWF.WFM.BLL;
using Omu.ValueInjecter;
using KSWF.WFM.Constract.Models;
using KSWF.Core.Utility;
using System.Collections.Generic;
namespace KSWF.Web.Admin.Controllers
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
            if (account != null && account.mastertype != 1)
            {

                base_dept dept = Manage.Select<base_dept>(account.deptid.ToString());
                if (dept != null)
                {
                    ViewBag.DepartName = dept.deptname;
                }


                var districts = Manage.SelectSearch<base_deptarea>(o => o.deptid == account.deptid);
                string Deptdis = "";
                foreach (var obj in districts)
                {
                    if (obj.schoolid != 0)
                    {
                        if (Deptdis.Length == 0)
                        {
                            Deptdis = obj.path + " " + obj.schoolname;
                        }
                        else
                        {
                            Deptdis += "\n" + obj.path + " " + obj.schoolname;
                        }
                    }
                    else
                    {
                        if (Deptdis.Length == 0)
                        {
                            Deptdis = obj.path;
                        }
                        else
                        {
                            Deptdis += "\n" + obj.path;
                        }
                    }
                }
                ViewBag.Districts = Deptdis;


                var masterdistricts = Manage.SelectSearch<join_mastertarea>(o => o.mastername == account.mastername);

                string masterdis = "";
                foreach (var obj in masterdistricts)
                {
                    if (obj.schoolid != 0)
                    {
                        if (masterdis.Length == 0)
                        {
                            masterdis = obj.path + " " + obj.schoolname;
                        }
                        else
                        {
                            masterdis += "\n" + obj.path + " " + obj.schoolname;
                        }
                    }
                    else
                    {
                        if (masterdis.Length == 0)
                        {
                            masterdis = obj.path;
                        }
                        else
                        {
                            masterdis += "\n" + obj.path;
                        }
                    }
                }
                ViewBag.MasterDistricts = masterdis;

                var group = Manage.Select<com_group>(account.groupid.ToString());
                ViewBag.GroupName = group == null ? "" : group.groupname;
                var masterPolicyPros = accountManager.GetmasterPolicy(account.mastername);
                ViewBag.MasterPolicyPros = masterPolicyPros;
            }
            else
                account = new com_master();

            ComMasterInfo model = new ComMasterInfo();
            model.InjectFrom(account);
            return View(model);
        }

        public ActionResult AgentIndex()
        {
            var account = Manage.SelectSearch<com_master>(o => o.mastername == masterinfo.mastername).FirstOrDefault();
            if (account != null && account.mastertype == 1)
            {
                base_dept deptinfo = Manage.Select<base_dept>(account.deptid.ToString());
                if (deptinfo != null)
                    ViewBag.DepartName = deptinfo.deptname;

                var districts = Manage.SelectSearch<base_deptarea>(o => o.deptid == account.deptid);
                string Deptdis = "";
                foreach (var obj in districts)
                {
                    if (obj.schoolid != 0)
                    {
                        if (Deptdis.Length == 0)
                        {
                            Deptdis = obj.path + " " + obj.schoolname;
                        }
                        else
                        {
                            Deptdis += "\n" + obj.path + " " + obj.schoolname;
                        }
                    }
                    else
                    {
                        if (Deptdis.Length == 0)
                        {
                            Deptdis = obj.path;
                        }
                        else
                        {
                            Deptdis += "\n" + obj.path;
                        }
                    }
                }
                ViewBag.Districts = Deptdis;







                var group = Manage.Select<com_group>(account.groupid.ToString());
                ViewBag.GroupName = group == null ? "" : group.groupname;
                var masterPolicyPros = accountManager.GetmasterPolicy(account.mastername);
                //var masterPolicyPros = Manage.SelectSearch<WFM.Constract.VW.vw_masterbpolicypr>(i => i.mastername == account.mastername);
                ViewBag.MasterPolicyPros = masterPolicyPros;
                var qudaojingli = Manage.Select<com_master>(account.parentid);
                if (qudaojingli != null)
                    ViewBag.Qudaojingli = qudaojingli.truename;

            }
            else
                account = new com_master();

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
                account.truename = model.truename;
                account.mobile = model.mobile;
                account.qq = model.qq;
                account.email = model.email;
                Manage.CustomUpdate(account, o => o.truename, o => o.mobile, o => o.email, o => o.qq);
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

        public ActionResult SaveAgentInfo(ComMasterInfo model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                var account = Manage.SelectSearch<com_master>(o => o.mastername == masterinfo.mastername).FirstOrDefault();
                account.truename = model.truename;
                account.mobile = model.mobile;
                account.qq = model.qq;
                account.email = model.email;
                account.agent_addr = model.agent_addr;
                account.agent_fax = model.agent_fax;
                account.agent_postal = model.agent_postal;
                account.agent_tel = model.agent_tel;
                Manage.CustomUpdate(account, o => o.truename, o => o.mobile, o => o.email, o => o.qq, o => o.agent_tel, o => o.agent_postal, o => o.agent_fax, o => o.agent_addr);
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

        #region 获取部门用户管理部门加载（根据数据权限） GetDept()
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="AgentId"></param>
        /// <returns></returns>
        public JsonResult GetDept()
        {
            GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();
            List<TreeView> tvlist = give.Dept(UserIdentity, masterinfo.deptid, masterinfo.dataauthority, masterinfo.agentid);
            return Json(tvlist);
        }
        #endregion


    }
}
