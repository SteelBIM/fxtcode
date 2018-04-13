using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KSWF.WFM.Constract.Models;
using KSWF.Framework.BLL;
using KSWF.WFM.Constract.VW;
using KSWF.Web.Admin.Models;
using Newtonsoft.Json;

namespace KSWF.Web.Admin.Controllers
{
    /// <summary>
    /// 获取接口区域
    /// </summary>
    public class ServiceArea
    {
        AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public string GetArea(int AreaId)
        {
            return client.GetAreaData(AreaId);
        }
        /// <summary>
        /// 获取学校
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public string GetSchool(string AreaId)
        {
            return client.GetSchoolData(AreaId, "", "");
        }
        Manage manage = new Manage();
        /// <summary>
        /// 部门合并
        /// </summary>
        /// <param name="olddeptid">合并的部门ID</param>
        /// <param name="newdeptid">合并后的部门ID</param>
        /// <param name="agentid">操作企业</param>
        /// <param name="mergetype">合并类型 1 上级部门 2同级部门</param>
        /// <param name="parentid">父部门ID</param>
        /// <returns></returns>
        public bool DeptMerge(int olddeptid, int newdeptid, string agentid, int parentid, int mergetype)
        {
            Recursive sive = new Recursive();
            string deptids = olddeptid.ToString();
            List<string> olddeptidlist = sive.GetDeptNodeId(olddeptid, agentid);
            if (olddeptidlist != null && olddeptidlist.Count > 0)
                foreach (string row in olddeptidlist)
                    deptids += "," + row;
            List<string> sqls = new List<string>();

            sqls.Add(" delete from fz_wfs.base_deptarea where deptid in  (" + olddeptid + ");");//删除合并的部门对应该所有的区域
            sqls.Add(" update fz_wfs.com_master set deptid=" + newdeptid + " where agentId='" + agentid + "' and deptid in (" + newdeptid + ");");//修改用户至被合并的部门
            if (mergetype == 2)
            {
                List<base_deptarea> mergeoldlist = manage.SelectSearch<base_deptarea>(j => j.deptid == olddeptid || j.deptid == newdeptid);//合并后的部门所包含的区域
                sqls.Add(" delete from fz_wfs.base_deptarea where deptid in  (" + newdeptid + ");");//删除被合并的部门对应该所有的区域


                List<base_deptarea> parentdeptlist = manage.SelectSearch<base_deptarea>(j => j.deptid == parentid);//父部门所包含的区域

                if (parentdeptlist != null && parentdeptlist.Count > 0 && mergeoldlist != null && mergeoldlist.Count > 0)
                {
                    foreach (base_deptarea row in parentdeptlist)//循环父级区域所有包含的部门
                    {
                        for (int i = mergeoldlist.Count - 1; i >= 0; i--)
                        {
                            if (row.districtid == mergeoldlist[i].districtid && row.schoolid == mergeoldlist[i].schoolid)//当区域ID和学校ID完成相等时添加(包含父级区域直接负责学校的判断)
                            {
                                sqls.Add(string.Format(@"insert into base_deptarea values({0},{1},'{2}',{3},'{4}',{5})", newdeptid, row.path, row.districtid, row.schoolid, row.schoolname, row.isend));
                                mergeoldlist.RemoveAt(i);
                                break;
                            }
                            else //不相等时
                            {
                                if (row.schoolid <= 0)//父级部门负责区域非学校
                                {
                                    #region 上级区域是区县
                                    if (row.path.Contains("区") || row.path.Contains("县"))
                                    {
                                        string str = JudgmentSchool(row, parentdeptlist[i], newdeptid);
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            sqls.Add(str);
                                            mergeoldlist.RemoveAt(i);
                                        }
                                    }
                                    #endregion
                                    #region 上级区域是市
                                    else if (row.path.Contains("市"))
                                    {
                                        if (JudgmentArea(row.districtid, parentdeptlist[i].districtid, 4))//当市相同时进一步获取学校判断
                                        {
                                            string citystr = GetArea(row.districtid);
                                            if (!string.IsNullOrEmpty(citystr))
                                            {
                                                var listAllCity = JsonConvert.DeserializeObject<List<ViewSchoolInfo>>(citystr);

                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
            return manage.CarriedOutSql(sqls);
        }

        /// <summary>
        /// 获取节点 
        /// </summary>
        /// <param name="schoolid"></param>
        /// <returns></returns>
        public int GetNonds(int schoolid)
        {
            if (schoolid > 0)
                return 2;
            return 1;
        }
        public bool JudgmentArea(int olddistrictid, int newdistrictid, int number)
        {
            if (olddistrictid.ToString().Substring(0, number) == newdistrictid.ToString().Substring(0, number))
                return true;
            return false;
        }

        /// <summary>
        /// 判断区域下学校是否包含
        /// </summary>
        /// <param name="oldrow"></param>
        /// <param name="newrow"></param>
        /// <param name="newdeptid"></param>
        /// <returns></returns>
        public string JudgmentSchool(base_deptarea oldrow, base_deptarea newrow, int newdeptid)
        {
            if (oldrow.districtid == newrow.districtid)//当区县完全相同时进一步获取学校判断
            {
                string schoolstr = GetSchool(oldrow.districtid.ToString());
                if (!string.IsNullOrEmpty(schoolstr))
                {
                    var listAllSchool = JsonConvert.DeserializeObject<List<ViewSchoolInfo>>(schoolstr);
                    var schoolTypeNos = schoolstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (schoolTypeNos != null && schoolTypeNos.Count > 0)
                    {
                        foreach (var schoolrow in listAllSchool)
                        {
                            if (schoolTypeNos.Contains(schoolrow.SchoolTypeNo))
                            {
                                if (Convert.ToInt32(schoolrow.DistrictID) == newrow.districtid && schoolrow.ID == newrow.schoolid)//当区域ID和学校ID完成相等时添加
                                {
                                    return string.Format(@"insert into base_deptarea values({0},{1},'{2}',{3},'{4}',{5})", newdeptid, schoolrow.Area, schoolrow.DistrictID, schoolrow.ID, schoolrow.SchoolName, GetNonds(schoolrow.ID));
                                }
                            }
                        }
                    }

                }
            }
            return "";
        }

    }
}