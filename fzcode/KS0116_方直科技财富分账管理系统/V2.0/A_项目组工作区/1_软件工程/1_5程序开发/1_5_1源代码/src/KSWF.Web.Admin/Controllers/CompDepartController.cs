using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KSWF.WFM.Constract.Models;
using KSWF.Core.Utility;
using Newtonsoft.Json;
using KSWF.Web.Admin.Models;
using System.Net;
using System.Net.Http;
using System.Configuration;
using System.Web.Caching;
using System.Text.RegularExpressions;
using KSWF.WFM.Constract.VW;

namespace KSWF.Web.Admin.Controllers
{
    public class CompDepartController : BaseController
    {

        public ActionResult Index(int schoolid = 0)
        {
            //测试用，不需要时是要删除
            //AreaService.ServiceSoapClient client1 = new AreaService.ServiceSoapClient();
            //string schoolstr = client1.GetSchoolInfo(schoolid);
            //if (schoolstr.IndexOf("错误|") == -1)
            //{
            //    KSWF.Web.Admin.Models.ViewSchoolInfo schoolInfo = JsonConvert.DeserializeObject<KSWF.Web.Admin.Models.ViewSchoolInfo>(schoolstr);
            //    if (schoolInfo != null)
            //    {
            //        ViewBag.ID = schoolInfo.ID;
            //        ViewBag.SchoolName = schoolInfo.SchoolName;
            //        ViewBag.DistrictID = schoolInfo.DistrictID;
            //        ViewBag.TownsID = schoolInfo.TownsID;
            //        ViewBag.Area = schoolInfo.Area;
            //        ViewBag.SchoolTypeNo = schoolInfo.SchoolTypeNo;
            //    }
            //}
            return View();
        }

        [HttpPost]
        public JsonResult CompDepart_GetDeptTree(int deptid,bool isMerge=true)
        {
            List<TreeView> res = new List<TreeView>();
            string rootname;
            if (masterinfo.agentid=="KSWF")
            {
                res = GetDeptTree(deptid);
                rootname = "方直科技";
            }
            else
            {
                if (isMerge)
                {
                    res = GetDeptTree(masterinfo.deptid);
                }
                else
                {
                    var root = base.Manage.SelectSearch<base_dept, base_deptarea>((x, y) => x.deptid == masterinfo.deptid && x.agentid == masterinfo.agentid && y.id != null, (x, y) => x.deptid == y.deptid);
                    TreeView deptTree = new TreeView
                    {
                        text = root[0].deptname,
                        Id = root[0].deptid.ToString(),
                        ParentId = root[0].parentid.ToString(),
                        Level=root[0].level
                    };
                    string strDistrictids = "";
                    foreach (var deptarea in root)
                    {
                        strDistrictids += deptarea.districtid.ToString() + "=" + deptarea.path + "=" + deptarea.isend + "|" + deptarea.schoolid.ToString() + "=" + deptarea.schoolname + ",";
                    }
                    strDistrictids = strDistrictids.Remove(strDistrictids.Length - 1);
                    deptTree.tag = strDistrictids;
                    deptTree.nodes = GetDeptTree(masterinfo.deptid);

                    res.Add(deptTree);
                }
                rootname = masterinfo.agentname;
            }
            return Json(new { Data = res, rootname = rootname });
        }

        private List<TreeView> GetDeptTree(int deptid)
        {
            var deptareas = base.Manage.SelectSearch<base_dept, base_deptarea>((x, y) => x.parentid == deptid && x.agentid == masterinfo.agentid && y.id != null, (x, y) => x.deptid == y.deptid);
            List<TreeView> returnDeptTrees = null;
            if (deptareas.Count > 0)
            {
                returnDeptTrees = new List<TreeView>();
                List<base_dept> depts = new List<base_dept>();
                int tempdeptid = -1;
                foreach (var item in deptareas)
                {
                    if (tempdeptid != item.deptid)
                    {
                        depts.Add(new base_dept
                        {
                            deptid = item.deptid,
                            deptname = item.deptname,
                            parentid = item.parentid,
                            level = item.level
                        });
                        tempdeptid = item.deptid;
                    }
                }

                foreach (var dept in depts)
                {
                    TreeView deptTree = new TreeView
                    {
                        text = dept.deptname,
                        Id = dept.deptid.ToString(),
                        ParentId = dept.parentid.ToString(),
                        Level=dept.level
                    };
                    string strDistrictids = "";
                    foreach (var deptarea in deptareas)
                    {
                        if (deptarea.deptid == dept.deptid)
                        {
                            strDistrictids += deptarea.districtid.ToString() + "=" + deptarea.path + "=" + deptarea.isend + "|" + deptarea.schoolid.ToString() + "=" + deptarea.schoolname + ",";
                        }
                    }
                    strDistrictids = strDistrictids.Remove(strDistrictids.Length - 1);
                    deptTree.tag = strDistrictids;
                    deptTree.nodes = GetDeptTree(dept.deptid);
                    returnDeptTrees.Add(deptTree);
                }
            }
            return returnDeptTrees;
        }

        [HttpPost]
        public JsonResult CompDepart_Move(int deptidF, int parentidF, int deptidS, int parentidS,int levelS,int levelF)
        {
            //更新son部门的父部门id，及相关部门的区域,需要新增区域的父级，需要减少区域的原父级
            //needRemoveDepts, newDeptareas , deptidS

            if (parentidS==deptidF)
            {
                return Json(KingResponse.GetErrorResponse("请选择不同的子级部门移动到上级部门中！"));
            }
            if (levelS < levelF)
            {
                return Json(KingResponse.GetErrorResponse("请选择子级部门移动到上级部门中！"));
            }

            //根据deptidS，找到所有的上级部门id，除去总公司，层数是不确定
            List<int> parentids = new List<int>();
            parentids.Add(parentidS);
            GetParentDept(parentidS, parentids);

            List<base_deptarea> deptareaS = base.Manage.SelectSearch<base_deptarea>(x => x.deptid == deptidS);

            //son级 到 与father级同级的原上级之间的 原来上级要减少区域（每一个上级）
            List<string> needRemoveDeptids = new List<string>();
            if (masterinfo.agentid != "KSWF")
            {
                needRemoveDeptids = parentids.Where(x => x > parentidF && x != masterinfo.deptid).Select(x => x.ToString()).ToList();
            }
            else
            {
                needRemoveDeptids = parentids.Where(x => x > parentidF && x != 1).Select(x => x.ToString()).ToList();
            }
            var needRemoveDeptareas = base.Manage.SelectIn<base_deptarea>("deptid", needRemoveDeptids);
            Dictionary<int, List<base_deptarea>> needRemoveDepts = new Dictionary<int, List<base_deptarea>>();
            foreach (var item in needRemoveDeptareas)
            {
                if (needRemoveDepts.ContainsKey(item.deptid))
                {
                    needRemoveDepts[item.deptid].Add(item);
                }
                else
                {
                    List<base_deptarea> temp = new List<base_deptarea>();
                    temp.Add(item);
                    needRemoveDepts.Add(item.deptid, temp);
                }
            }

            foreach (var needRemoveDept in needRemoveDepts)
            {
                RemoveArea(needRemoveDept.Value, deptareaS);
            }

            List<base_deptarea> newDeptareas = new List<base_deptarea>();

            //判断deptidF,在不在上级部门集合中
            if (!parentids.Contains(deptidF) || needRemoveDeptids.Contains(deptidF.ToString()))
            {
                //father级需要增加区域
                List<base_deptarea> deptareaF = new List<base_deptarea>();
                if (needRemoveDeptids.Contains(deptidF.ToString()))
                {
                    deptareaF.AddRange(needRemoveDepts[deptidF]);
                }
                else
                {
                    deptareaF = base.Manage.SelectSearch<base_deptarea>(x => x.deptid == deptidF);
                }
                newDeptareas.AddRange(deptareaS);
                newDeptareas.AddRange(deptareaF);

                //区域相加，合并区域中的区,市，省
                MergeSchool(newDeptareas);
                MergeArea("area", newDeptareas);
                MergeArea("city", newDeptareas);
                MergeArea("province", newDeptareas);
            }
            
            string errorMsg = "";
            if (base.Manage.TranMoveDept(newDeptareas, needRemoveDepts, deptidF, deptidS,masterinfo.agentid,out errorMsg))
            {
                return Json(KingResponse.GetResponse("移动成功！"));
            }
            else
            {
                if (errorMsg=="")
                {
                    errorMsg = "移动失败！";
                }
                return Json(KingResponse.GetErrorResponse(errorMsg));
            }
        }

        private void RemoveArea(List<base_deptarea> deptareaF, List<base_deptarea> deptareaS)
        {
            base_deptarea[] copyDeptareaF = new base_deptarea[deptareaF.Count];
            deptareaF.CopyTo(copyDeptareaF);

            for (int i = 0; i < copyDeptareaF.Length; i++)
            {
                foreach (var item in deptareaS)
                {
                    var strDistrictidF = copyDeptareaF[i].districtid.ToString();
                    var strDistrictidS = item.districtid.ToString();

                    //找到有关系的
                    //减少 父
                    //加 all子-子

                    if (copyDeptareaF[i].schoolid == 0 )
                    {
                        if (strDistrictidF.Substring(2, 7) == "0000000" && strDistrictidF.Substring(0, 2) == strDistrictidS.Substring(0, 2))
                        {
                            //省
                            if (item.schoolid == 0)
                            {
                                if (strDistrictidS.Substring(2, 7) == "0000000")
                                {
                                    //省 减 省
                                    if (strDistrictidF == strDistrictidS)
                                    {
                                        deptareaF.RemoveAll(x => x.id == copyDeptareaF[i].id);
                                    }
                                }
                                else if (strDistrictidS.Substring(4, 5) == "00000")
                                {
                                    //省 减 市
                                    base_deptarea cityinfo = new base_deptarea();
                                    var leftCitys = GetLeftAreas(copyDeptareaF[i], item.districtid.ToString(), cityinfo, deptareaS, deptareaF);
                                    deptareaF.RemoveAll(x => x.id == copyDeptareaF[i].id);
                                    deptareaF.AddRange(leftCitys);
                                }
                                else if (strDistrictidS.Substring(6, 3) == "000")
                                {
                                    //省 减 区
                                    base_deptarea cityinfo = new base_deptarea();
                                    string cityDistrictid = item.districtid.ToString().Substring(0, 4) + "00000";
                                    var leftCitys = GetLeftAreas(copyDeptareaF[i], cityDistrictid, cityinfo, deptareaS, deptareaF);
                                    deptareaF.RemoveAll(x => x.id == copyDeptareaF[i].id);
                                    deptareaF.AddRange(leftCitys);

                                    base_deptarea areainfo = new base_deptarea();
                                    var leftAreas = GetLeftAreas(cityinfo, item.districtid.ToString(), areainfo, deptareaS, deptareaF);
                                    deptareaF.AddRange(leftAreas);
                                }
                            }
                            else
                            {
                                //省 减 学校
                                base_deptarea cityinfo = new base_deptarea();
                                string cityDistrictid = item.districtid.ToString().Substring(0, 4) + "00000";
                                var leftCitys = GetLeftAreas(copyDeptareaF[i], cityDistrictid, cityinfo, deptareaS, deptareaF);
                                deptareaF.RemoveAll(x => x.id == copyDeptareaF[i].id);
                                deptareaF.AddRange(leftCitys);

                                base_deptarea areainfo = new base_deptarea();
                                var leftAreas = GetLeftAreas(cityinfo, item.districtid.ToString(), areainfo, deptareaS, deptareaF);
                                deptareaF.AddRange(leftAreas);

                                var leftSchools = GetLeftSchools(areainfo, item.schoolid, deptareaS, deptareaF);
                                deptareaF.AddRange(leftSchools);
                            }
                        }
                        else if (strDistrictidF.Substring(4, 5) == "00000" && strDistrictidF.Substring(0, 4) == strDistrictidS.Substring(0, 4))
                        {
                            //市
                            if (item.schoolid == 0)
                            {
                                if (strDistrictidS.Substring(4, 5) == "00000")
                                {
                                    //市 减 市
                                    if (strDistrictidF == strDistrictidS)
                                    {
                                        deptareaF.RemoveAll(x => x.id == copyDeptareaF[i].id);
                                    }
                                }
                                else if (strDistrictidS.Substring(6, 3) == "000")
                                {
                                    //市 减 区
                                    base_deptarea areainfo = new base_deptarea();
                                    var leftAreas = GetLeftAreas(copyDeptareaF[i], item.districtid.ToString(), areainfo, deptareaS, deptareaF);
                                    deptareaF.RemoveAll(x => x.id == copyDeptareaF[i].id);
                                    deptareaF.AddRange(leftAreas);

                                }
                            }
                            else
                            {
                                //市 减 学校
                                base_deptarea areainfo = new base_deptarea();
                                var leftAreas = GetLeftAreas(copyDeptareaF[i], item.districtid.ToString(), areainfo, deptareaS, deptareaF);
                                deptareaF.RemoveAll(x => x.id == copyDeptareaF[i].id);
                                deptareaF.AddRange(leftAreas);

                                var leftSchools = GetLeftSchools(areainfo, item.schoolid, deptareaS, deptareaF);
                                deptareaF.AddRange(leftSchools);
                            }
                        }
                        else if (strDistrictidF.Substring(6, 3) == "000" && strDistrictidF.Substring(0, 6) == strDistrictidS.Substring(0, 6))
                        {
                            //区 
                            if (strDistrictidS.Substring(6, 3) == "000")
                            {
                                //区 减 区
                                if (strDistrictidF == strDistrictidS)
                                {
                                    deptareaF.RemoveAll(x => x.id == copyDeptareaF[i].id);
                                }
                            }
                            else if (item.schoolid > 0)
                            {
                                //区 减 学校
                                var leftSchools = GetLeftSchools(copyDeptareaF[i], item.schoolid, deptareaS, deptareaF);
                                deptareaF.RemoveAll(x => x.id == copyDeptareaF[i].id);
                                deptareaF.AddRange(leftSchools);
                            }
                        }
                    }
                    else
                    {
                        //学校
                        if (item.schoolid > 0 && strDistrictidF == strDistrictidS)
                        {
                            //学校 减 学校
                            if (copyDeptareaF[i].schoolid == item.schoolid)
                            {
                                deptareaF.RemoveAll(x => x.id == copyDeptareaF[i].id);
                            }
                        }
                    }
                }
            }

        }

        private List<base_deptarea> GetLeftAreas(base_deptarea higherArea, string districtid, base_deptarea areainfo, List<base_deptarea> deptareaS, List<base_deptarea> deptareaF)
        {
            var areas = GetAreas(higherArea.districtid);

            List<base_deptarea> ret = new List<base_deptarea>();
            foreach (var item in areas)
            {
                if (!deptareaS.Select(x => x.districtid.ToString()).ToList().Contains(item.ID) && !deptareaF.Select(x => x.districtid.ToString()).ToList().Contains(item.ID))
                {
                    if (item.ID != districtid)
                    {
                        base_deptarea temp = new base_deptarea();
                        temp.deptid = higherArea.deptid;
                        temp.districtid = Convert.ToInt32(item.ID);
                        temp.isend = 0;
                        temp.path = item.Path;
                        temp.schoolid = 0;
                        temp.schoolname = "";
                        ret.Add(temp);
                    }
                    else
                    {
                        areainfo.deptid = higherArea.deptid;
                        areainfo.districtid = Convert.ToInt32(item.ID);
                        areainfo.isend = 0;
                        areainfo.path = item.Path;
                        areainfo.schoolid = 0;
                        areainfo.schoolname = "";
                    }
                }
            }
            return ret;
        }

        private List<base_deptarea> GetLeftSchools(base_deptarea area, int schoolid, List<base_deptarea> deptareaS, List<base_deptarea> deptareaF)
        {
            var schools = GetSchools(area.districtid);

            List<base_deptarea> ret = new List<base_deptarea>();
            foreach (var item in schools)
            {
                if (!deptareaS.Select(x => x.schoolid).ToList().Contains(item.ID) && !deptareaF.Select(x => x.schoolid).ToList().Contains(item.ID))
                {
                    if (item.ID != schoolid)
                    {
                        base_deptarea temp = new base_deptarea();
                        temp.deptid = area.deptid;
                        temp.districtid = area.districtid;
                        temp.isend = 2;
                        temp.path = area.path;
                        temp.schoolid = item.ID;
                        temp.schoolname = item.SchoolName;
                        ret.Add(temp);
                    }
                }
            }
            return ret;
        }

        private void GetParentDept(int deptid, List<int> parentids)
        {
            var dept = base.Manage.SelectSearch<base_dept>(x => x.deptid == deptid && x.agentid == masterinfo.agentid);
            if (dept.Count > 0)
            {
                var parentid = dept[0].parentid;
                parentids.Add(parentid);
                if (parentid != 1)
                {
                    GetParentDept(parentid, parentids);
                }
            }
        }

        [HttpPost]
        public JsonResult CompDepart_Merge(string deptname, int deptidA, int parentidA, string districtidsA, int deptidB, int parentidB, string districtidsB, int mergeType)
        {
            // mergeType: 1 上下级部门， 2 同级部门
            // A 为上级， B为下级

            List<base_dept> depts = Manage.SelectSearch<base_dept>(x => x.deptname == deptname && x.agentid == masterinfo.agentid && x.deptid != deptidA);
            if (depts.Count > 0)
            {
                return Json(KingResponse.GetErrorResponse("部门名称不能相同！"));
            }
            //找到部门A的 下级部门，员工，代理商
            List<int> deptIDsA = new List<int>();
            GetChildDeptID(deptidA, deptIDsA);

            List<int> agentDeptIDsA = new List<int>();
            GetAgentChildDeptID(deptidA, agentDeptIDsA);

            //找到部门B的 下级部门，员工，代理商
            List<int> deptIDsB = new List<int>();
            GetChildDeptID(deptidB, deptIDsB);

            List<int> agentDeptIDsB = new List<int>();
            GetAgentChildDeptID(deptidB, agentDeptIDsB);

            bool result = true;
            if (mergeType == 1)
            {
                //数据库操作
                result = base.Manage.TranMergeUpLowerLevelDept(deptname, deptidA, deptidB, deptIDsB, agentDeptIDsB, deptIDsA, agentDeptIDsA);
            }
            else
            {
                //解析部门A，部门B 的区域
                List<base_deptarea> deptareaA = AnalysisDis(districtidsA);
                List<base_deptarea> deptareaB = AnalysisDis(districtidsB);
                List<base_deptarea> newDeptareas = new List<base_deptarea>();
                newDeptareas.AddRange(deptareaA);
                newDeptareas.AddRange(deptareaB);

                //区域相加，合并区域中的区,市，省
                MergeSchool(newDeptareas);
                MergeArea("area", newDeptareas);
                MergeArea("city", newDeptareas);
                MergeArea("province", newDeptareas);

                //通过父部门ID，获取父部门区域
                var oldDeptarea = base.Manage.SelectSearch<base_deptarea>(x => x.deptid == parentidA);

                //判断是否与父部门区域相等
                bool isEqual = true;
                foreach (var item in oldDeptarea)
                {
                    if (!newDeptareas.Contains(item, new CompareDeptArea1()))
                    {
                        isEqual = false;
                    }
                }
                //通过判断，进行合并，部门A保存不变，部门B更新下级子部门，员工，代理商
                if (isEqual == true)
                {
                    return Json(KingResponse.GetErrorResponse("不能合并，合并后的区域与父级相等！"));
                }

                result = base.Manage.TranMergeBrotherLevelDept(deptname, deptidA, deptidB, deptIDsB, agentDeptIDsB, newDeptareas, deptIDsA, agentDeptIDsA);
            }
            if (result)
            {
                return Json(KingResponse.GetResponse("合并成功！"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("合并失败！"));
            }
        }

        private void MergeSchool(List<base_deptarea> needFilterDeptarea)
        {
            Dictionary<int, List<base_deptarea>> newSchools = new Dictionary<int, List<base_deptarea>>();
            foreach (var newDeptarea in needFilterDeptarea)
            {
                if (newDeptarea.schoolid > 0)
                {
                    if (newSchools.ContainsKey(newDeptarea.districtid))
                    {
                        newSchools[newDeptarea.districtid].Add(newDeptarea);
                    }
                    else
                    {
                        List<base_deptarea> tempSchool = new List<base_deptarea>();
                        tempSchool.Add(newDeptarea);
                        newSchools.Add(newDeptarea.districtid, tempSchool);
                    }
                }
            }

            Dictionary<int, List<base_deptarea>> needMergeSchools = new Dictionary<int, List<base_deptarea>>();
            foreach (var newSchool in newSchools)
            {
                //通过区域id，获取学校
                //对比数量是否一致
                var oldSchoolCount = GetSchoolCount(newSchool.Key);
                var newSchoolCount = newSchool.Value.Count;
                if (oldSchoolCount == newSchoolCount)
                {
                    needMergeSchools.Add(newSchool.Key, newSchool.Value);
                }
            }

            foreach (var needMergeSchool in needMergeSchools)
            {
                foreach (var schoolDeptarea in needMergeSchool.Value)
                {
                    needFilterDeptarea.Remove(schoolDeptarea);
                }

                var area = GetAreaInfo(Convert.ToInt32(needMergeSchool.Key));
                needFilterDeptarea.Add(new base_deptarea
                {
                    districtid = needMergeSchool.Key,
                    schoolid = 0,
                    path = area.Path,
                    isend = 1 
                });
            }
        }

        private void MergeArea(string areaType, List<base_deptarea> needFilterDeptarea)
        {
            Dictionary<string, List<base_deptarea>> newAreas = new Dictionary<string, List<base_deptarea>>();

            switch (areaType)
            {
                case "area":
                    foreach (var newDeptarea in needFilterDeptarea)
                    {
                        string strNewDistrictid = newDeptarea.districtid.ToString();
                        if (strNewDistrictid.Substring(2, 7) != "0000000"
                   && strNewDistrictid.Substring(4, 5) != "00000"
                   && strNewDistrictid.Substring(6, 3) == "000")
                        {
                            string key = strNewDistrictid.Substring(0, 4) + "00000";
                            if (newAreas.ContainsKey(key))
                            {
                                newAreas[key].Add(newDeptarea);
                            }
                            else
                            {
                                List<base_deptarea> tempArea = new List<base_deptarea>();
                                tempArea.Add(newDeptarea);
                                newAreas.Add(key, tempArea);
                            }
                        }
                    }
                    break;
                case "city":
                    foreach (var newDeptarea in needFilterDeptarea)
                    {
                        string strNewDistrictid = newDeptarea.districtid.ToString();
                        if (strNewDistrictid.Substring(2, 7) != "0000000"
                   && strNewDistrictid.Substring(4, 5) == "00000")
                        {
                            string key = strNewDistrictid.Substring(0, 2) + "0000000";
                            if (newAreas.ContainsKey(key))
                            {
                                newAreas[key].Add(newDeptarea);
                            }
                            else
                            {
                                List<base_deptarea> tempArea = new List<base_deptarea>();
                                tempArea.Add(newDeptarea);
                                newAreas.Add(key, tempArea);
                            }
                        }
                    }
                    break;
                case "province":
                    foreach (var newDeptarea in needFilterDeptarea)
                    {
                        string strNewDistrictid = newDeptarea.districtid.ToString();
                        if (strNewDistrictid.Substring(2, 7) == "0000000")
                        {
                            string key = "0";
                            if (newAreas.ContainsKey(key))
                            {
                                newAreas[key].Add(newDeptarea);
                            }
                            else
                            {
                                List<base_deptarea> tempArea = new List<base_deptarea>();
                                tempArea.Add(newDeptarea);
                                newAreas.Add(key, tempArea);
                            }
                        }
                    }
                    break;
            }

            Dictionary<string, List<base_deptarea>> needMergeAreas = new Dictionary<string, List<base_deptarea>>();
            foreach (var newArea in newAreas)
            {
                var oldAreaCount = GetAreaCount(Convert.ToInt32(newArea.Key));
                var newAreaCount = newArea.Value.Count;
                if (oldAreaCount == newAreaCount)
                {
                    needMergeAreas.Add(newArea.Key, newArea.Value);
                }
            }

            foreach (var needMergeArea in needMergeAreas)
            {
                foreach (var areaDeptarea in needMergeArea.Value)
                {
                    needFilterDeptarea.Remove(areaDeptarea);
                }

                var area=GetAreaInfo(Convert.ToInt32(needMergeArea.Key));
                base_deptarea temparea = new base_deptarea
                {
                    districtid = Convert.ToInt32(needMergeArea.Key),
                    schoolid = 0,
                    path=area.Path,
                    isend = 0
                };
                
                needFilterDeptarea.Add(temparea);
            }
        }

        private List<base_deptarea> AnalysisDis(string strDistrictids)
        {
            List<base_deptarea> deptareas = new List<base_deptarea>();
            if (!string.IsNullOrEmpty(strDistrictids))
            {
                var districtids = strDistrictids.Split(new char[] { ',' });
                for (int i = 0; i < districtids.Length; i++)
                {
                    base_deptarea addDeptarea = new base_deptarea();
                    var deptarea = districtids[i].Split(new char[] { '|' });
                    if (deptarea.Count() > 1)
                    {
                        var area = deptarea[0].Split(new char[] { '=' });
                        var school = deptarea[1].Split(new char[] { '=' });
                        if (area.Count() > 2)
                        {
                            addDeptarea.districtid = Convert.ToInt32(area[0]);
                            addDeptarea.path = area[1];
                            addDeptarea.isend = Convert.ToInt32(area[2]);
                        }
                        addDeptarea.schoolid = Convert.ToInt32(school[0]);
                        if (school.Count() > 1)
                        {
                            addDeptarea.schoolname = school[1];
                        }

                    }
                    deptareas.Add(addDeptarea);
                }
            }
            return deptareas;
        }

        private void GetChildDeptID(int deptid, List<int> deptIDs)
        {
            List<base_dept> depts = new List<base_dept>();
            depts = base.Manage.SelectSearch<base_dept>(x => x.parentid == deptid && x.agentid == masterinfo.agentid);
            if (depts.Count > 0)
            {
                foreach (var dept in depts)
                {
                    deptIDs.Add(dept.deptid);
                    //GetChildDeptID(dept.deptid, deptIDs);//后代
                }
            }
        }

        private void GetAgentChildDeptID(int deptid, List<int> deptIDs)
        {
            List<base_dept> depts = new List<base_dept>();
            depts = base.Manage.SelectSearch<base_dept>(x => x.parentid == deptid && x.agentid != masterinfo.agentid);
            if (depts.Count > 0)
            {
                foreach (var dept in depts)
                {
                    deptIDs.Add(dept.deptid);
                }
            }
        }

        /// <summary>
        /// 获取action显示控制权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetcurrentAction()
        {
            return Json(action);
        }

        [HttpPost]
        public JsonResult CompDepart_Details(int pagesize, int pageindex)
        {
            int deptId = 0;
            string agentid = masterinfo.agentid;
            if (masterinfo.agentid != "KSWF")
            {
                deptId = masterinfo.deptid;
            }
            if (UserIdentity == 1 && masterinfo.dataauthority == (int)Dataauthority.全部)
            {
                List<com_master> master = base.SelectSearch<com_master>(t => t.agentid == masterinfo.agentid && t.mastertype == 1);
                if (master != null && master.Count > 0)
                {
                    List<base_dept> deptlist = base.SelectSearch<base_dept>(t => t.deptid == master[0].deptid);
                    if (deptlist != null && deptlist.Count > 0)
                    {
                        deptId = deptlist[0].parentid;
                    }
                }
            }
            PageParameter<base_dept> pageParameter = new PageParameter<base_dept>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.Where = t1 => (t1.agentid == agentid && t1.parentid == deptId);
            pageParameter.OrderColumns = t1 => t1.deptid;
            int total;
            var listdeptarea = base.Manage.SelectPage<base_dept, base_deptarea>(pageParameter, (t1, t2) => t1.deptid == t2.deptid && t1.delflg == 0, out total);
            List<base_dept> rows = GetFormatDept(listdeptarea.ToList());
            return Json(new { total = rows.Count, rows = rows });
        }

        [HttpPost]
        public JsonResult CompDepart_DetailsChildren(int parentid)
        {
            //连表获取所有区域对应子区域的集合
            var listdeptarea = base.Manage.SelectSearch<base_dept, base_deptarea>(t1 => t1.parentid == parentid && t1.agentid == masterinfo.agentid && t1.delflg == 0
                , (t1, t2) => t1.deptid == t2.deptid);

            List<base_dept> rows = GetFormatDept(listdeptarea);
            return Json(new { total = rows.Count, rows = rows });
        }

        [HttpPost]
        public JsonResult CompDepart_DetailsByID(int deptid)
        {
            var list = base.Manage.SelectSearch<base_dept, base_deptarea>(t1 => t1.deptid == deptid && t1.agentid == masterinfo.agentid && t1.delflg == 0
                , (t1, t2) => t1.deptid == t2.deptid);
            if (list != null && list.Count > 0)
            {
                List<TreeView> treeViews = new List<TreeView>();
                foreach (var school in list)
                {
                    TreeView treeView = new TreeView();
                    if (school.districtid > 0)
                    {
                        if (school.schoolid != "0")
                        {
                            treeView.Id = school.schoolid.ToString();
                            treeView.tag = school.districtid.ToString() + "|2=" + school.path + "?" + school.schoolid + "?" + school.schoolname;
                        }
                        else
                        {
                            treeView.Id = school.districtid.ToString();
                            treeView.tag = school.districtid.ToString() + "|0=" + school.path;
                        }
                        treeView.text = school.path + school.schoolname;
                        treeView.schoolname = school.schoolname;
                        CheckedCheck s = new CheckedCheck();
                        s.@checked = false;
                        treeView.state = s;
                        treeViews.Add(treeView);
                    }
                }
                string districtids = "";
                foreach (var item in list)
                {
                    //area.ID + "|" + area.IsEnd + "=" + area.Path;
                    //school.DistrictID + "|2=" + school.Area + "?" + school.ID + "?" + school.SchoolName;
                    if (item.schoolid == "0")
                    {
                        districtids += item.districtid + "|" + item.isend + "=" + item.path + ",";
                    }
                    else
                    {
                        districtids += item.districtid + "|2=" + item.path + "?" + item.schoolid + "?" + item.schoolname + ",";
                    }
                }
                districtids = districtids.Remove(districtids.Length - 1, 1);
                int DeptareaCount = list.Count;
                List<base_dept> rows = GetFormatDept(list);
                int grandfatherid = 0;
                if (rows.Count > 0 && rows[0].parentid > 0)
                {
                    grandfatherid = base.Manage.SelectSearch<base_dept>(x => x.deptid == rows[0].parentid).First().parentid;
                }
                return Json(new { total = rows.Count, rows = rows, treeViews = treeViews, grandfatherid = grandfatherid, districtids = districtids, deptareaCount = DeptareaCount });
            }
            return Json(KingResponse.GetErrorResponse("获取失败！"));
        }

        [HttpPost]
        public JsonResult CompDepart_GetSelectedAreas(int deptid)
        {
            var listdeptarea = base.Manage.SelectSearch<base_dept, base_deptarea>(t1 => t1.deptid == deptid && t1.agentid == masterinfo.agentid && t1.delflg == 0
                , (t1, t2) => t1.deptid == t2.deptid);
            var result = KingResponse.GetResponse(listdeptarea);
            return Json(result);
        }

        /// <summary>
        /// 格式化显示负责区域
        /// </summary>
        /// <param name="listdeptarea"></param>
        /// <returns></returns>
        private static List<base_dept> GetFormatDept(List<base_dept> listdeptarea)
        {
            //筛选父区域，path拼接显示所有子区域
            List<base_dept> rows = new List<base_dept>();
            foreach (var deptarea in listdeptarea)
            {
                if (!rows.Contains<base_dept>(deptarea, new CompareDept()))
                {
                    rows.Add(deptarea);
                }
            }
            foreach (var row in rows)
            {
                int startIndex = 1;
                foreach (var deptarea in listdeptarea)
                {
                    if (deptarea.deptid == row.deptid)
                    {
                        if (startIndex != 1)
                        {
                            if (deptarea.schoolname != null && !string.IsNullOrEmpty(deptarea.schoolname))
                            {
                                row.path += "、" + deptarea.schoolname;
                            }
                            else
                            {
                                row.path += "、" + deptarea.path;
                            }
                        }
                        else
                        {
                            if (deptarea.schoolname != null && !string.IsNullOrEmpty(deptarea.schoolname))
                            {
                                row.path += " " + row.schoolname;
                            }
                        }
                        startIndex++;
                    }
                }
            }
            return rows;
        }

        [HttpPost]
        public JsonResult CompDepart_GetAreas(int deptid, int parentid)
        {
            //根据deptid，在base_deptarea,获取到districtid集合
            List<TreeView> treeViews = new List<TreeView>();
            if (masterinfo.agentid == Core.Utility.PublicHelp.OrgId && parentid == 0)
            {
                TreeView baseTreeView = new TreeView();
                baseTreeView.text = "全国";
                baseTreeView.tag = "0|0=0";
                baseTreeView.nodes = new List<TreeView>();
                CheckedCheck check = new CheckedCheck();
                check.showcheckbox = true;
                baseTreeView.state = check;
                treeViews.Add(baseTreeView);
                return Json(KingResponse.GetResponse(treeViews));
            }
            var listArea = base.Manage.SelectSearch<base_deptarea>(x => x.deptid == deptid);
            foreach (var area in listArea)
            {
                TreeView treeView = new TreeView();
                if (HasChild(area.districtid))
                {
                    treeView.nodes = new List<TreeView>();
                }
                else
                {
                    area.isend = 1;
                }
                //新增学校逻辑
                if (area.schoolname != null && !string.IsNullOrEmpty(area.schoolname))
                {
                    treeView.text = area.schoolname;
                    treeView.tag = area.districtid.ToString() + "|2=" + area.path + "?" + area.schoolid + "?" + area.schoolname;
                }
                else
                {
                    treeView.text = area.path;
                    treeView.tag = area.districtid.ToString() + "|" + area.isend + "=" + area.path;
                }
                CheckedCheck check = new CheckedCheck();
                if (AreaFilter(new base_deptarea() { districtid = area.districtid, schoolid = area.schoolid }, deptid))
                    check.showcheckbox = false;
                else
                    check.showcheckbox = true;
                treeView.state = check;
                treeViews.Add(treeView);
            }
            var result = KingResponse.GetResponse(treeViews);
            return Json(result);
        }

        private bool HasChild(int districtid)
        {
            AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
            var strChildArea = client.GetAreaData(districtid);
            var listChildArea = JsonConvert.DeserializeObject<List<AreaView>>(strChildArea);
            bool hasChild = false;
            if (listChildArea != null && listChildArea.Count > 0)
            {
                hasChild = listChildArea[0].IsEnd == "1" ? false : true;
            }
            return hasChild;
        }

        [HttpPost]
        public JsonResult CompDepart_GetChildrenAreas(string parentid, int deptid)
        {
            List<TreeView> treeViews = new List<TreeView>();
            string[] strPars = parentid.Split(new char[] { '|' });

            AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
            if (strPars.Length > 0)
            {
                string[] isendAndPath = strPars[1].Split(new char[] { '=' });
                if (isendAndPath.Length > 0)
                {
                    if (isendAndPath[0] == "0" && !string.IsNullOrEmpty(strPars[0]))
                    {
                        var strArea = client.GetAreaData(Convert.ToInt32(strPars[0]));
                        var listArea = JsonConvert.DeserializeObject<List<AreaView>>(strArea);
                        if (listArea != null && listArea.Count > 0 && listArea[0].IsEnd != "1")
                        {
                            foreach (var area in listArea)
                            {
                                TreeView treeView = new TreeView();
                                treeView.text = area.CodeName;
                                if (HasChild(Convert.ToInt32(listArea[0].ID)))
                                {
                                    treeView.nodes = new List<TreeView>();
                                }
                                else
                                {
                                    area.IsEnd = "1";
                                }
                                treeView.Id = area.ID;
                                treeView.tag = area.ID + "|" + area.IsEnd + "=" + area.Path;
                                CheckedCheck check = new CheckedCheck();
                                if (AreaFilter(new base_deptarea() { districtid = int.Parse(area.ID) }, deptid))
                                    check.showcheckbox = false;
                                else
                                    check.showcheckbox = true;
                                treeView.state = check;
                                treeViews.Add(treeView);


                                //  List<base_deptarea> deptareas = AnalysisDistrict(obj.districtids);//选择传来的

                            }
                        }
                    }
                }
            }

            var result = KingResponse.GetResponse(treeViews);
            return Json(result);
        }

        [HttpPost]
        public JsonResult CompDepart_GetSchools(string tag, int deptid)
        {
            List<TreeView> treeViews = new List<TreeView>();
            string[] strPars = tag.Split(new char[] { '|' });

            AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
            string areaId = strPars[0].Substring(0, strPars[0].Length - 3) + "000";
            string strAllSchool = client.GetSchoolData(areaId, "", "");
            var listAllSchool = JsonConvert.DeserializeObject<List<ViewSchoolInfo>>(strAllSchool);
            var strSchoolTypeNo = ConfigurationManager.AppSettings["SchoolTypeNo"];
            var schoolTypeNos = strSchoolTypeNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var school in listAllSchool)
            {
                if (schoolTypeNos.Contains(school.SchoolTypeNo))
                {
                    TreeView treeView = new TreeView();
                    treeView.text = school.SchoolName;
                    treeView.ParentId = school.DistrictID.ToString();
                    treeView.tag = school.DistrictID.ToString() + "|2=" + school.Area + "?" + school.ID + "?" + school.SchoolName;
                    //430202000|2= 湖南省 株洲市 荷塘区?103790?湖南省株洲市云龙示范区三搭桥小学
                    CheckedCheck check = new CheckedCheck();
                    base_deptarea deptarea = new base_deptarea() { districtid = int.Parse(school.DistrictID), schoolid = school.ID };
                    if (AreaFilter(deptarea, deptid))
                        check.showcheckbox = false;
                    else
                        check.showcheckbox = true;
                    treeView.state = check;
                    treeViews.Add(treeView);
                }
            }
            HttpContext.Cache.Remove("schools");
            HttpContext.Cache.Add("schools", treeViews, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), CacheItemPriority.Normal, null
                );
            var result = KingResponse.GetResponse(treeViews);
            return Json(result);
        }

        [HttpPost]
        public JsonResult CompDepart_GetSearchSchools(string searchKey, string areaId)
        {
            var result = KingResponse.GetResponse(GetSearchSchools(searchKey, "schools"));
            return Json(result);
        }

        [HttpPost]
        public JsonResult CompDepart_SaveAdd(DeptSaveView obj)
        {
            if (!ModelState.IsValid)
            {
                return Json(KingResponse.GetErrorResponse("输入参数有误！"));
            }
            List<base_dept> depts = Manage.SelectSearch<base_dept>(x => x.deptname == obj.deptname && x.agentid == masterinfo.agentid);
            if (depts.Count > 0)
            {
                return Json(KingResponse.GetErrorResponse("部门名称不能相同！"));
            }
            base_dept addDept = new base_dept
            {
                deptname = obj.deptname,
                parentid = obj.parentid,
                createname = masterinfo.mastername,
                agentid = masterinfo.agentid,
                createtime=DateTime.Now.ToString(),
                level=obj.level+1//在父部门的层级上加1
            };

            if (obj.districtids != null)
            {
                //获取相关区域集合
                List<base_deptarea> deptareas = AnalysisDistrict(obj.districtids);//选择传来的

                string msg = "";
                if (CheckDeptArea(deptareas, obj, out msg))
                {
                    //准备实体，事务插入，父子表
                    RelationEntity<base_dept, base_deptarea> relationEntity = new RelationEntity<base_dept, base_deptarea>();
                    relationEntity.ParentEntity = addDept;
                    relationEntity.ParentDisableColumns = new string[] { "districtid", "path", "isend", "schoolid", "schoolname"};
                    relationEntity.ParentIdName = "deptid";
                    relationEntity.ChildrenEntities = deptareas;

                    var result = base.Manage.TransactionAdd<base_dept, base_deptarea>(relationEntity);
                    return Json(KingResponse.GetResponse("新增成功！"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse(msg));
                }
            }
            else
            {
                if (base.Manage.Add<base_dept>(addDept, new string[] { "districtid", "path", "isend", "schoolid", "schoolname" }) > 0)
                {
                    return Json(KingResponse.GetResponse("新增成功！"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("新增失败"));
                }
            }

        }

        [HttpPost]
        public JsonResult CompDepart_SaveEdit(DeptSaveView obj)
        {

            if (!ModelState.IsValid)
            {
                return Json(KingResponse.GetErrorResponse("输入参数有误！"));
            }
            base_dept dept = new base_dept();
            dept.deptid = obj.deptid;
            dept.deptname = obj.deptname;
            dept.parentid = obj.parentid;
            dept.level = obj.level;
            List<base_dept> depts = Manage.SelectSearch<base_dept>(x => x.deptname == obj.deptname && x.agentid == masterinfo.agentid && x.deptid != obj.deptid);
            if (depts.Count > 0)
            {
                return Json(KingResponse.GetErrorResponse("部门名称不能相同！"));
            }

            string[] disUpdateColums = new string[] { "path", "isend", "agentid", "districtid", "createname", "schoolid", "schoolname"};

            //判断区域是否互斥

            //获取相关区域集合
            List<base_deptarea> newDeptareas = AnalysisDistrict(obj.districtids, obj.deptid);//选择传来的

            List<base_dept> childDepts = base.Manage.SelectSearch<base_dept>(x => x.parentid == obj.deptid);
            foreach (var item in childDepts)
            {
                List<base_deptarea> childDeptareas = base.Manage.SelectSearch<base_deptarea>(x => x.deptid == item.deptid);
                if (newDeptareas.Count == childDeptareas.Count)
                {
                    bool isEqual = true;
                    foreach (var item1 in newDeptareas)
                    {
                        if (!childDeptareas.Contains(item1, new CompareDeptArea()))
                        {
                            isEqual = false;
                        }
                    }
                    if (isEqual)
                    {
                        return Json(KingResponse.GetErrorResponse("部门修改后不能与子级区域相等！"));
                    }
                }
            }

            List<base_deptarea> parentDeptareas = base.Manage.SelectSearch<base_deptarea>(x => x.deptid == obj.parentid);
            if (newDeptareas.Count==parentDeptareas.Count)
            {
                bool isEqual = true;
                foreach (var item in newDeptareas)
                {
                    if (!parentDeptareas.Contains(item,new CompareDeptArea()))
                    {
                        isEqual = false;
                    }
                }
                if (isEqual)
                {
                    return Json(KingResponse.GetErrorResponse("部门修改后不能与父级区域相等！"));
                }
            }

            List<base_deptarea> oldDeptareas = base.Manage.SelectSearch<base_deptarea>(x => x.deptid == obj.deptid);

            //新增区域，不变区域，减少区域
            List<base_deptarea> uniqueNewDeptareas = new List<base_deptarea>();
            foreach (var item in newDeptareas)
            {
                if (!oldDeptareas.Contains(item, new CompareDeptArea1()))
                {
                    uniqueNewDeptareas.Add(item);
                }
            }

            List<base_deptarea> uniqueOldDeptareas = new List<base_deptarea>();
            foreach (var item in oldDeptareas)
            {
                if (!newDeptareas.Contains(item, new CompareDeptArea1()))
                {
                    uniqueOldDeptareas.Add(item);
                }
            }

            string msg = "";
            //检查新增区域
            if (CheckDeptArea(uniqueNewDeptareas, obj, out msg))
            {
                //减少的区域要判断，是否有子部门，员工，代理商有负责
                List<base_deptarea> effectDepts = new List<base_deptarea>();
                List<string> effectSchoolID = new List<string>();
                List<string> effectDistrictID = new List<string>();

                //通过减少的区域找到受影响的部门
                foreach (var uniqueOldDeptarea in uniqueOldDeptareas)
                {
                    if (uniqueOldDeptarea.schoolid > 0)
                    {
                        effectSchoolID.Add(uniqueOldDeptarea.schoolid.ToString());
                    }
                    else
                    {
                        effectDistrictID.Add(uniqueOldDeptarea.districtid.ToString());
                    }
                }

                if (uniqueOldDeptareas.Count > 0 && effectSchoolID.Count > 0)
                {
                    effectDepts = base.Manage.SelectIn<base_deptarea, base_dept>("x.deptid>" + obj.deptid + " and y.agentid='KSWF'", "x.schoolid", effectSchoolID, (x, y) => x.deptid == y.deptid);
                }
                if (effectDistrictID.Count > 0 && effectDistrictID.Count > 0)
                {
                    effectDepts.AddRange(base.Manage.SelectIn<base_deptarea, base_dept>("x.deptid>" + obj.deptid + " and y.agentid='KSWF'", "x.districtid", effectDistrictID, (x, y) => x.deptid == y.deptid));
                }
                var rootDepts = effectDepts.Distinct(new CompareDeptArea2()).ToList();
                List<string> effectDeptIds = new List<string>();

                //受影响的相关子部门
                foreach (var rootDept in rootDepts)
                {
                    GetChlildDept(rootDept.deptid, effectDeptIds, "KSWF");
                }
                var deptareas = base.Manage.SelectIn<base_deptarea>("deptid", effectDeptIds.Distinct().ToList());
                var endUniqueEffectDeptids = GetNotAffectDistrict(deptareas, uniqueOldDeptareas);

                if (endUniqueEffectDeptids.Count > 0)
                {
                    return Json(KingResponse.GetErrorResponse("减少了相关子部门的区域，无法修改！"));
                }

                //所有员工
                List<string> allDepts = new List<string>();
                GetChlildDept(obj.deptid, allDepts, "KSWF");
                List<string> endEmployees = new List<string>();
                var employees = base.Manage.SelectIn<com_master>("mastertype=0", "deptid", allDepts);
                if (employees.Count > 0)
                {
                    var join_masterarea_employees = base.Manage.SelectIn<join_mastertarea>("mastername", employees.Select(x => x.mastername).ToList());
                    endEmployees = GetNotAffectDistrict(join_masterarea_employees, uniqueOldDeptareas);
                }

                if (endEmployees.Count > 0)
                {
                    return Json(KingResponse.GetErrorResponse("减少了相关员工的区域，无法修改！"));
                }

                //所有代理商
                List<string> endAgents = new List<string>();
                var agents = base.Manage.SelectIn<com_master>("mastertype=1", "parentname", employees.Select(x => x.mastername).ToList());
                if (agents.Count > 0)
                {
                    var join_masterarea_agents = base.Manage.SelectIn<join_mastertarea>("mastername", agents.Select(x => x.mastername).ToList());
                    endAgents = GetNotAffectDistrict(join_masterarea_agents, uniqueOldDeptareas);
                }

                if (endAgents.Count > 0)
                {
                    return Json(KingResponse.GetErrorResponse("减少了相关代理商的区域，无法修改！"));
                }

                if (base.Manage.TranUpdate<base_dept, base_deptarea>(dept, disUpdateColums, newDeptareas))
                {
                    return Json(KingResponse.GetResponse("修改成功！"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("修改失败"));
                }
            }
            else
            {
                return Json(KingResponse.GetErrorResponse(msg));
            }
        }

        public bool AreaFilter(base_deptarea deptdate, int deptid)
        {
            List<base_deptarea> deptareas = new List<base_deptarea>();
            deptareas.Add(deptdate);
            DeptSaveView dept = new DeptSaveView() { parentid = deptid };
            string msg = "";
            return CheckDeptArea(deptareas, dept, out msg);
        }

        public bool CheckDeptArea(List<base_deptarea> deptareas, DeptSaveView obj, out string msg)
        {
            if (deptareas.Count > 0)
            {
                //遍历每个区域id，判断有无互斥
                foreach (var item in deptareas)
                {
                    //通过区域id，解析出所有的父区域id
                    List<string> parentDistrictIds = new List<string>();
                    string districtid = item.districtid.ToString();
                    parentDistrictIds.Add("0");//全国
                    parentDistrictIds.Add(districtid.Substring(0, districtid.Length - 7) + "0000000");//省
                    parentDistrictIds.Add(districtid.Substring(0, districtid.Length - 5) + "00000");//市
                    parentDistrictIds.Add(districtid.Substring(0, districtid.Length - 3) + "000");//区

                    var listDeptArea = base.Manage.SelectSearch<base_deptarea>(x => x.districtid == item.districtid);
                    listDeptArea.AddRange(base.Manage.SelectIn<base_deptarea>("districtid", parentDistrictIds.Distinct().ToList()));

                    //排除所有的父部门负责的相关区域
                    List<string> parentDeptids = new List<string>();
                    GetParentDept(obj.parentid, parentDeptids);
                    var listParentArea = base.Manage.SelectIn<base_deptarea>("deptid", parentDeptids).Distinct().ToList();

                    if (item.schoolid != 0)
                    {
                        List<base_deptarea> filterDeptSchools = new List<base_deptarea>();
                        var listDeptAreaSchool = base.Manage.SelectSearch<base_deptarea>(x => x.schoolid == item.schoolid);//学校
                        foreach (var item4 in listDeptAreaSchool)
                        {
                            if (!listParentArea.Contains<base_deptarea>(item4, new CompareDeptArea3()))
                            {
                                filterDeptSchools.Add(item4);
                            }
                        }
                        if (filterDeptSchools.Count > 0)
                        {
                            msg = "不同部门之间不能选择相同的学校！";
                            return false;
                        }
                    }

                    //注意排除代理商的父级负责区域
                    List<base_deptarea> filterDeptareas = new List<base_deptarea>();
                    foreach (var item3 in listDeptArea)
                    {
                        if (item.schoolid != 0)
                        {
                            if (!listParentArea.Contains<base_deptarea>(item3, new CompareDeptArea2()) && item3.schoolid == 0)
                            {
                                filterDeptareas.Add(item3);
                            }
                        }
                        else
                        {
                            if (!listParentArea.Contains<base_deptarea>(item3, new CompareDeptArea2()))
                            {
                                filterDeptareas.Add(item3);
                            }
                        }

                    }

                    if (filterDeptareas.Count > 0)
                    {
                        msg = "选择区域已使用，请勿重复选择！";
                        return false;
                    }

                    int exclusion = 0;
                    //排除相关同级部门的父区域
                    var listbrotherDept = base.Manage.SelectSearch<base_dept, base_deptarea>(t1 => t1.parentid == obj.parentid && t1.delflg == 0, (t1, t2) => t1.deptid == t2.deptid);
                    foreach (var item4 in listbrotherDept)
                    {
                        if (item.schoolid != 0)
                        {
                            if (item4.districtid > 0 && item4.schoolid == "0")
                            {
                                if (IsExclusion(item4.districtid, item.districtid))
                                {
                                    exclusion++;
                                }
                            }
                        }
                        else
                        {
                            if (item4.districtid > 0)
                            {
                                if (IsExclusion(item4.districtid, item.districtid))
                                {
                                    exclusion++;
                                }
                            }
                        }

                    }
                    if (exclusion > 0)
                    {
                        msg = "选择区域已使用，请勿重复选择！";
                        //msg = "不能选择同级部门的负责区域及父区域！或代理商的负责区域！";
                        return false;
                    }

                    //判断部门员工的区域是否互斥
                    List<vw_masterarea> masterareas = new List<vw_masterarea>();
                    if (item.schoolid > 0)
                    {
                        masterareas = base.Manage.SelectSearch<vw_masterarea>("mastertype=0 and agentid='" + masterinfo.agentid + "' and schoolid=" + item.schoolid);
                        //向上查学校所在区域，市，省有没有员工负责
                        masterareas.AddRange(base.Manage.SelectIn<vw_masterarea>("mastertype=0 and agentid='" + masterinfo.agentid+"'", "districtid", parentDistrictIds.Distinct().ToList()));
                    }
                    else
                    {
                        masterareas = base.Manage.SelectIn<vw_masterarea>("mastertype=0 and agentid='" + masterinfo.agentid + "' and schoolid=0", "districtid", parentDistrictIds.Distinct().ToList());
                        if (districtid.Substring(2, 7) == "0000000")
                        {
                            masterareas.AddRange(base.Manage.SelectSearch<vw_masterarea>("mastertype=0 and agentid='" + masterinfo.agentid + "' and left(districtid,2)='" + districtid.Substring(0, 2) + "'"));
                        }
                        else
                        {
                            if (districtid.Substring(4, 5) == "00000")
                            {
                                masterareas.AddRange(base.Manage.SelectSearch<vw_masterarea>("mastertype=0 and agentid='" + masterinfo.agentid + "' and left(districtid,4)='" + districtid.Substring(0, 4) + "'"));
                            }
                            else
                            {
                                if (districtid.Substring(6, 3) == "000")
                                {
                                    masterareas.AddRange(base.Manage.SelectSearch<vw_masterarea>("mastertype=0 and agentid='" + masterinfo.agentid + "' and left(districtid,6)='" + districtid.Substring(0, 6) + "'"));
                                }
                            }
                        }

                    }

                    if (masterareas.Count > 0)
                    {
                        msg = "选择区域已使用，请勿重复选择！";
                        return false;
                    }
                }
            }
            msg = "";
            return true;
        }

        public void GetParentDept(int deptid, List<string> allParentDeptIds)
        {
            var parentBasedept = base.Manage.SelectSearch<base_dept>(x => x.deptid == deptid && x.delflg == 0);
            if (parentBasedept.Count > 0)
            {
                allParentDeptIds.Add(parentBasedept.First().deptid.ToString());
                //代理商的父部门负责区域是否互斥
                if (parentBasedept.First().parentid != 0)
                {
                    GetParentDept(parentBasedept.First().parentid, allParentDeptIds);
                }
            }
        }

        private List<base_deptarea> AnalysisDistrict(string districtids, int deptid = 0)
        {
            //解析格式  districtid|isend=path，districtid2|isend2=path2
            //area.ID + "|" + area.IsEnd + "=" + area.Path;
            //school.DistrictID + "|2=" + school.Area + "?" + school.ID + "?" + school.SchoolName;

            List<base_deptarea> deptareas = new List<base_deptarea>();
            if (!string.IsNullOrEmpty(districtids))
            {
                string[] districts = districtids.Split(new char[] { ',' });
                foreach (var item in districts)
                {
                    string[] district = item.Split(new char[] { '=' });
                    base_deptarea deptarea = new base_deptarea();
                    if (district.Count() > 0)
                    {
                        if (deptid > 0)
                        {
                            deptarea.deptid = deptid;
                        }
                        string[] districtAndIsend = district[0].Split(new char[] { '|' });
                        if (districtAndIsend.Count() > 1)
                        {
                            deptarea.districtid = Convert.ToInt32(districtAndIsend[0]);
                            deptarea.isend = Convert.ToInt32(districtAndIsend[1]);
                            string[] schools = district[1].Split(new char[] { '?' });
                            if (schools.Count() > 2)
                            {
                                deptarea.path = schools[0];
                                if (!string.IsNullOrEmpty(schools[1]))
                                    deptarea.schoolid = Convert.ToInt32(schools[1]);
                                else
                                    deptarea.schoolid = 0;
                                deptarea.schoolname = schools[2];
                            }
                            else
                            {
                                deptarea.path = district[1];
                            }
                            deptareas.Add(deptarea);
                        }
                    }
                }
            }
            return deptareas;
        }

        [HttpPost]
        public JsonResult CompDepart_Del(int deptid)
        {
            List<base_dept> allNeedDel = new List<base_dept>();
            var basearea = base.Manage.SelectSearch<base_dept>(x => x.deptid == deptid && x.delflg == 0);
            allNeedDel.AddRange(basearea);
            var listarea = base.Manage.SelectSearch<base_dept>(x => x.parentid == deptid && x.delflg == 0 && x.agentid==masterinfo.agentid);
            GetListArea(listarea, allNeedDel);
            string[] deleteIds = new string[allNeedDel.Count];
            for (int i = 0; i < allNeedDel.Count; i++)
            {
                deleteIds[i] = allNeedDel[i].deptid.ToString();
            }
            //判断部门下有没有员工及子部门
            var existMaseters = base.Manage.SelectIn<com_master>("state=0", "deptid", deleteIds.ToList());
            var existChildDepts = base.Manage.SelectSearch<base_dept>(x => x.parentid == deptid && x.agentid==masterinfo.agentid);
            KingResponse response;
            if (existMaseters.Count > 0)
            {
                response = KingResponse.GetErrorResponse("部门下有员工，不能删除！");
            }
            else if (existChildDepts.Count > 0)
            {
                response = KingResponse.GetErrorResponse("部门下有子部门，不能删除！");
            }
            else
            {
                var result = base.Manage.TransactionDelete<base_dept, base_deptarea>(deleteIds, x => x.deptid, deleteIds);
                response = result == true ? KingResponse.GetResponse("删除成功！") : KingResponse.GetErrorResponse("删除失败！");
            }
            return Json(response);
        }

        private void GetListArea(List<base_dept> listarea, List<base_dept> all)
        {
            all.AddRange(listarea);
            foreach (var item in listarea)
            {
                var listareaChild = base.Manage.SelectSearch<base_dept>(x => x.parentid == item.deptid && x.delflg == 0);
                if (listareaChild.Count > 0)
                {
                    GetListArea(listareaChild, all);
                }
            }
        }
    }

    public class CompareDept : IEqualityComparer<base_dept>
    {

        public bool Equals(base_dept x, base_dept y)
        {
            return x.deptid == y.deptid;
        }

        public int GetHashCode(base_dept obj)
        {
            return obj.GetHashCode();
        }
    }

    public class CompareDeptArea : IEqualityComparer<base_deptarea>
    {

        public bool Equals(base_deptarea x, base_deptarea y)
        {
            return x.districtid == y.districtid;
        }

        public int GetHashCode(base_deptarea obj)
        {
            return obj.GetHashCode();
        }
    }

    public class CompareDeptArea1 : IEqualityComparer<base_deptarea>
    {

        public bool Equals(base_deptarea x, base_deptarea y)
        {
            if (x.schoolid > 0 || y.schoolid > 0)
            {
                return x.districtid == y.districtid && x.schoolid == y.schoolid;
            }
            else
            {
                return x.districtid == y.districtid;
            }
        }

        public int GetHashCode(base_deptarea obj)
        {
            return obj.GetHashCode();
        }
    }
    public class CompareDeptArea2 : IEqualityComparer<base_deptarea>
    {

        public bool Equals(base_deptarea x, base_deptarea y)
        {
            return x.deptid == y.deptid;
        }

        public int GetHashCode(base_deptarea obj)
        {
            return obj.GetHashCode();
        }
    }

    public class CompareDeptArea3 : IEqualityComparer<base_deptarea>
    {

        public bool Equals(base_deptarea x, base_deptarea y)
        {
            return x.schoolid == y.schoolid;
        }

        public int GetHashCode(base_deptarea obj)
        {
            return obj.GetHashCode();
        }
    }
}
