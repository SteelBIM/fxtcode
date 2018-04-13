using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using KSWF.Core.Utility;
using KSWF.Web.Admin.Models;
using KSWF.WFM.Constract.Models;
using KSWF.WFM.Constract.VW;
using Newtonsoft.Json;
using System.Configuration;

namespace KSWF.Web.Admin.Controllers
{
    public class AgentController : BaseController
    {
        /// <summary>
        /// 代理商管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Agent_Add()
        {
            string sTime = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            string eTime = DateTime.Now.AddYears(99).ToString("yyyy-MM-dd");
            if (UserIdentity == 1)
            {
                sTime = masterinfo.agent_startdate.ToString("yyyy-MM-dd");
                eTime = masterinfo.agent_enddate.ToString("yyyy-MM-dd");
            }
            ViewBag.sTime = sTime;
            ViewBag.eTime = eTime;
            return View();
        }
        public ActionResult Agent_Detailed()
        {
            return View();
        }

        #region 获取action控制权限(用户view呈现操作按钮) GetcurrentAction()
        /// <summary>
        /// 获取action控制权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetcurrentAction()
        {
            return Json(action);
        }
        #endregion
        [HttpPost]
        public JsonResult GetMasterById(string Id)
        {
            return Json(base.Select<com_master>(Id));
        }


        [HttpPost]
        /// <summary>
        /// 获取渠道用户
        /// </summary>
        /// <returns></returns>
        public JsonResult GetChannelPrincipal(int deptid, int Principal)
        {
            List<string> list = new List<string>();
            list.Add("4");
            list.Add("5");
            if (Principal > 0)
            {
                com_master master = base.Select<com_master>(Principal.ToString());
                if (master != null)
                {
                    deptid = master.deptid;
                }
            }
            if (deptid > 0)
                return Json(base.SelectIn<com_master>(t => (t.mastertype == 0 && t.agentid == masterinfo.agentid && t.deptid == deptid && t.state == 0), "groupid", list));
            return Json(base.SelectIn<com_master>(t => (t.mastertype == 0 && t.agentid == masterinfo.agentid && t.state == 0), "groupid", list));
        }

        [HttpPost]
        public JsonResult GetPrincipalDeptId(int Principal)
        {
            return Json(base.Select<com_master>(Principal.ToString()));

        }

        #region 添加代理商 Agent_Add(string jsondata, string Areas, string ProductIds)
        /// <summary>
        /// 添加代理商
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="Areas"></param>
        /// <param name="ProductIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Agent_Add(string jsondata, string Areas, string parentmastername, string productkeys)
        {
            Core.Utility.KingResponse res = new KingResponse();
            if (!action.Add)
            {
                res.ErrorMsg = "您没有操作权限~";
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                com_master masterdata = (com_master)serializer.Deserialize(jsondata, typeof(com_master));
                AreaFilter filter = new AreaFilter();
                KingResponse rest = filter.JudgmentArea(Areas, "", masterdata.deptid, 2, base.masterinfo.agentid, productkeys);
                if (rest.Success)
                {
                    rest.Success = false;
                    if (rest.ErrorMsg.Contains("子部门负责区域相同"))
                    {
                        rest.ErrorMsg = "所选地区与子部门负责地区相同！请重新选择~";
                    }
                    return Json(rest);
                }
                productkeys = productkeys.TrimEnd(',');

                string mastername = masterdata.mastername;//用户名
                masterdata.mastertype = 1;
                masterdata.createid = masterinfo.masterid;
                masterdata.agentid = KSWF.Core.Utility.PublicHelp.AgentId();//添加代理商的唯一ID  
                masterdata.pagentid = masterinfo.agentid;
                masterdata.password = PublicHelp.pswToSecurity(masterdata.password);
                masterdata.createtime = DateTime.Now;
                base_dept dept = new base_dept()
                {
                    agentid = masterdata.agentid,
                    createname = masterinfo.mastername,
                    delflg = 0,
                    level = 0,
                    parentid = masterdata.deptid,
                    deptname = masterdata.agentname,
                    createtime = DateTime.Now.ToString()
                };
                int deptid = base.Add<base_dept>(dept, new string[] { "districtid", "path", "isend", "schoolid", "schoolname" });
                if (deptid > 0)
                {
                    base_dept dept2 = new base_dept()
                    {
                        agentid = masterdata.agentid,
                        createname = "1",
                        delflg = 0,
                        parentid = deptid,
                        deptname = "商务部",
                        level = 1,
                        createtime = DateTime.Now.ToString()
                    };

                    base.Add<base_dept>(dept2, new string[] { "districtid", "path", "isend", "schoolid", "schoolname" });
                    masterdata.deptid = deptid;
                    int masterid = base.Add<com_master>(masterdata);//添加用户
                    if (masterid > 0)
                    {
                        #region 区域
                        if (!string.IsNullOrEmpty(Areas))
                        {
                            string[] array = Areas.TrimEnd('@').Split('@');
                            if (array.Length > 0)
                            {
                                List<join_mastertarea> entitys = new List<join_mastertarea>();
                                List<base_deptarea> deptares = new List<base_deptarea>();

                                #region 用户负责区域 部门负责区域 数据填充
                                foreach (string row in array)
                                {
                                    string[] areaarray = row.Split('|');
                                    entitys.Add(new join_mastertarea() { mastername = mastername, districtid = areaarray[0], path = areaarray[1], schoolid = Convert.ToInt32(areaarray[2]), schoolname = areaarray[3] == null ? "" : areaarray[3], pids = AddFh(productkeys) });
                                    deptares.Add(new base_deptarea() { deptid = deptid, districtid = Convert.ToInt32(areaarray[0]), path = areaarray[1], schoolid = Convert.ToInt32(areaarray[2]), schoolname = areaarray[3] == null ? "" : areaarray[3] });
                                }
                                #endregion
                                if (base.InsertRange<join_mastertarea, base_deptarea>(entitys, deptares) > 0)//添加用户负责区域
                                {
                                    res.Success = true;
                                }
                                else
                                {
                                    res.ErrorMsg = "添加负责区域失败！请编辑用户~";
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        res.ErrorMsg = "添加用户失败！请重试~";
                    }
                }
                else
                {
                    res.ErrorMsg = "添加部门失败！请重试~";
                }
            }
            return Json(res);
        }
        #endregion

        #region 编辑代理商 Agent_Edit(string jsondata, string Areas, string ProductIds)
        /// <summary>
        /// 编辑代理商
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="Areas"></param>
        /// <param name="parentmastername"></param>
        /// <param name="deptid"></param>
        /// <param name="productkeys">现负责产品</param>
        /// <param name="oldproduct">原负责产品</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Agent_Edit(string jsondata, string Areas, string oldareas, string parentmastername, int deptid, string productkeys, string oldproduct)
        {
            Core.Utility.KingResponse res = new KingResponse();
            if (!action.Edit) //没有预览权限
            {
                res.ErrorMsg = "您没有操作权限~";
            }
            else
            {
                productkeys = productkeys.TrimEnd(',');
                string newareaids = "";
                string newareanames = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                com_master masterdata = (com_master)serializer.Deserialize(jsondata, typeof(com_master));
                AreaFilter filter = new AreaFilter();
                KingResponse rest = filter.JudgmentArea(Areas, masterdata.mastername, deptid, 2, base.masterinfo.agentid,","+ productkeys+",");
                if (rest.Success)
                {
                    rest.Success = false;
                    if (rest.ErrorMsg.Contains("子部门负责区域相同"))
                    {
                        rest.ErrorMsg = "所选地区与子部门负责地区相同！请重新选择~";
                    }
                    return Json(rest);
                }
                if (base.Update<com_master>(masterdata))
                {
                    try
                    {
                        //代理商的修改后区域
                        List<base_deptarea> newDeptareas = new List<base_deptarea>();
                        if (!string.IsNullOrEmpty(Areas))
                        {
                            string[] array = Areas.TrimEnd('@').Split('@');
                            if (array.Length > 0)
                            {
                                List<join_mastertarea> entitys = new List<join_mastertarea>();
                                List<base_deptarea> deptares = new List<base_deptarea>();

                                #region 用户负责区域 部门负责区域 数据填充
                                foreach (string row in array)
                                {
                                    string[] areaarray = row.Split('|');
                                    newDeptareas.Add(new base_deptarea() { deptid = masterdata.deptid, districtid = Convert.ToInt32(areaarray[0]), path = areaarray[1], schoolid = Convert.ToInt32(areaarray[2]), schoolname = areaarray[3] == null ? "" : areaarray[3] });
                                }
                                #endregion
                            }
                        }

                        //代理商的原来负责区域
                        List<base_deptarea> oldDeptareas = base.Manage.SelectSearch<base_deptarea>(j => j.deptid == masterdata.deptid);

                        #region 原负责区域
                        string masterareaids = "";
                        string masteraresnames = "";
                        if (oldDeptareas != null && oldDeptareas.Count > 0)
                        {
                            foreach (base_deptarea row in oldDeptareas)
                            {
                                if (row.schoolid > 0)
                                {
                                    masterareaids += row.schoolid.ToString() + ",";
                                    masteraresnames += row.path + row.schoolname + ",";
                                }
                                else
                                {
                                    masterareaids += row.districtid + ",";
                                    masteraresnames += row.path + ",";
                                }
                            }
                        }
                        #endregion

                        #region 判断区域是否有改变或者减少
                        if (!string.IsNullOrEmpty(masterareaids))//用户原来负责的区域
                        {
                            GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();
                            string[] arrayoldarea = masterareaids.TrimEnd(',').Split(',');
                            foreach (string row in arrayoldarea)
                            {
                                newareaids = "," + newareaids;
                                if (!newareaids.Contains("," + row + ","))
                                {
                                    give.MasterChangeInfoAdd(masterdata.mastername, (int)KSWF.Web.Admin.Models.ChangeType.区域变更, masterareaids, masteraresnames, newareaids, newareanames, masterinfo.mastername);
                                    break;
                                }
                            }
                        }
                        #endregion

                        Recursive sive = new Recursive();
                        List<string> agentids = sive.GetNodsAgentId(masterdata.agentid);//所有下级代理商的agetid
                        agentids.Add(masterdata.agentid);


                        string deldeptareaids = "";//所有要删除的部门对应区域记录ID
                        string delmasterareaids = "";//所有要删除的员工对就区域记录ID
                        string[] newarrayareas = Areas.TrimEnd('@').Split('@');//修改后的区域
                        List<base_deptarea> adddeptarealist = new List<base_deptarea>();//所有需要新增的部门区域
                        List<join_mastertarea> addmasterarealist = new List<join_mastertarea>();

                        List<vw_deptarea> deptarealist = base.SelectIn<vw_deptarea>("agentid", agentids); //获取所有部门对应的区域
                        List<vw_areamaster> masterlist = base.SelectIn<vw_areamaster>("agentid", agentids); //获取所有用户对应的区域

                        if (newarrayareas != null && newarrayareas.Length > 0)//新的区域直接在当前修改的部门、人员对应添加
                        {
                            foreach (string row in newarrayareas)
                            {
                                string[] newareaarray = row.Split('|');

                                #region 添加部门区域
                                adddeptarealist.Add(new base_deptarea()
                                {
                                    deptid = masterdata.deptid,
                                    districtid = Convert.ToInt32(newareaarray[0]),
                                    path = newareaarray[1],
                                    schoolid = Convert.ToInt32(newareaarray[2]),
                                    schoolname = newareaarray[3] == null ? "" : newareaarray[3],
                                    isend = base.isend(Convert.ToInt32(newareaarray[0]), Convert.ToInt32(newareaarray[2]))
                                });
                                #endregion

                                #region 添加用户区域
                                addmasterarealist.Add(new join_mastertarea()
                                {
                                    districtid = newareaarray[0],
                                    mastername = masterdata.mastername,
                                    path = newareaarray[1],
                                    schoolid = Convert.ToInt32(newareaarray[2]),
                                    schoolname = newareaarray[3] == null ? "" : newareaarray[3],
                                    pids = ","+productkeys+","
                                });
                                #endregion
                            }
                        }


                        if (deptarealist != null && deptarealist.Count > 0)
                        {
                            foreach (vw_deptarea row in deptarealist)
                            {
                                if (row.deptid == masterdata.deptid)//部门为当前代理商所在部门时直接删除
                                {
                                    deldeptareaids += row.id + ",";
                                }
                                else
                                {
                                    #region 循环比对新选的区域
                                    int comparedresut = 4;
                                    foreach (string rownewarea in newarrayareas)//循环比对新设置的区域
                                    {
                                        string[] newareaarray = rownewarea.Split('|');
                                        int resut = AreaCompared(Convert.ToInt32(newareaarray[0]), Convert.ToInt32(newareaarray[2]), row.districtid, row.schoolid);
                                        if (resut < 4)
                                        {
                                            comparedresut = resut;
                                            if (resut == 1)
                                            {
                                                deldeptareaids += row.id + ",";

                                                #region 添加部门区域
                                                adddeptarealist.Add(new base_deptarea()
                                                {
                                                    deptid = row.deptid,
                                                    districtid = Convert.ToInt32(newareaarray[0]),
                                                    path = newareaarray[1],
                                                    schoolid = Convert.ToInt32(newareaarray[2]),
                                                    schoolname = newareaarray[3] == null ? "" : newareaarray[3],
                                                    isend = base.isend(Convert.ToInt32(newareaarray[0]), Convert.ToInt32(newareaarray[2]))
                                                });
                                                #endregion
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    if (comparedresut == 4)//循环所有新区域找不到相关区域就删除该部门对应区域记录
                                        deldeptareaids += row.id + ",";
                                    #endregion
                                }
                            }
                        }

                        if (masterlist != null && masterlist.Count > 0)
                        {
                            foreach (vw_areamaster row in masterlist)
                            {
                                if (row.mastername == masterdata.mastername)//用户为当前修改的代理商时直接删除
                                {
                                    delmasterareaids += row.id + ",";
                                }
                                else
                                {
                                    #region 循环比对新选的区域

                                    int comparedresut = 4;
                                    foreach (string rownewarea in newarrayareas)//循环比对新设置的区域
                                    {
                                        string[] newareaarray = rownewarea.Split('|');
                                        int resut = AreaCompared(Convert.ToInt32(newareaarray[0]), Convert.ToInt32(newareaarray[2]), Convert.ToInt32(row.districtid), row.schoolid);
                                        if (resut < 4)
                                        {
                                            comparedresut = resut;
                                            if (resut == 1)
                                            {
                                                delmasterareaids += row.id + ",";

                                                string rowpids = ProductComparedPids(productkeys, row.pids);
                                                if (!string.IsNullOrEmpty(rowpids))
                                                {
                                                    #region 添加用户区域
                                                    addmasterarealist.Add(new join_mastertarea()
                                                    {
                                                        districtid = newareaarray[0],
                                                        mastername = row.mastername,
                                                        path = newareaarray[1],
                                                        schoolid = Convert.ToInt32(newareaarray[2]),
                                                        schoolname = newareaarray[3] == null ? "" : newareaarray[3],
                                                        pids = AddFh(rowpids)
                                                    });
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (ProductCompared(productkeys, row.pids))
                                                {
                                                    delmasterareaids += row.id + ",";

                                                    string rowpids = ProductComparedPids(productkeys, row.pids);
                                                    if (!string.IsNullOrEmpty(rowpids))
                                                    {
                                                        #region 添加用户区域
                                                        addmasterarealist.Add(new join_mastertarea()
                                                        {
                                                            districtid = row.districtid,
                                                            mastername = row.mastername,
                                                            path = row.path,
                                                            schoolid = row.schoolid,
                                                            schoolname = row.schoolname == null ? "" : row.schoolname,
                                                            pids = AddFh(rowpids)
                                                        });
                                                        #endregion
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    if (comparedresut == 4)//循环所有新区域找不到相关区域就删除该部门对应区域记录
                                        delmasterareaids += row.id + ",";
                                    #endregion
                                }
                            }
                        }


                        var re = base.Manage.AgentDeleteArea<base_deptarea, join_mastertarea>(deldeptareaids.TrimEnd(','), adddeptarealist, delmasterareaids.TrimEnd(','), addmasterarealist);
                        if (re)
                        {
                            base.Update<base_dept>(new base_dept() { deptname = masterdata.agentname, deptid = masterdata.deptid, parentid = deptid, createtime = DateTime.Now.ToString(), createname = masterinfo.mastername }, new string[] { "districtid", "path", "isend", "schoolid", "schoolname", "delflg", "agentid", "level" });
                            res.Success = true;
                        }
                        else
                        {
                            res.ErrorMsg = "编辑失败！请重试~";
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
                else
                {
                    res.ErrorMsg = "编辑失败！请重试~";
                }
            }
            return Json(res);
        }

        #endregion

        /// <summary>
        /// 产品比对（判断产品是否减少）
        /// </summary>
        /// <param name="newpids"></param>
        /// <param name="pids"></param>
        /// <returns></returns>
        private bool ProductCompared(string newpids, string pids)
        {
            if (!string.IsNullOrEmpty(pids) && !string.IsNullOrEmpty(newpids))
            {
                string[] array = pids.Split(',');
                foreach (string row in array)
                {
                    if (!newpids.Contains(row))
                        return true;
                }
            }
            return false;
        }


        public string AddFh(string productId)
        {
            productId = productId.Trim();
            if (!string.IsNullOrEmpty(productId))
            {
                if (productId.Substring(0, 1) != ",")
                {
                    productId = "," + productId;
                }
                if (productId.Substring(productId.Length - 1, 1) != ",")
                {
                    productId += ",";
                }
            }
            return productId;
        }
        private string ProductComparedPids(string newpids, string pids)
        {
            newpids=newpids.TrimStart(',').TrimEnd(',');
            pids = pids.TrimStart(',').TrimEnd(',');
            if (newpids.Trim() == pids.Trim())
                return pids;
            string resut = "";
            if (!string.IsNullOrEmpty(pids) && !string.IsNullOrEmpty(newpids))
            {
                string[] array = pids.Split(',');
                foreach (string row in array)
                {
                    if (newpids.Contains(row))
                    {
                        resut += row + ",";
                    }
                }
            }
            return resut.TrimEnd(',');
        }

        /// <summary>
        /// 区域比对
        /// </summary>
        /// <param name="newarea">新区域</param>
        /// <param name="newschoolid">新学校</param>
        /// <param name="oldarea">原区域</param>
        /// <param name="oldschoolid">原学校</param>
        /// <returns>1 新区域比原区域小 2 相等  3新区域比原区域大 4 不沾边</returns>
        private int AreaCompared(int newarea, int newschoolid, int oldarea, int oldschoolid)
        {
            if (newschoolid > 0 && newschoolid == oldschoolid)
                return 2;

            if (newschoolid == oldschoolid && newarea == oldarea)
                return 2;
            if (newschoolid > 0 && oldschoolid == 0 && newarea == oldarea)
                return 1;
            if (newschoolid == 0 && oldschoolid > 0 && newarea == oldarea)
                return 3;

            if (newarea.ToString().Substring(0, 2) == oldarea.ToString().Substring(0, 2))
            {
                if (newarea.ToString().Substring(2, 2) != oldarea.ToString().Substring(2, 2))
                {
                    if (newarea.ToString().Substring(2, 2) == "00")
                        return 3;
                    else if (oldarea.ToString().Substring(2, 2) == "00")
                        return 1;
                }
                else
                {
                    if (newarea.ToString().Substring(4, 2) != oldarea.ToString().Substring(4, 2))
                    {
                        if (newarea.ToString().Substring(4, 2) == "00")
                            return 3;
                        else if (oldarea.ToString().Substring(4, 2) == "00")
                            return 1;
                    }
                }
            }
            return 4;
        }

        #region 获取查询条件
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        [AuthenticationAttribute(IsCheck = false)]
        public List<Expression<Func<vw_agent, bool>>> GetUserWheres(int dataauthority, string agentname, int deptid, out string Flide, out List<string> inids)
        {
            Recursive give = new Recursive();
            List<Expression<Func<vw_agent, bool>>> exprlist = new List<Expression<Func<vw_agent, bool>>>();

            if (deptid == 1)
                deptid = 0;

            Flide = "";
            inids = null;
            exprlist.Add(t => t.state == 0);
            if (dataauthority == (int)Dataauthority.全部)
            {
                if (deptid > 0)
                {
                    List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                    InIds.Add(deptid.ToString());//添加本部门
                    if (InIds != null && InIds.Count > 0)
                    {
                        Flide = "deptid";
                        inids = InIds;
                    }
                }
                exprlist.Add(t => t.pagentid == masterinfo.agentid);
            }
            else
            {
                if (dataauthority == (int)Dataauthority.本人下级代理商)
                {
                    exprlist.Add(t => (t.parentid == masterinfo.masterid));
                }
                else if (dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商 || dataauthority == (int)Dataauthority.本人下级部门下级代理商)
                {
                    int did = masterinfo.deptid;
                    if (deptid > 0)
                        did = deptid;
                    List<string> InIds = give.GetDeptNodeId(did, masterinfo.agentid);
                    if (dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商 && deptid == 0)
                        InIds.Add(masterinfo.deptid.ToString());
                    else
                        InIds.Add(did.ToString());
                    List<vw_deptAgent> listmaster = base.SelectIn<vw_deptAgent>("deptid", InIds);
                    Flide = "masterid";
                    List<string> masterinids = new List<string>();
                    if (listmaster != null && listmaster.Count > 0)
                    {
                        for (int i = 0; i < listmaster.Count; i++)
                        {
                            masterinids.Add(listmaster[i].masterid.ToString());
                        }
                    }
                    else
                    {
                        masterinids.Add("");
                    }
                    inids = masterinids;
                }
            }
            if (!string.IsNullOrEmpty(agentname))
                exprlist.Add(t => (t.agentname.Contains(agentname) || t.parentname.Contains(agentname)));
            return exprlist;
        }
        #endregion

        public JsonResult Agent_View(int pagesize, int pageindex, string agentname, int deptid)
        {
            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<vw_agent> pageParameter = new PageParameter<vw_agent>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            string flide = "";
            List<string> idslist = null;
            pageParameter.Wheres = GetUserWheres(masterinfo.dataauthority, agentname, deptid, out   flide, out idslist);
            pageParameter.Field = flide;
            pageParameter.In = idslist;
            pageParameter.OrderColumns = t1 => t1.masterid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<vw_agent> usre = base.Manage.SelectPage<vw_agent>(pageParameter, out total);
            EmployeeController employee = new EmployeeController();
            for (int i = 0; i < usre.Count; i++)
            {
                usre[i].responsiblearea = employee.GetArea(usre[i].mastername);
            }
            return Json(new { total = total, rows = usre });
        }

        #region 导出
        [HttpPost]
        public FileResult Agent_Export([System.Web.Http.FromBody]vw_user vw_userrInfo)
        {
            List<Expression<Func<vw_agent, bool>>> exprlist = new List<Expression<Func<vw_agent, bool>>>();


            string Field = "";
            List<string> In = null;



            List<vw_agent> list = base.SelectSearchs<vw_agent>(exprlist, Field, In, " masterid desc");

            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = "用户" + DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式
            EmployeeController employee = new EmployeeController();
            //给 sheet 添加第一行的头部标题         
            headerrow.CreateCell(0).SetCellValue("序号");
            headerrow.CreateCell(1).SetCellValue("代理商名称");
            headerrow.CreateCell(2).SetCellValue("用户名");
            headerrow.CreateCell(3).SetCellValue("负责人姓名");
            headerrow.CreateCell(4).SetCellValue("渠道经理");
            headerrow.CreateCell(5).SetCellValue("所属部门");
            headerrow.CreateCell(6).SetCellValue("负责区域");
            headerrow.CreateCell(7).SetCellValue("签约截止日期");
            for (int i = 0; i < list.Count; i++)
            {
                vw_agent userinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);               //新创建一行
                ICell cell = row.CreateCell(i);         //在新创建的一行中创建单元格
                cell.CellStyle = style;        //设置单元格格式
                row.CreateCell(0).SetCellValue(i + 1);        //给单元格赋值
                row.CreateCell(1).SetCellValue(userinfo.agentname);
                row.CreateCell(2).SetCellValue(userinfo.mastername);
                row.CreateCell(3).SetCellValue(userinfo.truename);
                row.CreateCell(4).SetCellValue(userinfo.parentname);
                row.CreateCell(5).SetCellValue(userinfo.deptname);
                row.CreateCell(6).SetCellValue(employee.GetArea(userinfo.mastername));
                row.CreateCell(7).SetCellValue(userinfo.agent_enddate.ToString("yyyy-MM-dd"));
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            /*这里判断使用的浏览器是否为Firefox
             * （1）
             * Firefox导出文件时不需要对文件名显示编码，编码后文件名会乱码
             * IE和Google需要编码才能保持文件名正常
             * 
             * （2）
             * 经过HttpUtility.UrlEncode方法加密过文件名后该方法将空格替换成了+号，用%20替换掉就可以正常显示了，
             * 但是这个方法在IE和谷歌里面可以解决问题，在火狐里面仍然无效，用%20替换+后输出的就是%20，并不会显示为空格，解决办法如下
             * */
            string UserAgent = System.Web.HttpContext.Current.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") == -1)
            {
                tmpTitle = HttpUtility.UrlEncode(tmpTitle, System.Text.Encoding.UTF8).Replace("+", "%20").Replace("%27", "'");
            }
            else
            {
                tmpTitle = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tmpTitle)) + "?=";
            }
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/excel", tmpTitle + ".xls");

        }
        #endregion

        #region 代理商拉黑
        /// <summary>
        /// 代理商拉黑
        /// </summary>
        /// <param name="masterid"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AgentPullBlack(string agentid)
        {
            if (!action.Pullblack)
                return false;

            if (agentid != null && !string.IsNullOrEmpty(agentid) && agentid.Trim() != Core.Utility.PublicHelp.OrgId)
            {
                Recursive sive = new Recursive();
                List<string> agentidlist = sive.GetNodsAgentId(agentid);//所有下级代理商ID
                agentidlist.Add(agentid);
                List<base_dept> deptlist = base.SelectIn<base_dept>(" agentid ", agentidlist, " deptid ");//获取部门ID
                List<int> deptids = new List<int>();
                if (deptlist != null && deptlist.Count > 0)
                    foreach (base_dept row in deptlist)
                        deptids.Add(row.deptid);
                List<com_master> masterlist = base.SelectIn<com_master>(" agentid ", agentidlist, " mastername ");//获取用户名
                List<string> masternames = new List<string>();
                if (masterlist != null && masterlist.Count > 0)
                    foreach (com_master row in masterlist)
                        masternames.Add(row.mastername);
                return base.UpdateDelete<base_deptarea, join_mastertarea, com_master>(bd => bd.deptid, deptids, ma => ma.mastername, masternames, new { state = 1, createtime = DateTime.Now }, o => agentidlist.Contains(o.agentid));
            }
            return false;
        }
        #endregion

        #region 获取当前用户负责的产品
        /// <summary>
        /// 获取当前用户负责的产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetProduct()
        {
            if (masterinfo.agentid == Core.Utility.PublicHelp.OrgId)
                return Json(base.SelectSearch<cfg_keyvalue>(t => t.UseType == "Channel"));
            List<com_master> master = base.SelectSearch<com_master>(m => m.agentid == masterinfo.agentid && m.mastertype == 1);
            if (master != null && master.Count > 0)
            {
                List<join_mastertarea> masterarea = base.SelectSearch<join_mastertarea>(m => m.mastername == master[0].mastername, 1, "id");
                if (masterarea != null && masterarea.Count > 0)
                {
                    List<Expression<Func<cfg_keyvalue, bool>>> exprlist = new List<Expression<Func<cfg_keyvalue, bool>>>();
                    exprlist.Add(t => t.UseType == "Channel");
                    List<string> list = new List<string>();
                    string[] array = masterarea[0].pids.Split(',');
                    foreach (string row in array)
                        list.Add(row);
                    return Json(base.SelectSearchs<cfg_keyvalue>(exprlist, "`Key`", list));
                }
            }
            return Json("");
        }
        #endregion

    }
}
