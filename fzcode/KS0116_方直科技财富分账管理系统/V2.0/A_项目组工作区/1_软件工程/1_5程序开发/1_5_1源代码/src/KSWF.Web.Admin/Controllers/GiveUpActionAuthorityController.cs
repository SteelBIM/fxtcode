using System.Web.Mvc;
using KSWF.Framework.BLL;
using KSWF.WFM.Constract.Models;
using System.Collections.Generic;
using KSWF.Web.Admin.Models;
using System;
using System.Linq.Expressions;
using KSWF.WFM.Constract.VW;
using System.Collections;

namespace KSWF.Web.Admin.Controllers
{
    /// <summary>
    /// 公共调用方法 不需要判断权限 
    /// </summary>
    public class GiveUpActionAuthorityController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        Manage manage = new Manage();

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        [AuthenticationAttribute(IsCheck = false)]
        [HttpPost]
        public JsonResult GetAllGroup()
        {
            return Json(manage.SelectSearch<com_group>(x => x.delflg == 0, 0));
        }

      

        /// <summary>
        /// 根据部门ID获取地区
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public JsonResult GetArea(int deptId)
        {
            if (deptId > 0)
            {
                return Json(manage.SelectSearch<base_deptarea>(x => x.deptid == deptId));
            }
            return Json("");
        }

        public int GetAreaNumber(int deptid)
        {
            if (deptid > 0)
            {
                return manage.GetTotalCount<base_deptarea>(j => j.deptid == deptid);
            }
            return 0;
        }

        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult GetDeptById(int Id)
        {
            return Json(manage.SelectSearch<base_dept>(x => x.deptid == Id));
        }
        public JsonResult GetAreaByMasterName(string mastername)
        {
            List<join_mastertarea> list = manage.SelectSearch<join_mastertarea>(t => t.mastername == mastername);
            if (list != null && list.Count > 0)
            {
                List<TreeView> treeViews = new List<TreeView>();
                foreach (var school in list)
                {
                    TreeView treeView = new TreeView();
                    //if (school.schoolid > 0)
                    //{
                    //    treeView.Id = school.schoolid.ToString();
                    //    treeView.tag = school.districtid.ToString() + "|2=" + school.path + "?" + school.schoolid + "?" + school.schoolname;
                    //}
                    //else
                    //{
                    //    treeView.Id = school.districtid.ToString();
                    //    treeView.tag = school.districtid.ToString() + "|2=" + school.path;
                    //}

                    treeView.tag = school.districtid.ToString() + "|" + school.path + "|" + school.schoolid + "|" + school.schoolname+"|"+school.pids;
                    treeView.productkeys = school.pids;
                    treeView.text = school.path + school.schoolname;
                    treeView.schoolname = school.schoolname;
                    CheckedCheck s = new CheckedCheck();
                    s.@checked = false;
                    treeView.state = s;
                    treeViews.Add(treeView);
                }
                return Json(treeViews);
            }
            return Json(list);
        }

        public JsonResult GetMasterNameArea(string mastername, string productkeys)
        {
            List<join_mastertarea> list = manage.SelectSearch<join_mastertarea>(t => t.mastername == mastername && t.pids.Contains(","+productkeys+","));
            if (list != null && list.Count > 0)
            {
                List<TreeView> treeViews = new List<TreeView>();
                foreach (var school in list)
                {
                    TreeView treeView = new TreeView();
                    //if (school.schoolid > 0)
                    //{
                    //    treeView.Id = school.schoolid.ToString();
                    //    treeView.tag = school.districtid.ToString() + "|2=" + school.path + "?" + school.schoolid + "?" + school.schoolname;
                    //}
                    //else
                    //{
                    //    treeView.Id = school.districtid.ToString();
                    //    treeView.tag = school.districtid.ToString() + "|2=" + school.path;
                    //}

                    treeView.tag = school.districtid.ToString() + "|" + school.path + "|" + school.schoolid + "|" + school.schoolname + "|" + productkeys;
                    treeView.productkeys = school.pids;
                    treeView.text = school.path + school.schoolname;
                    treeView.schoolname = school.schoolname;
                    CheckedCheck s = new CheckedCheck();
                    s.@checked = false;
                    treeView.state = s;
                    treeViews.Add(treeView);
                }
                return Json(treeViews);
            }
            return Json(list);
        }

        public string[] Heavy(string area)
        {
            string AreaClone = area;
            string[] array = AreaClone.Split('@');

            if (array != null && array.Length > 0)
            {
                for (int i = array.Length - 1; i >= 0; i--)
                {
                    string AreaId = array[i].Split('|')[0];
                    AreaClone = AreaClone.Replace(AreaId, "");
                    ArrayList al = new ArrayList(array);
                    array = (string[])al.ToArray(typeof(string));
                    if (AreaClone.Contains(AreaId))
                        al.RemoveAt(1);

                }
            }
            return array;
        }

       

        public int MasterChangeInfoAdd(string mastername, int changetype, string oldIds, string oldNames, string newIds, string newNames, string createname)
        {
            master_changeinfo changeinfo = new master_changeinfo()
            {
                mastername = mastername,
                changetype = changetype,
                old_id = oldIds,
                old_name = oldNames,
                new_id = newIds,
                new_name = newNames,
                createname = createname,
                createtime = DateTime.Now
            };
            return manage.Add(changeinfo);
        }

        public void MasterChangeInfoDel()
        {
            manage.Delete<master_changeinfo>(t => t.mastername == "数据同步");
        }

        /// <summary>
        /// 加载部门 （部门搜索）
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="UserIdentity"></param>
        /// <param name="dataauthority"></param>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public List<TreeView> Dept(int UserIdentity, int deptId, int dataauthority, string agentid)
        {
            if (UserIdentity == 1 && dataauthority == (int)Dataauthority.全部)
            {
                List<com_master> master = manage.SelectSearch<com_master>(t => t.agentid == agentid && t.mastertype == 1);
                if (master != null && master.Count > 0)
                {
                    List<base_dept> deptlist = manage.SelectSearch<base_dept>(t => t.deptid == master[0].deptid);
                    if (deptlist != null && deptlist.Count > 0)
                    {
                        deptId = deptlist[0].parentid;
                    }
                }
            }
            else if (UserIdentity == 0)
            {
                if (dataauthority == (int)Dataauthority.全部)
                {
                    deptId = 0;
                }
            }
            Recursive giveup = new Recursive();
            if (deptId == 0 || dataauthority == (int)Dataauthority.全部)
                return giveup.GetDeptNode(deptId, agentid);
            if (deptId == 0 || dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商 || dataauthority == (int)Dataauthority.本人下级部门下级代理商)
                return giveup.GetDeptRootNode(deptId, agentid);
            List<base_dept> dept = manage.SelectSearch<base_dept>(x => x.deptid == deptId);
            List<TreeView> treeview = new List<TreeView>();
            if (dept.Count > 0)
            {
                for (int i = 0; i < dept.Count; i++)
                {
                    treeview.Add(new TreeView() { tag = dept[i].deptid.ToString(), ParentId = dept[i].parentid.ToString(), text = "<span onclick=\"itemOnclick(" + dept[i].deptid.ToString() + ")\">" + dept[i].deptname + "</span>", nodes = null, isContainNods = false });
                }
            }
            return treeview;
        }


        /// <summary>
        /// 获取代理商
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public JsonResult GetAgent(string agentid, int deptid)
        {
            if (deptid > 0)
            {
                Recursive sive = new Recursive();
                List<string> deptlist = sive.GetDeptNodeId(deptid, agentid);
                deptlist.Add(deptid.ToString());
                List<vw_agent> dlist = manage.SelectIn<vw_agent>(t => (t.pagentid == agentid && t.state == 0), "deptid", deptlist);
                return Json(dlist);
            }
            return Json(manage.SelectSearch<com_master>(t => (t.pagentid == agentid && t.mastertype == 1 && t.state == 0)));
        }

        /// <summary>
        /// 获取员工
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public JsonResult GetEmplloyee(string agentid, int deptid)
        {
            if (deptid > 0)
            {
                Recursive sive = new Recursive();
                List<string> deptlist = sive.GetDeptNodeId(deptid, agentid);
                deptlist.Add(deptid.ToString());
                List<com_master> dlist = manage.SelectIn<com_master>(t => (t.agentid == agentid && t.state == 0 && t.mastertype == 0), "deptid", deptlist);
                return Json(dlist);
            }
            return Json(manage.SelectSearch<com_master>(t => (t.agentid == agentid && t.mastertype == 0 && t.state == 0)));
        }


        public JsonResult GetDeptPrincipalName(string agentid, int deptid)
        {
            if (deptid > 0)
            {
                Recursive sive = new Recursive();
                List<string> deptlist = sive.GetDeptNodeId(deptid, agentid);
                deptlist.Add(deptid.ToString());
                List<com_master> dlist = manage.SelectIn<com_master>(t => (t.agentid == agentid && t.state == 0 && t.groupid == 4), "deptid", deptlist);
                return Json(dlist);
            }
            return Json(manage.SelectSearch<com_master>(t => (t.agentid == agentid && t.groupid == 4 && t.state == 0)));
        }
        /// <summary>
        /// 获取所有产品
        /// </summary>
        /// <returns></returns>
        public  JsonResult GetAllProduct()
        {
            return Json(manage.SelectSearch<cfg_keyvalue>(t => t.UseType == "Channel"));
        }
        /// <summary>
        /// 获取用户负责的所有产品
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        public List<string> GetUserProduct(string mastername)
        {
            List<join_mastertarea> masterarea = manage.SelectSearch<join_mastertarea>(m => m.mastername ==  mastername);
            string productkeys = "";
            List<string> list = new List<string>();
            if (masterarea!=null && masterarea.Count>0)
            {
                foreach (join_mastertarea row in masterarea)
                {
                    if (!string.IsNullOrEmpty(row.pids))
                    {
                        string[] pidarray = row.pids.Split(',');
                        foreach (string rowpid in pidarray)
                        {
                            if (!productkeys.Contains(rowpid))
                                list.Add(rowpid);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取部门下代理商所代理的产品
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public string GetMasterChannel(int deptid)
        {
            string chanels = "";
            List<com_master> masterlist = manage.SelectSearch<com_master>(m => m.deptid == deptid, 1, " masterid");
            if (masterlist != null && masterlist.Count > 0)
            {
                if (masterlist[0].agentid == Core.Utility.PublicHelp.OrgId)
                    return "0";
                else
                {
                    List<com_master> agentlist = manage.SelectSearch<com_master>(m => m.agentid == masterlist[0].agentid && m.mastertype == 1 && m.state == 0);
                    if (agentlist != null && agentlist.Count > 0)
                    {
                        List<string> productlist = GetUserProduct(agentlist[0].mastername);
                        if (productlist != null && productlist.Count > 0)
                        {
                            foreach (string row in productlist)
                                chanels += row + ",";
                        }
                    }
                }
            }
            else
            {
                return "0";
            }
            return chanels.TrimEnd(',');
        }
        public base_dept GetDept(List<base_dept> dept, string channel)
        {
            foreach (base_dept row in dept)
            {
                string channels = GetMasterChannel(row.deptid);
                if (channels == "0" || channels.Contains(channel))
                    return row;
            }
            return null;
        }
    }
}
