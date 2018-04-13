using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KSWF.WFM.Constract.Models;
using KSWF.Core.Utility;
using KSWF.WFM.Constract.VW;
using KSWF.Web.Admin.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace KSWF.Web.Admin.Controllers
{
    public class RoyaltyPolicyMgrController : BaseController
    {
        /// <summary>
        /// 提成策略
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RoyaltyPolicyMgr_Add()
        {

            ViewBag.ChannleList = GetAgendChannel(); //获取产品渠道来源
            return View();
        }
        public ActionResult RoyaltyPolicyMgr_Eidt()
        {
            ViewBag.ChannleList = GetAgendChannel(); //获取产品渠道来源
            return View();
        }



        #region 获取控制权限
        /// <summary>
        /// 获取控制权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetcurrentAction()
        {
            return Json(action);
        }
        #endregion

        #region view

        #region 查看
        /// <summary>
        /// 查看 
        /// </summary>
        /// <param name="policyid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RoyaltyPolicyMgrt_View(int pagesize, int pageindex, int ptype)
        {
            if (!action.View)
                return Json("");
            PageParameter<vw_bpolicy> pageParameter = new PageParameter<vw_bpolicy>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.Where = t1 => (t1.delflg == 0 && t1.ptype == ptype && t1.agentid == masterinfo.agentid);//根据权限加载的参数
            pageParameter.OrderColumns = t1 => t1.bid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<vw_bpolicy> usre = base.Manage.SelectPage<vw_bpolicy>(pageParameter, out total);
            if (usre != null && usre.Count > 0)
            {
                for (int i = 0; i < usre.Count; i++)
                {
                    string version = "";
                    string category = "";
                    string class_divided = "";
                    string divided = "";
                    List<vw_bpolicyproduct> list = base.SelectSearch<vw_bpolicyproduct>(p => p.bid == usre[i].bid);
                    if (list != null && list.Count > 0)
                    {
                        foreach (vw_bpolicyproduct row in list)
                        {
                            version += row.version == null || row.version == "" ? " " : row.version + ",";
                            category += row.category == null || row.category == "" ? " " : row.category + ",";
                            class_divided += row.class_divided.ToString() == null || row.category == "" ? " " : (row.class_divided * 100).ToString() + "%,";
                            divided += row.divided.ToString() == null || row.divided.ToString() == "" ? " " : (row.divided * 100).ToString() + "%,";
                        }
                    }
                    usre[i].version = version.TrimEnd(',');
                    usre[i].category = category.TrimEnd(',');
                    usre[i].class_divided = class_divided.TrimEnd(',');
                    usre[i].divided = divided.TrimEnd(',');
                }
            }
            return Json(new { total = total, rows = usre });
        }

        #endregion

        #region  复制
        /// <summary>
        ///  复制
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public JsonResult RoyaltyPolicyMgr_Copy(int bid)
        {
            cfg_bpolicy entity = base.Select<cfg_bpolicy>(bid.ToString());
            entity.pllicyname = entity.pllicyname + "--复本";
            if (entity != null)
            {
                List<cfg_bpolicyproduct> list = base.SelectSearch<cfg_bpolicyproduct>(t => t.bid == bid);
                if (list != null && list.Count > 0)
                {
                    //准备实体，事务插入，父子表
                    RelationEntity<cfg_bpolicy, cfg_bpolicyproduct> relationEntity = new RelationEntity<cfg_bpolicy, cfg_bpolicyproduct>();
                    relationEntity.ParentEntity = entity;
                    relationEntity.ParentIdName = "bid";
                    relationEntity.ChildrenEntities = list;
                    if (base.Manage.TransactionAdd<cfg_bpolicy, cfg_bpolicyproduct>(relationEntity))
                        return Json(KingResponse.GetResponse("复制成功！"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("该策略下没有分成比例~"));
                }
            }
            return Json(KingResponse.GetErrorResponse("复制失败~请重试！"));
        }
        #endregion

        #region 删除(逻辑删除)
        /// <summary>
        ///  删除(逻辑删除) 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult Bpolicypr_Del(int Id)
        {
            Core.Utility.KingResponse res = new KingResponse();
            if (!action.Del)
            {
                res.ErrorMsg = "您没有删除权限~";
            }
            else if (BpolicyprMaster(Id))
            {
                res.ErrorMsg = "该策略已生效！不可删除~";
            }
            else
            {
                if (base.LogicDelete<cfg_bpolicy>(t => t.bid == Id, "delflg"))
                    res.Success = true;
            }
            return Json(res);
        }
        #endregion

        #endregion

        #region add

        #region 添加产品策略
        /// <summary>
        /// 添加策略
        /// </summary>
        /// <param name="entiyt"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BpolicyAdd(string jsonData)
        {
            if (!action.Add)
                return Json(KingResponse.GetErrorResponse("您没有新增权限！"));
            var entity = JsonConvert.DeserializeObject<bpolicy>(jsonData);
            entity.cfg_bpolicy.createname = masterinfo.mastername;
            entity.cfg_bpolicy.createtime = DateTime.Now.ToString();
            entity.cfg_bpolicy.agentid = masterinfo.agentid;
            if (GetCount(entity.cfg_bpolicy.bid, entity.cfg_bpolicy.pllicyname) > 0)
            {
                return Json(KingResponse.GetErrorResponse("策略名称已存在！"));
            }
            RelationEntity<cfg_bpolicy, cfg_bpolicyproduct> relationEntity = new RelationEntity<cfg_bpolicy, cfg_bpolicyproduct>();
            relationEntity.ParentEntity = entity.cfg_bpolicy;
            relationEntity.ParentIdName = "bid";
            relationEntity.ChildrenEntities = entity.cfg_bpolicyproducts;

            var result = base.Manage.TransactionAdd<cfg_bpolicy, cfg_bpolicyproduct>(relationEntity);
            return Json(KingResponse.GetResponse("新增成功！"));
        }
        #endregion

        #region 加载分类及版本
        /// <summary>
        ///  版本 
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="categoryid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetVersion(int productid, string categoryid)
        {
            if (UserIdentity == 1)
            {
                List<vw_agentproduct> list = base.SelectSearch<vw_agentproduct>(vw => vw.agentid == masterinfo.agentid && vw.pid == productid);
                List<string> versionidlist = new List<string>();
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].versionid == 0)
                        {
                            List<cfg_product> productlist;
                            if (!string.IsNullOrEmpty(categoryid) && categoryid != "0")
                            {
                                productlist = base.SelectGroupBy<cfg_product>(x => (x.channel == productid && x.categorykey == Convert.ToInt32(categoryid)), "versionid");
                            }
                            else
                            {
                                productlist=base.SelectGroupBy<cfg_product>(x => x.channel == productid, "versionid");
                            }
                            return Json(AddVersionId(productlist));
                        }
                        else
                        {
                            versionidlist.Add(list[i].versionid.ToString());
                        }
                    }
                    if (versionidlist.Count > 0)
                    {
                        List<cfg_product> productlist;
                        if (!string.IsNullOrEmpty(categoryid) && categoryid != "0")
                        {
                            productlist = base.SelectGroupBy<cfg_product>(x => (x.channel == productid && x.categorykey == Convert.ToInt32(categoryid)), "versionid", "versionid", versionidlist);
                        }
                        else
                        {
                            productlist = base.SelectGroupBy<cfg_product>(x => x.channel == productid, "versionid", "versionid", versionidlist);
                        }
                       return Json(productlist);
                    }
                }
            }
            else
            {

                List<cfg_product> productlist;
                if (!string.IsNullOrEmpty(categoryid) && categoryid != "0")
                {
                    productlist = base.SelectGroupBy<cfg_product>(x => (x.channel == productid && x.categorykey == Convert.ToInt32(categoryid)), "versionid");
                }
                else
                {
                    productlist = base.SelectGroupBy<cfg_product>(x => x.channel == productid, "versionid");
                }
                return Json(AddVersionId(productlist));
            }
            return Json("");
        }
        /// <summary>
        /// 版本增加全部
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<cfg_product> AddVersionId(List<cfg_product> list)
        {
            list.Insert(0, new cfg_product() { version = "全部", versionid = 0 });
            return list;
        }
        /// <summary>
        /// 分类增加全部
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<cfg_product> AddCategoryId(List<cfg_product> list)
        {
            list.Insert(0, new cfg_product() { categorykey = 0, category = "全部" });
            return list;
        }

        /// <summary>
        ///  分类 
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="categoryid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCategoryid(int productid, int versionid)
        {
            if (UserIdentity == 1)
            {
                List<vw_agentproduct> list = base.SelectSearch<vw_agentproduct>(vw => vw.agentid == masterinfo.agentid && vw.pid == productid);
                List<string> versionidlist = new List<string>();
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].categorykey == 0)
                        {
                            List<cfg_product> productlist;
                            if (versionid > 0)
                            {
                                productlist = base.SelectGroupBy<cfg_product>(x => (x.channel == productid && x.versionid == versionid && x.isshevel == 1 && x.delflg == 0), "categorykey");
                            }
                            else
                            {
                                productlist = base.SelectGroupBy<cfg_product>(x => (x.channel == productid && x.isshevel == 1 && x.delflg==0), "categorykey");
                            }

                            return Json(AddCategoryId(productlist));
                        }
                        else
                        {
                            versionidlist.Add(list[i].categorykey.ToString());
                        }
                    }
                    if (versionidlist.Count > 0)
                    {
                        List<cfg_product> productlist;
                        if (versionid > 0)
                        {
                            productlist = base.SelectGroupBy<cfg_product>(x => (x.channel == productid && x.versionid == versionid && x.isshevel == 1 && x.delflg == 0), "categorykey", "categorykey", versionidlist);
                        }
                        return Json(base.SelectGroupBy<cfg_product>(x => (x.channel == productid && x.isshevel == 1 && x.delflg==0), "categorykey", "categorykey", versionidlist));
                    }
                }
            }
            else
            {
                List<cfg_product> productlist;
                if (versionid > 0)
                {
                    productlist = base.SelectGroupBy<cfg_product>(x => (x.channel == productid && x.versionid == versionid && x.isshevel == 1 && x.delflg==0), "categorykey");
                }
                else
                {
                    productlist = base.SelectGroupBy<cfg_product>(x => (x.channel == productid && x.isshevel == 1 && x.delflg == 0), "categorykey");
                }
                return Json(AddCategoryId(productlist));
            }
            return Json("");
        }
        #endregion


        #endregion

        //获取提成比例
        public JsonResult GetProportion(int Categoryid, int VersionId,int pid)
        {
            if (UserIdentity == 1)
            {
                string master = "";
                List<com_master> lsitmaser = base.SelectSearch<com_master>(t=>t.agentid==masterinfo.agentid && t.mastertype==1);
                master = lsitmaser[0].mastername;
                List<vw_agentproduct> list = base.SelectSearch<vw_agentproduct>(vw => (vw.agentid == masterinfo.agentid && vw.mastername == master && vw.versionid == VersionId && vw.categorykey == Categoryid && vw.pid == pid));
                if (list != null && list.Count > 0)
                {
                    return Json(new { Proportion = list[0].divided * 100, classProportion = list[0].class_divided * 100 });
                }
                else
                {
                    List<vw_agentproduct> list2 = base.SelectSearch<vw_agentproduct>(vw => (vw.agentid == masterinfo.agentid && vw.mastername == master && vw.versionid == 0 && vw.categorykey == Categoryid && vw.pid == pid));
                    if (list2 != null && list2.Count > 0)
                    {
                        return Json(new { Proportion = list2[0].divided * 100, classProportion = list2[0].class_divided * 100 });
                    }
                    else
                    {

                        List<vw_agentproduct> list3 = base.SelectSearch<vw_agentproduct>(vw => (vw.agentid == masterinfo.agentid && vw.mastername == master && vw.versionid == VersionId && vw.categorykey == 0 && vw.pid == pid));
                        if (list3 != null && list3.Count > 0)
                        {
                            return Json(new { Proportion = list3[0].divided * 100, classProportion = list3[0].class_divided * 100 });
                        }
                        else
                        {
                            List<vw_agentproduct> list4 = base.SelectSearch<vw_agentproduct>(vw => (vw.agentid == masterinfo.agentid && vw.mastername == master && vw.versionid == 0 && vw.categorykey == 0 && vw.pid == pid));
                            if (list4 != null && list4.Count > 0)
                            {
                                return Json(new { Proportion = list4[0].divided * 100, classProportion = list4[0].class_divided * 100 });
                            }
                        }
                    }
                }
                return Json("");
            }
            else
            {
                return Json(new { Proportion = 100, classProportion = 100 });
            }
        }


        #region edit

        #region 根据ID获取策略详细
        /// <summary>
        /// 根据ID获取策略详细 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRoyaltyPolickById(string Id)
        {
            return Json(base.Select<cfg_bpolicy>(Id));
        }
        #endregion

        #region 编辑产品策略

        /// <summary>
        /// 编辑产品策略
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BpolicyEdit(string jsonData)
        {
            if (!action.Edit)
                return Json(KingResponse.GetErrorResponse("您没有编辑权限！~"));
            var entity = JsonConvert.DeserializeObject<bpolicy>(jsonData);
            entity.cfg_bpolicy.createname = masterinfo.mastername;
            entity.cfg_bpolicy.createtime = DateTime.Now.ToString();
            entity.cfg_bpolicy.agentid = masterinfo.agentid;
            if (BpolicyprMaster(entity.cfg_bpolicy.bid))
                return Json(KingResponse.GetErrorResponse("该策略已生效！不可编辑~"));
            else if (GetCount(entity.cfg_bpolicy.bid, entity.cfg_bpolicy.pllicyname) > 0)
                return Json(KingResponse.GetErrorResponse("策略名称已存在！~"));
            if (base.Update<cfg_bpolicy>(entity.cfg_bpolicy) && base.Manage.UpdateActionBusinessAffairs<cfg_bpolicyproduct>(cb => cb.bid == entity.cfg_bpolicy.bid, entity.cfg_bpolicyproducts) > 0)
                return Json(KingResponse.GetResponse("编辑成功！"));
            return Json(KingResponse.GetErrorResponse("编辑失败！请重试~"));

        }
        #endregion

        #endregion

        #region 查看策略下分成比例  1（暂时无用）bpolicyproduct_View(int policyid, int pagesize, int pageindex)
        /// <summary>
        /// 查看策略下分成比例  1（暂时无用）
        /// </summary>
        /// <param name="policyid">策略id</param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult bpolicyproduct_View(int policyid, int pagesize, int pageindex)
        {
            if (!action.Add)
                return Json("");
            PageParameter<vw_bpolicyproduct> pageParameter = new PageParameter<vw_bpolicyproduct>();
            if (pageindex == 0)
                pageindex = pageindex / pagesize;
            else
                pageindex = pageindex / pagesize + 1;
            pageParameter.PageIndex = pageindex;
            pageParameter.PageSize = pagesize;
            pageParameter.Where = t1 => t1.bid == policyid;
            pageParameter.OrderColumns = t1 => t1.id;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<vw_bpolicyproduct> usre = base.Manage.SelectPage<vw_bpolicyproduct>(pageParameter, out total);
            return Json(new { total = total, rows = usre });
        }
        #endregion

        #region 删除分成比例(逻辑删除) bpolicyproduct_Del(string ids)
        /// <summary>
        /// 删除分成比例(逻辑删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public bool bpolicyproduct_Dels(string ids)
        {
            if (!action.Del)
                return false;
            return base.DeleteMore<cfg_bpolicyproduct>(ids.TrimEnd(','));
        }
        #endregion

        #region 删除分成比例(物理删除、未添加任务判断) bpolicyproduct_Del(int ID)
        /// <summary>
        /// 删除分成比例物理删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BpolicyProduct_Del(int ID, int bid)
        {
            Core.Utility.KingResponse res = new KingResponse();
            if (!action.Del)
                res.ErrorMsg = "您没有删除权限！~";
            else if (BpolicyprMaster(bid))
                res.ErrorMsg = "该策略已生效！不可删除提成比例~";
            else if (base.DeleteById<cfg_bpolicyproduct>(ID))
                res.Success = true;
            return Json(res);
        }
        #endregion

        #region 根据名称获取策略的条数 GetCount(int Id, string pllicyname)
        /// <summary>
        /// 根据名称获取策略的条数
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="pllicyname"></param>
        /// <returns></returns>
        public int GetCount(int Id, string pllicyname)
        {
            if (Id > 0)
                return base.GetTotalCount<cfg_bpolicy>(t => (t.bid != Id && t.agentid == masterinfo.agentid && t.delflg == 0 && t.pllicyname == pllicyname.Trim()));
            else
                return base.GetTotalCount<cfg_bpolicy>(t => (t.delflg == 0 && t.agentid == masterinfo.agentid && t.pllicyname == pllicyname.Trim()));
        }
        #endregion

        /// <summary>
        /// 判断策略是否与用户关联
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool BpolicyprMaster(int Id)
        {
            return base.GetTotalCount<KSWF.WFM.Constract.Models.join_masterbpolicypr>(t => (t.bid == Id && t.startdate < DateTime.Now)) > 0;
        }


    }
}
