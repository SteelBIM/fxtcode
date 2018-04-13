using KSWF.Core.Utility;
using KSWF.Framework.BLL;
using KSWF.Web.Admin.Models;
using KSWF.WFM.Constract.Models;
using KSWF.WFM.Constract.VW;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace KSWF.Web.Admin.Controllers
{
    /// <summary>
    /// 区域过滤
    /// </summary>
    public class AreaFilter : Controller
    {
        Manage manage = new Manage();

        #region 判断区域或者学校是否已经选择
        /// <summary>
        /// 判断地区或者学校是否已选
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="mastername"></param>
        /// <returns></returns>
        public KingResponse JudgmentArea(string Area, string mastername, int deptid, int type, string agentid, string productid)
        {
            KingResponse res = new KingResponse();
            if (!string.IsNullOrEmpty(Area))
            {
                string pid = "";
                string[] arraylist = Area.TrimEnd('@').Split('@');
                string areaname = "";
                for (int i = 0; i < arraylist.Length; i++)
                {
                    string[] array = arraylist[i].Split('|');
                    if (!string.IsNullOrEmpty(productid.TrimStart(',').TrimEnd(',')))
                        pid = productid;
                    else
                        pid = array[4];
                    if (Convert.ToInt32(array[2]) > 0)//包含学校
                    {
                        string schoolname = "";
                        if (JudgmentSelectedSchool(arraylist[i], mastername, deptid, type, agentid, pid, out schoolname))
                        {
                            res.Success = true;
                            res.ErrorMsg += schoolname + "、";
                        }
                        else
                        {
                            if (JudgmentSelectedAreaIdSchool(arraylist[i], mastername, deptid, type, agentid, pid, out areaname))
                            {
                                res.Success = true;
                                res.ErrorMsg += areaname + "、";
                            }
                        }
                    }
                    else if (JudgmentSelectedAreaId(arraylist[i], mastername, deptid, type, agentid, pid, out areaname))
                    {
                        res.Success = true;
                        res.ErrorMsg += areaname + "、";
                    }
                }
            }
            if (!string.IsNullOrEmpty(res.ErrorMsg))
            {
                res.ErrorMsg = res.ErrorMsg.TrimEnd('、') + " 已使用！请勿重复选择。";
            }
            return res;
        }


        /// <summary>
        /// 判断学校是否已选
        /// </summary>
        /// <param name="area"></param>
        /// <param name="mastername"></param>
        /// <param name="schoolname"></param>
        /// <returns></returns>
        public bool JudgmentSelectedSchool(string area, string mastername, int deptid, int type, string agentid, string productid, out string schoolname)
        {
            string[] schoolarray = area.Split('|');
            int schoolid = Convert.ToInt32(schoolarray[2]);
            schoolname = schoolarray[1] + schoolarray[3];
            if (schoolid == 0 || string.IsNullOrEmpty(schoolname))
                return false;

            #region 判断子部门学校是否已选
            if (manage.GetTotalCount<vw_deptarea>(t => (t.parentid == deptid && t.schoolid == schoolid && t.agentid==agentid)) > 0)
                return true;
            #endregion

            List<Expression<Func<vw_masterarea, bool>>> expr = Expr(agentid, type, productid, mastername);
            expr.Add(jma => jma.schoolid == schoolid);

            return manage.GetTotalCount<vw_masterarea>(expr) > 0;

        }

        /// <summary>
        /// 判断地区学校是否已选
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool JudgmentSelectedAreaIdSchool(string area, string mastername, int deptid, int type, string agentid, string productid, out string areaname)
        {
            string[] areaarray = area.Split('|');
            string areaId = areaarray[0].ToString();//区域ID
            areaname = areaarray[1].ToString() + areaarray[3];


            List<string> listareaid = ConvertAreaIds(areaId);
            int substrnumber = subnumber(areaId);//截取长度
            string substr = areaId.Substring(0, substrnumber);

            #region 判断子部门地区是否已选
            if (manage.GetTotalCount<vw_deptarea>(da => da.parentid == deptid && da.schoolid == 0 && da.agentid == agentid, listareaid, "districtid") == 0)
            {
                if (manage.GetTotalCount<vw_deptarea>(da => da.parentid == deptid && da.schoolid == 0 && da.agentid == agentid, "  left(districtid," + substrnumber + ") ='" + substr + "' ") > 0)
                {
                    areaname = "子部门负责区域相同。";
                    return true;
                }
            }
            else
            {
                areaname = "子部门负责区域相同";
                return true;
            }
            #endregion

            List<Expression<Func<vw_masterarea, bool>>> expr = Expr(agentid, type, productid, mastername);
            expr.Add(m => m.schoolid == 0);
            if (manage.GetTotalCount<vw_masterarea>(expr, listareaid, "districtid") == 0)
                return manage.GetTotalCount<vw_masterarea>(expr, " left(districtid," + substrnumber + ") ='" + substr + "' ") > 0;
            return true;
        }



        /// <summary>
        /// 判断地区是否已选
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool JudgmentSelectedAreaId(string area, string mastername, int deptid, int type, string agentid, string productid, out string areaname)
        {
            string[] areaarray = area.Split('|');
            string areaId = areaarray[0].ToString();//区域ID
            areaname = areaarray[1].ToString() + areaarray[3];
            List<string> listareaid = ConvertAreaIds(areaId);
            int substrnumber = subnumber(areaId);//截取长度
            string substr = areaId.Substring(0, substrnumber);

            #region 判断子部门地区是否已选

            List<Expression<Func<vw_deptarea, bool>>> exprdept = new List<Expression<Func<vw_deptarea, bool>>>();
            string OrgId = Core.Utility.PublicHelp.OrgId;
            exprdept.Add(da => da.parentid == deptid);
            if (type == 2)
                exprdept.Add(da => da.agentid == agentid);
            else
                exprdept.Add(da => da.agentid == OrgId);

            if (manage.GetTotalCount<vw_deptarea>(exprdept, listareaid, "districtid") == 0)
            {
                if (manage.GetTotalCount<vw_deptarea>(exprdept, " left(districtid," + substrnumber + ") ='" + substr + "' ") > 0)
                {
                    areaname = "子部门负责区域相同。";
                    return true;
                }
            }
            else
            {
                areaname = "子部门负责区域相同";
                return true;
            }
            #endregion

            List<Expression<Func<vw_masterarea, bool>>> expr = Expr(agentid, type, productid, mastername);
            if (manage.GetTotalCount<vw_masterarea>(expr, listareaid, "districtid") == 0)
                return manage.GetTotalCount<vw_masterarea>(expr, " left(districtid," + substrnumber + ") ='" + substr + "' ") > 0;

            return true;
        }


        #region 找到区域ID List<string>
        /// <summary>
        /// 区域ID并找到根节点 
        /// </summary>
        /// <param name="AreaIds"></param>
        /// <returns></returns>
        public List<string> ConvertAreaIds(string AreaIds)
        {
            List<string> listareaid = new List<string>();
            listareaid.Add(AreaIds);
            listareaid.Add(ReturnNumber(AreaIds, 3));
            listareaid.Add(ReturnNumber(AreaIds, 5));
            listareaid.Add(ReturnNumber(AreaIds, 7));
            return listareaid;
        }

        public string ReturnNumber(string str, int num)
        {
            string ext = "";
            for (int i = 0; i < num; i++)
                ext += "0";
            return str.Substring(0, str.Length - num) + ext;
        }

        public int subnumber(string str)
        {
            if (str.Substring(2, 7) == "0000000")
                return 2;
            else if (str.Substring(4, 5) == "00000")
                return 4;
            else if (str.Substring(6, 3) == "000")
                return 6;
            return 9;
        }
        #endregion



        private List<Expression<Func<vw_masterarea, bool>>> Expr(string agentid, int type, string productid, string mastername)
        {
            List<Expression<Func<vw_masterarea, bool>>> expr = new List<Expression<Func<vw_masterarea, bool>>>();


            bool isagent = false;
            if (agentid == Core.Utility.PublicHelp.OrgId)
                isagent = true;

            if (!string.IsNullOrEmpty(productid))
            {
                string[] parductarray = productid.TrimEnd(',').Split(',');
                if (parductarray.Length > 1)
                {
                    if (parductarray.Length == 2)       
                        expr.Add(jma => jma.pids.Contains("," + parductarray[0].ToString() + ",") || jma.pids.Contains("," + parductarray[1].ToString() + ","));
                    else if (parductarray.Length == 3)
                        expr.Add(jma => jma.pids.Contains("," + parductarray[0].ToString() + ",") || jma.pids.Contains("," + parductarray[1].ToString() + ",") || jma.pids.Contains("," + parductarray[2].ToString() + ","));
                    else if (parductarray.Length == 4)
                        expr.Add(jma => jma.pids.Contains("," + parductarray[0].ToString() + ",") || jma.pids.Contains("," + parductarray[1].ToString() + ",") || jma.pids.Contains("," + parductarray[2].ToString() + ",") || jma.pids.Contains("," + parductarray[3].ToString() + ","));
                    else if (parductarray.Length == 5)
                        expr.Add(jma => jma.pids.Contains("," + parductarray[0].ToString() + ",") || jma.pids.Contains("," + parductarray[1].ToString() + ",") || jma.pids.Contains("," + parductarray[2].ToString() + ",") || jma.pids.Contains("," + parductarray[3].ToString() + ",") || jma.pids.Contains("," + parductarray[4].ToString() + ","));
                    else if (parductarray.Length == 6)
                        expr.Add(jma => jma.pids.Contains("," + parductarray[0].ToString() + ",") || jma.pids.Contains("," + parductarray[1].ToString() + ",") || jma.pids.Contains("," + parductarray[2].ToString() + ",") || jma.pids.Contains("," + parductarray[3].ToString() + ",") || jma.pids.Contains("," + parductarray[4].ToString() + ",") || jma.pids.Contains("," + parductarray[5].ToString() + ","));
                    else if (parductarray.Length == 7)
                        expr.Add(jma => jma.pids.Contains("," + parductarray[0].ToString() + ",") || jma.pids.Contains("," + parductarray[1].ToString() + ",") || jma.pids.Contains("," + parductarray[2].ToString() + ",") || jma.pids.Contains("," + parductarray[3].ToString() + ",") || jma.pids.Contains("," + parductarray[4].ToString() + ",") || jma.pids.Contains("," + parductarray[5].ToString() + ",") || jma.pids.Contains("," + parductarray[6].ToString() + ","));
                    else if (parductarray.Length == 8)
                        expr.Add(jma => jma.pids.Contains("," + parductarray[0].ToString() + ",") || jma.pids.Contains("," + parductarray[1].ToString() + ",") || jma.pids.Contains("," + parductarray[2].ToString() + ",") || jma.pids.Contains("," + parductarray[3].ToString() + ",") || jma.pids.Contains("," + parductarray[4].ToString() + ",") || jma.pids.Contains("," + parductarray[5].ToString() + ",") || jma.pids.Contains("," + parductarray[6].ToString() + ",") || jma.pids.Contains("," + parductarray[7].ToString() + ","));

                }
                else
                {
                    expr.Add(jma => jma.pids.Contains(","+productid.ToString()+","));
                }
            }
            if (type == 2)
                expr.Add(jma => jma.agentid == agentid);
           

            if (!string.IsNullOrEmpty(mastername))
                expr.Add(jma => jma.mastername != mastername);

            if (!isagent)
                expr.Add(jma => jma.mastertype == 0);

            return expr;
        }

        #endregion




        /// <summary>
        /// 判断学校是否已选
        /// </summary>
        /// <param name="area"></param>
        /// <param name="mastername"></param>
        /// <param name="schoolname"></param>
        /// <returns></returns>
        public bool JudgmentSelectedSchoolFilter(int schoolid, List<vw_masterarea> arealist, int deptid, int type, string agentid, string productid, List<vw_deptarea> deptarelist)
        {
           
            Predicate<vw_deptarea> deptarea  = delegate(vw_deptarea da) { return da.schoolid == schoolid; };
            List<vw_deptarea> deptarealist = deptarelist.FindAll(deptarea);
            if (deptarealist != null && deptarealist.Count > 0)
                return true;
 
            Predicate<vw_masterarea> masterarea = delegate(vw_masterarea ma) { return ma.schoolid == schoolid; };
            List<vw_masterarea> areaschool = arealist.FindAll(masterarea);
            if (areaschool != null && areaschool.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 判断地区学校是否已选
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool JudgmentSelectedAreaIdSchoolFilter(string areaId, List<vw_masterarea> arealist, int deptid, int type, string agentid, string productid, List<vw_deptarea> deptarelist)
        {
            List<string> listareaid = ConvertAreaIds(areaId);
            int substrnumber = subnumber(areaId);//截取长度
            string substr = areaId.Substring(0, substrnumber);


            string districtids = "";
            if (listareaid != null && listareaid.Count > 0)
            {
                foreach (string row in listareaid)
                    districtids += row + ",";
            }

            Predicate<vw_deptarea> deptarea = delegate(vw_deptarea da) { return da.schoolid == 0 && da.agentid==agentid  &&  (districtids.Contains(da.districtid.ToString()) || da.districtid.ToString().Substring(0, substrnumber) == substr); };
            List<vw_deptarea> deptarealist = deptarelist.FindAll(deptarea);
            if (deptarealist != null && deptarealist.Count > 0)
                return true;

            Predicate<vw_masterarea> schoolarea = delegate(vw_masterarea ma) { return ma.schoolid == 0 && (districtids.Contains(ma.districtid) || ma.districtid.Substring(0, substrnumber) == substr); };
            List<vw_masterarea> selectlist = arealist.FindAll(schoolarea);
            if (selectlist != null && selectlist.Count > 0)
                return true;
            return false;
        }

        public List<vw_masterarea> GetUserArea(string mastername, int type, string agentid, string productid, string province)
        {
            List<Expression<Func<vw_masterarea, bool>>> expr = Expr(agentid, type, productid, mastername);
            return manage.SelectSearchs<vw_masterarea>(expr, "  left(districtid,2) ='" + province + "' ");
        }

        public List<vw_deptarea> GetDeptArea(int deptid, string agentid, string province)
        {
            List<Expression<Func<vw_deptarea, bool>>> expr = new List<Expression<Func<vw_deptarea, bool>>>();
            expr.Add(t => t.parentid == deptid);
            return manage.SelectSearchs<vw_deptarea>(expr, "  left(districtid,2) ='" + province + "' ");

        }
    }
}

