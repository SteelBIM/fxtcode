using KSWF.Framework.BLL;
using KSWF.Web.Admin.Controllers;
using KSWF.WFM.Constract.Models;
using KSWF.WFM.Constract.VW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    /// <summary>
    /// 需要用到递归的方法
    /// </summary>
    public class Recursive
    {

        Manage manage = new Manage();

        #region 加载部门下所有节点（包含本部门,包含节点点击事件）GetDeptRootNode(int ParentId, string AgentId)
        /// <summary>
        /// 加载树形部门(获取部门根节点)
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="AgentId"></param>
        /// <returns></returns>
        public List<TreeView> GetDeptRootNode(int ParentId, string AgentId, int isfilterarea = 0)
        {
            List<TreeView> treeview = new List<TreeView>();
            if (isfilterarea == 1)
            {
                List<vw_areadept> deptlist = manage.SelectSearch<vw_areadept>(x => ( x.agentid == AgentId && x.delflg == 0));
                if (deptlist != null && deptlist.Count > 0)
                {
                    Predicate<vw_areadept> dept = delegate(vw_areadept p) { return p.deptid == ParentId; };
                    List<vw_areadept> nodelistdept = deptlist.FindAll(dept);
                    if (nodelistdept != null && nodelistdept.Count > 0)
                    {
                        for (int i = 0; i < nodelistdept.Count; i++)
                        {
                            List<TreeView> tv = GetDeptNodeFilterArea(nodelistdept[i].deptid, deptlist);
                            bool isContainNods = false;
                            if (tv != null && tv.Count > 0)
                            {
                                isContainNods = true;
                            }
                            treeview.Add(new TreeView() { tag = nodelistdept[i].deptid.ToString(), ParentId = nodelistdept[i].parentid.ToString(), text = "<span onclick=\"itemOnclick(" + nodelistdept[i].deptid.ToString() + ")\">" + nodelistdept[i].deptname + "</span>", nodes = tv, isContainNods = isContainNods });
                        }
                    }
                }
            }
            else
            {
                List<base_dept> deptlist = manage.SelectSearch<base_dept>(x => (x.agentid == AgentId && x.delflg == 0));
                if (deptlist!=null &&  deptlist.Count > 0)
                {
                    Predicate<base_dept> dept = delegate(base_dept p) { return p.deptid == ParentId; };
                    List<base_dept> nodelistdept = deptlist.FindAll(dept);
                    if (nodelistdept != null && nodelistdept.Count > 0)
                    {
                        for (int i = 0; i < nodelistdept.Count; i++)
                        {
                            List<TreeView> tv = GetDeptNode(nodelistdept[i].deptid, deptlist);
                            bool isContainNods = false;
                            if (tv != null && tv.Count > 0)
                            {
                                isContainNods = true;
                            }
                            treeview.Add(new TreeView() { tag = nodelistdept[i].deptid.ToString(), ParentId = nodelistdept[i].parentid.ToString(), text = "<span onclick=\"itemOnclick(" + nodelistdept[i].deptid.ToString() + ")\">" + nodelistdept[i].deptname + "</span>", nodes = tv, isContainNods = isContainNods });
                        }
                    }
                }
            }
            return treeview;
        }

        /// <summary>
        /// 加载下级部门 过滤未选区域的部门
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="AgentId"></param>
        /// <param name="isfilterarea"></param>
        /// <returns></returns>
        public List<TreeView> GetDeptNodeFilterArea(int deptid, List<vw_areadept> deptlist)
        {
            List<TreeView> treeview = new List<TreeView>();
            Predicate<vw_areadept> dept = delegate(vw_areadept p) { return p.parentid == deptid; };
            List<vw_areadept> nodelistdept = deptlist.FindAll(dept);
            if (nodelistdept != null && nodelistdept.Count > 0)
            {
                for (int i = 0; i < nodelistdept.Count; i++)
                {
                    List<TreeView> tv = GetDeptNodeFilterArea(nodelistdept[i].deptid, deptlist);
                    bool isContainNods = false;
                    if (tv != null && tv.Count > 0)
                    {
                        isContainNods = true;
                    }
                    treeview.Add(new TreeView() { tag = nodelistdept[i].deptid.ToString(), ParentId = nodelistdept[i].parentid.ToString(), text = "<span onclick=\"itemOnclick(" + nodelistdept[i].deptid.ToString() + ")\">" + nodelistdept[i].deptname + "</span>", nodes = tv, isContainNods = isContainNods });
                }
            }
            return treeview;
        }

        /// <summary>
        /// 加载下级部门
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="AgentId"></param>
        /// <param name="isfilterarea"></param>
        /// <returns></returns>
        public List<TreeView> GetDeptNode(int deptid, List<base_dept> deptlist)
        {
            List<TreeView> treeview = new List<TreeView>();
            Predicate<base_dept> dept = delegate(base_dept p) { return p.parentid == deptid; };
            List<base_dept> nodelistdept = deptlist.FindAll(dept);
            if (nodelistdept != null && nodelistdept.Count > 0)
            {
                for (int i = 0; i < nodelistdept.Count; i++)
                {
                    List<TreeView> tv = GetDeptNode(nodelistdept[i].deptid, deptlist);
                    bool isContainNods = false;
                    if (tv != null && tv.Count > 0)
                    {
                        isContainNods = true;
                    }
                    treeview.Add(new TreeView() { tag = nodelistdept[i].deptid.ToString(), ParentId = nodelistdept[i].parentid.ToString(), text = "<span onclick=\"itemOnclick(" + nodelistdept[i].deptid.ToString() + ")\">" + nodelistdept[i].deptname + "</span>", nodes = tv, isContainNods = isContainNods });
                }
            }
            return treeview;
        }
        #endregion
        /// <summary>
        /// 加载树形部门
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="AgentId"></param>
        /// <param name="isfilterarea">1 过滤没有区域的部门</param>
        /// <returns></returns>
        public List<TreeView> GetDeptNode(int ParentId, string AgentId, int isfilterarea = 0)
        {

            //return GetDeptRootNode(ParentId, AgentId, isfilterarea);

            List<TreeView> treeview = new List<TreeView>();
            if (isfilterarea == 1)
            {
                List<vw_areadept> dept = manage.SelectSearch<vw_areadept>(x => (x.agentid == AgentId && x.delflg == 0),0," deptid ");
                treeview = GetDeptNodeFilterArea(ParentId, dept);
            }
            else
            {
                List<base_dept> dept = manage.SelectSearch<base_dept>(x => ( x.agentid == AgentId && x.delflg == 0));
                treeview = GetDeptNode(ParentId, dept);
            }

            return treeview;
        }
        
        #region 加载树形部门 
        /// <summary>
        /// 加载树形部门 （包括本部门）
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="AgentId"></param>
        /// <returns></returns>
        public List<TreeView> GetDept(int deptid, string AgentId)
        {
            List<base_dept> deptlist = manage.SelectSearch<base_dept>(x => (x.agentid == AgentId && x.delflg == 0));
            Predicate<base_dept> dept = delegate(base_dept p) { return p.deptid == deptid; };
         
            List<base_dept> nodelistdept = deptlist.FindAll(dept);
            List<TreeView> treeview = new List<TreeView>();
            if (nodelistdept!=null && nodelistdept.Count > 0)
            {
                for (int i = 0; i < nodelistdept.Count; i++)
                {
                    List<TreeView> tv = GetDeptNodeTreeView(nodelistdept[i].deptid, deptlist);
                    bool isContainNods = false;
                    if (tv != null && tv.Count > 0)
                    {
                        isContainNods = true;
                    }
                    treeview.Add(new TreeView() { Id = nodelistdept[i].deptid.ToString(), tag = nodelistdept[i].deptid.ToString(), createname = nodelistdept[i].createname, ParentId = nodelistdept[i].parentid.ToString(), text = nodelistdept[i].deptname, nodes = tv, isContainNods = isContainNods });
                }
            }
            return treeview;
        }
       
        /// <summary>
        /// 加载下级部门（不包括本部门） 
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="AgentId"></param>
        /// <returns></returns>
        public List<TreeView> SelectDept(int ParentId, string AgentId)
        {
            List<base_dept> deptlist = manage.SelectSearch<base_dept>(x => ( x.agentid == AgentId && x.delflg == 0));
            return GetDeptNodeTreeView(ParentId, deptlist);
        }


        public List<TreeView> GetDeptNodeTreeView(int deptid, List<base_dept> listdept)
        {
            Predicate<base_dept> dept = delegate(base_dept p) { return p.parentid == deptid; };
            List<base_dept> nodelistdept = listdept.FindAll(dept);
            List<TreeView> treeview = new List<TreeView>();
            if (nodelistdept!=null &&  nodelistdept.Count > 0)
            {
                for (int i = 0; i < nodelistdept.Count; i++)
                {
                    List<TreeView> tv = GetDeptNodeTreeView(nodelistdept[i].deptid, listdept);
                    bool isContainNods = false;
                    if (tv != null && tv.Count > 0)
                    {
                        isContainNods = true;
                    }
                    treeview.Add(new TreeView() { Id = nodelistdept[i].deptid.ToString(), tag = nodelistdept[i].deptid.ToString(), createname = nodelistdept[i].createname, ParentId = nodelistdept[i].parentid.ToString(), text = nodelistdept[i].deptname, nodes = tv, isContainNods = isContainNods });
                }
            }
            return treeview;
        }
        #endregion


        #region   List<string> 获取部门所有下所有子部门ID GetDeptNodeId(int DeptId, string agentid)

        List<string> InIds = new List<string>();

        public List<string> GetDeptNodeIdList(int DeptId)
        {
            new List<string>();
            GetDeptNodeId(DeptId);
            return InIds;
        }

        /// <summary>
        /// 获取部门所有下所有子部门ID
        /// </summary>
        /// <param name="DeptId"></param>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public List<string> GetDeptNodeId(int DeptId, string agentid)
        {
            new List<string>();
            GetDeptNodeId(DeptId, manage.SelectSearch<base_dept>(x => (x.agentid == agentid && x.delflg == 0)));
            return InIds;
        }
       
        public List<string> GetDeptNodeId(int deptid, List<base_dept> listdept)
        {
            Predicate<base_dept> dept = delegate(base_dept p) { return p.parentid == deptid; };
            List<base_dept> nodelistdept = listdept.FindAll(dept);

            if (nodelistdept != null && nodelistdept.Count > 0)
            {
                for (int i = 0; i < nodelistdept.Count; i++)
                {
                    InIds.Add(nodelistdept[i].deptid.ToString());
                    GetDeptNodeId(nodelistdept[i].deptid, listdept);
                }
            }
            return InIds;
        }

        public List<string> GetDeptNodeId(int deptid)
        {
            List<base_dept> nodelistdept = manage.SelectAppointField<base_dept>(x => (x.parentid == deptid && x.delflg == 0),"deptid");

            if (nodelistdept != null && nodelistdept.Count > 0)
            {
                for (int i = 0; i < nodelistdept.Count; i++)
                {
                    InIds.Add(nodelistdept[i].deptid.ToString());
                    GetDeptNodeId(nodelistdept[i].deptid);
                }
            }
            return InIds;
        }

        #endregion


        #region List<vw_user> 获取用户及该用户下所有用户的信息 GetUser(string mastername)
        List<vw_user> listuser = new List<vw_user>();
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        public List<vw_user> GetUser(string mastername)
        {
            listuser = manage.SelectSearch<vw_user>(x => (x.mastername == mastername));

            if (listuser.Count > 0)
            {
                GetUserCurrentNode(mastername);
            }
            return listuser;
        }
        /// <summary>
        /// 获取用户下所有的用户信息
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        public List<vw_user> GetUserCurrentNode(string mastername)
        {
            List<vw_user> userlist = manage.SelectSearch<vw_user>(x => (x.parentname == mastername));

            if (userlist.Count > 0)
            {
                for (int i = 0; i < userlist.Count; i++)
                {
                    GetUserCurrentNode(userlist[i].mastername);
                    listuser.Add(userlist[i]);
                }
            }
            return listuser;
        }
        #endregion

        #region 获取用户名或者部门 GetUserNameDeptId(string AreaId, int schoolid)
        /// <summary>
        /// 获取用户名或者部门
        /// </summary>
        /// <param name="channel">产品的key</param>
        /// <param name="AreaId"></param>
        /// <param name="schoolid"></param>
        /// <returns></returns>
        public com_master GetUserNameDeptId(int? channel, int? areaId, int? schoolid = 0)
        {
            com_master master = new com_master();
            //bool isMaster = true;
            if (schoolid != null && schoolid > 0)
            {
                List<join_mastertarea> listmasterarea = manage.SelectSearch<join_mastertarea>(t => t.schoolid == schoolid && t.pids.Contains(channel.ToString()));//获取学校
                if (listmasterarea != null && listmasterarea.Count > 0)
                {
                    return GetMinMastername(listmasterarea);
                }
            }
            if (areaId != null)
            {
                string AreaId = areaId.ToString();
                if (AreaId.Length == 9)
                {
                    List<join_mastertarea> listmasterarea2 = manage.SelectSearch<join_mastertarea>(t => t.districtid == AreaId && t.schoolid == 0 && t.pids.Contains(channel.ToString())).OrderByDescending(x => x.id).ToList();
                    if (listmasterarea2 != null && listmasterarea2.Count > 0)
                    {
                        return GetMinMastername(listmasterarea2);
                        //master.deptid = 0;
                    }
                    else
                    {
                        string aid = AreaId.Substring(0, 6) + "000";
                        List<join_mastertarea> listmasterarea3 = manage.SelectSearch<join_mastertarea>(t => t.districtid == aid && t.schoolid == 0 && t.pids.Contains(channel.ToString())).OrderByDescending(x => x.id).ToList();
                        if (listmasterarea3 != null && listmasterarea3.Count > 0)
                        {
                            return GetMinMastername(listmasterarea3);
                            //master.deptid = 0;
                        }
                        else
                        {
                            string aid2 = AreaId.Substring(0, 4) + "00000";
                            List<join_mastertarea> listmasterarea4 = manage.SelectSearch<join_mastertarea>(t => t.districtid == aid2 && t.pids.Contains(channel.ToString())).OrderByDescending(x => x.id).ToList();
                            if (listmasterarea4 != null && listmasterarea4.Count > 0)
                            {
                                return GetMinMastername(listmasterarea4);
                                //master.deptid = 0;
                            }
                            else
                            {

                                string aid3 = AreaId.Substring(0, 2) + "0000000";
                                List<join_mastertarea> listmasterarea5 = manage.SelectSearch<join_mastertarea>(t => t.districtid == aid3 && t.pids.Contains(channel.ToString())).OrderByDescending(x => x.id).ToList();
                                if (listmasterarea5 != null && listmasterarea5.Count > 0)
                                {
                                    return GetMinMastername(listmasterarea5);
                                    //master.deptid = 0;
                                }
                                else
                                {
                                    BaseController controller = new BaseController();
                                    base_dept dept = controller.GetDeptByArea(AreaId, channel);
                                    if (dept != null)
                                    {
                                        master.mastername = "";
                                        master.deptid = dept.deptid;
                                        ////特殊字段，传递部门名称出去
                                        master.password = dept.deptname;
                                        master.agentid = dept.agentid;
                                        //isMaster = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //if (master != null && !string.IsNullOrEmpty(master.mastername) && isMaster==true)
            //{
            //    return manage.SelectSearch<com_master>(x => x.mastername == master.mastername).FirstOrDefault();
            //}
            return master;
        }
        #endregion

        private com_master GetMinMastername(List<join_mastertarea> listmasterarea)
        {
            List<string> masternames = new List<string>();
            foreach (var item in listmasterarea)
            {
                masternames.Add(item.mastername);
            }
            return manage.SelectIn<com_master>( x=>x.state==0,"mastername", masternames).OrderByDescending(x => x.masterid).First();
        }

        /// <summary>
        /// 获取员工所有下级代理商ID
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        public List<string> GetEmplloyAllNodsAgentId(string mastername)
        {
            new List<string>();
            List<com_master> masterlist = manage.SelectAppointField<com_master>(t=>t.parentname==mastername && t.state==0,"agentid");
            if (masterlist != null && masterlist.Count > 0)
            {
                for (int i = 0; i < masterlist.Count; i++)
                {
                    InIds.Add(masterlist[i].agentid);
                    GetNodsAgentId(masterlist[i].agentid);
                }
            }
            return InIds;
        }

        public List<string> GetAllNodsAgentId(List<string> deptilist)
        {
            new List<string>();
            List<Expression<Func<com_master, bool>>> expression = new List<Expression<Func<com_master, bool>>>();
            expression.Add(t => t.state == 0);
            List<com_master> masterlist = manage.SelectSearchs<com_master>(expression, "agentid", deptilist);
            if (masterlist != null && masterlist.Count > 0)
            {
                for (int i = 0; i < masterlist.Count; i++)
                {
                    InIds.Add(masterlist[i].agentid);
                    GetNodsAgentId(masterlist[i].agentid);
                }
            }
            return InIds;
        } 


        public List<string> GetNodsAgentId(string agentid)
        {
            List<com_master> masterlist = manage.SelectAppointField<com_master>(t => t.pagentid == agentid && t.state == 0, "agentid");
            if (masterlist != null && masterlist.Count > 0)
            {
                for (int i = 0; i < masterlist.Count; i++)
                {
                    InIds.Add(masterlist[i].agentid);
                    GetNodsAgentId(masterlist[i].agentid);
                }
            }
            return InIds;
        }
    }
}