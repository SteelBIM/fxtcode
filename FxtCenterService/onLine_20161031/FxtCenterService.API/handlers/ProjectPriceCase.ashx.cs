using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using FxtCenterService.API;
using CAS.Entity.DBEntity;
using FxtCenterService.Logic;
using CAS.Entity.GJBEntity;
using System.Data;

namespace FxtCenterService.API.handlers
{
    /// <summary>
    /// buildingcase 的摘要说明
    /// </summary>
    public class ProjectPriceCase : HttpHandlerBase
    {

        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (!CheckMustRequest(new string[] {  "cityid", "fxtcompanyid" })) return;
            string type = GetRequest("type");
            if (type != "casecount") if (!CheckMustRequest(new string[] { "projectname" })) return;
            string result = "";
            string projectname = GetRequest("projectname");

            string fxtcompanyId = GetRequest("fxtcompanyid");
            int companyId;
            int.TryParse(fxtcompanyId,out companyId);



            int[] caseIds = string.IsNullOrEmpty(GetRequest("caseIds")) ? null : GetRequest("caseIds").Split(',').Select(StringHelper.TryGetInt).ToArray();
            try
            {
                if (type == "gridlist")
                {
                    /*
                     * 查询案例的列表页面,目前仅来自于测算时选取的案例列表 byte 12-03
                     */
                    List<Dat_Case> list =null;
                    if(string.IsNullOrEmpty(projectname.Trim()))
                    {
                        list = new List<Dat_Case>();
                    }
                    else{

                        //modified by qiuyan 2014-03-12
                        if (companyId == 178)
                        {
                            var listResult = DATCaseBL.GetDATCaseListByCalculateForSpecial(search, projectname);
                            result = GetJson(listResult, 1, "", null);
                        }
                        else
                        {
                            list = DATCaseBL.GetDATCaseListByCalculate(search, projectname);
                            result = GetJson(list, 1, "", null);
                        }

                      
                    }
                  
                }
                else if (type == "casecount") //获取楼盘案例总数 caoq 2014-3-28
                {
                    int cityid = StringHelper.TryGetInt(GetRequest("cityid"));
                    int projectid = StringHelper.TryGetInt(GetRequest("projectid"));
                    string purpose = GetRequest("purpose");//物业类型                    
                    double buildingArea = StringHelper.TryGetDouble(GetRequest("buildingarea"));//物业面积                    
                    int totalfloor = StringHelper.TryGetInt(GetRequest("totalfloor"));//总楼层数
                    DateTime startdate = StringHelper.TryGetDateTime(GetRequest("startdate", DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss")));
                    DateTime enddate = StringHelper.TryGetDateTime(GetRequest("enddate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    int buildingareatype = CodeHelper.GetBuildingAreaType(buildingArea);//面积类型
                    int buildingtypecode = CodeHelper.GetBuildingTypeCode(totalfloor);//楼栋类型
                    int purposetypecode = 0;
                    if (purpose != "普通住宅") purposetypecode = 1002027;
                    else
                    {
                        SYSCode code = SYSCodeBL.GetCode(1002, purpose);
                        purposetypecode = (code != null && code.code != null) ? code.code.Value : 0;//物业类型 
                    }

                    int cnt=DATCaseBL.GetProjectCaseCount(StringHelper.TryGetInt(fxtcompanyId), cityid, projectid, purposetypecode, buildingareatype, buildingtypecode, startdate, enddate);
                    result = GetJson(cnt);
                }
                else
                {
                    if (null != caseIds)
                    {   //案例Ids last modified byte 07-24
                        /*
                        DataSet ds = DATCaseBL.GetDATCaseList(search, caseIds);
                        result = JSONHelper.DataTableToJSON(ds.Tables[0]);
                        */

                        if (companyId == 178)
                        {
                            List<Dat_Case_Dhhy> list = DATCaseBL.GetDATCaseListForSpecial(search, "", string.Empty, caseIds);
                            result = JSONHelper.ObjectToJSON(list);
                        }
                        else
                        {
                            List<Dat_Case> list = DATCaseBL.GetDATCaseList(search, "", string.Empty, caseIds);
                            result = JSONHelper.ObjectToJSON(list);

                        }
                    }
                    else
                    {
                        //modified by qiuyan 2014-03-12
                        if (companyId == 178)
                        {
                            GetDATCaseListForSpecial(projectname, out result);
                        }
                        else
                        {
                            GetDATCaseList(projectname, out result);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                result = GetJson(ex);
            }
            context.Response.Write(result);
            context.Response.End();
        }


        private void GetDATCaseList (string projectname,out string result)
        {

            List<Dat_Case> list = DATCaseBL.GetDATCaseList(search, "", projectname, null);
            decimal? oneStageUnitprice = list.Where(s => s.buildingarea < 60).ToList().Average(s => s.unitprice);
            decimal? twoStageUnitprice = list.Where(s => s.buildingarea >= 60 && s.buildingarea < 90).ToList().Average(s => s.unitprice);
            decimal? threeStageUnitprice = list.Where(s => s.buildingarea >= 90 && s.buildingarea < 120).ToList().Average(s => s.unitprice);
            decimal? forStageUnitprice = list.Where(s => s.buildingarea >= 120 && s.buildingarea < 144).ToList().Average(s => s.unitprice);
            decimal? fiveStageUnitprice = list.Where(s => s.buildingarea >= 144).ToList().Average(s => s.unitprice);
            if (list.Count > 0)
            {
                list[0].oneStageUnitprice = oneStageUnitprice == null ? 0 : (decimal)oneStageUnitprice;
                list[0].twoStageUnitprice = twoStageUnitprice == null ? 0 : (decimal)twoStageUnitprice;
                list[0].threeStageUnitprice = threeStageUnitprice == null ? 0 : (decimal)threeStageUnitprice;
                list[0].forStageUnitprice = forStageUnitprice == null ? 0 : (decimal)forStageUnitprice;
                list[0].fiveStageUnitprice = fiveStageUnitprice == null ? 0 : (decimal)fiveStageUnitprice;
            }
            result = GetJson(list, 1, "", null);
        }

        private void GetDATCaseListForSpecial(string projectname, out string result)
        {

            List<Dat_Case_Dhhy> list = DATCaseBL.GetDATCaseListForSpecial(search, "", projectname, null);
            decimal? oneStageUnitprice = list.Where(s => s.buildingarea < 60).ToList().Average(s => s.unitprice);
            decimal? twoStageUnitprice = list.Where(s => s.buildingarea >= 60 && s.buildingarea < 90).ToList().Average(s => s.unitprice);
            decimal? threeStageUnitprice = list.Where(s => s.buildingarea >= 90 && s.buildingarea < 120).ToList().Average(s => s.unitprice);
            decimal? forStageUnitprice = list.Where(s => s.buildingarea >= 120 && s.buildingarea < 144).ToList().Average(s => s.unitprice);
            decimal? fiveStageUnitprice = list.Where(s => s.buildingarea >= 144).ToList().Average(s => s.unitprice);
            if (list.Count > 0)
            {
                list[0].oneStageUnitprice = oneStageUnitprice == null ? 0 : (decimal)oneStageUnitprice;
                list[0].twoStageUnitprice = twoStageUnitprice == null ? 0 : (decimal)twoStageUnitprice;
                list[0].threeStageUnitprice = threeStageUnitprice == null ? 0 : (decimal)threeStageUnitprice;
                list[0].forStageUnitprice = forStageUnitprice == null ? 0 : (decimal)forStageUnitprice;
                list[0].fiveStageUnitprice = fiveStageUnitprice == null ? 0 : (decimal)fiveStageUnitprice;
            }
            result = GetJson(list, 1, "", null);
        }

    }
}