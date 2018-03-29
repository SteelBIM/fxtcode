using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity.DBEntity;
using System.Data;

namespace FxtCenterService.API.handlers
{
    /// <summary>
    /// projectlist楼盘列表
    /// </summary>
    public class projectlist : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
            {
            if (!CheckMustRequest(new string[] { "cityid", "fxtcompanyid", "type" })) return;
            string result = "";
            
            string key = HttpUtility.UrlDecode(GetRequest("key"));          
            int companyid = StringHelper.TryGetInt(GetRequest("companyid"));
            int cityid = StringHelper.TryGetInt(GetRequest("cityid"));
            int areaId = StringHelper.TryGetInt(GetRequest("areaid"));
            int purposecode = StringHelper.TryGetInt(GetRequest("purposecode"));
            int buildingTypeCode = StringHelper.TryGetInt(GetRequest("buildingtypecode"));
            int fxtCompanyId = StringHelper.TryGetInt(GetRequest("fxtcompanyid"));
            int projectid = StringHelper.TryGetInt(GetRequest("projectid"));
            string type = GetRequest("type");
            DataSet ds = null;
            DATProject modelProject = new DATProject();

            switch (type)
            {
                case "list":
                    List<DATProject> plist = DatProjectBL.GetDATProjectList(search, key, areaId, buildingTypeCode, purposecode);
                    result = GetJson(plist, "");
                    break;
                case "dropdown":
                    int items = 15;
                    List<Dictionary<string, object>> list = null;
                    list = DatProjectBL.GetProjectDropDownList(search, key, items);
                    result = GetJson(list, "");
                    break;
                case "details":
                    DATProject dat = DatProjectBL.GetProjectInfoById(cityid,projectid,fxtCompanyId);
                    result = GetJson(dat,"");
                    break;
                case "photo":
                    List<LNKPPhoto> photo = DatProjectBL.GetProjectPhotoById(cityid, projectid, fxtCompanyId);
                    result = GetJson(photo,"");
                    break;
                case "month":
                    search.DateBegin = StringHelper.TryGetDateTimeFormat(context.Request["begindate"], DateTime.Now.AddMonths(-12));
                    search.DateEnd = StringHelper.TryGetDateTimeFormat(context.Request["enddate"], DateTime.Now);
                    ds = DATAvgPriceMonthBL.GetDATAvgPriceMonthList(search, projectid);
                    result = GetJson(returnFlashJson(ds,0,0),1,ConstCommon.Operation_success);
                    break;
                case "case": 
                    search.DateBegin = StringHelper.TryGetDateTimeFormat(context.Request["begindate"], DateTime.Now.AddMonths(-48));
                    search.DateEnd = StringHelper.TryGetDateTimeFormat(context.Request["enddate"], DateTime.Now);
                    result = DataTableToJSON(DatProjectBL.GetProjectCase(search, projectid, fxtCompanyId,cityid));
                    break;
                case "autoprojectlist":
                    result = GetJson(DatProjectBL.GetSearchProjectListByKey(search, fxtCompanyId, cityid, key), 1, ConstCommon.Operation_success);
                    break;
                case "autoprojectdetails":
                    result =GetJson(DatProjectBL.GetProjectDetailsByProjectid(search, fxtCompanyId, cityid, projectid),1,ConstCommon.Operation_success);
                    break;

            }
            context.Response.Write(result);
            context.Response.End();
        }

        //loading 1:indexTrend or 0:priceTrend 
        private string returnFlashJson(DataSet ds, int dateType, int flashType)
        {
            int max = 0;
            int min = 0;
            string title = "";
            string labels = "";
            string values = "";
            string yy = ""; //11年
            string yyyy = "";//2012年
            string format = "";

            switch (dateType)
            {
                case 1://week
                    format = "周";
                    break;
                case 2://Quarter
                    format = "季";
                    break;
                case 3://year
                    format = "年"; break;
                case 0:
                    format = "月";
                    break;
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                //图形数据
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int price = Convert.ToInt32(dt.Rows[i]["avgprice"]);
                        string week = "";
                        DateTime time = Convert.ToDateTime(dt.Rows[i]["avgpricedate"]);
                        yy = time.ToString("yyyy-MM-dd").Substring(2, 2);
                        yyyy = time.ToString("yyyy-MM-dd").Substring(0, 4);
                        if (dateType != 3)
                        {
                            week = time.ToString("yyyy-MM-dd").Substring(5, 2);
                            labels += string.Format("\"{0}-{1}\",", yy, week);
                        }
                        else
                        {
                            labels += string.Format("\"{0}\",", yy, week);
                        }




                        //if (i == 0)
                        //    title += date.ToString("yyyy-MM") + " 至 ";
                        //else if (i == dt.Rows.Count - 1)
                        //    title += date.ToString("yyyy-MM");

                        if (flashType == 0)
                        {
                            if (price > max) max = price;
                            if (min == 0) min = price;
                            else if (price < min) min = price;
                            if (dateType != 3)
                            {
                                values += string.Format("{{\"value\": {0},\"tip\": \"{1}年{2}{3}<br>#val#元/平方米\"}},", price, yyyy, week, format);
                            }
                            else
                            {
                                values += string.Format("{{\"value\": {0},\"tip\": \"{1}年<br>#val#元/平方米\"}},", price, yyyy);
                            }

                        }
                        else
                        {
                            int index = Convert.ToInt32(dt.Rows[i]["Exponential"]);
                            if (index > max) max = index;
                            if (min == 0) min = index;
                            else if (index < min) min = index;
                            values += string.Format("{{\"value\": {0},\"tip\": \"{1}年{2}{3}<br>指数：#val#\"}},", index, yyyy, week, format);
                        }

                        //CasePrice.Append('"' + dt.Rows[i]["CaseDate"].ToString() + ";" + dt.Rows[i]["UnitPrice"].ToString() + '"' + ",");
                        //values:鼠标滑过提示；title时间区间轴：201211至201212，labels:x轴；
                    }
                    labels = labels.Remove(labels.Length - 1);
                    values = values.Remove(values.Length - 1);
                }
            }//GetTrendZS
            if (flashType == 0)
            { return TrendHelper.GetTrend(title, labels, min, max, values); }
            else { return TrendHelper.GetTrendZS(title, labels, min, max, values); }

        }
    }
}