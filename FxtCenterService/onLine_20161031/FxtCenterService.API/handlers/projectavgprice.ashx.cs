using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity.DBEntity;
using Newtonsoft.Json.Linq;

namespace FxtCenterService.API.handlers
{
    /// <summary>
    /// projectavgprice 的摘要说明
    /// 均价信息
    /// </summary>
    public class projectavgprice : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequest(new string[] { "cityid", "projectid", "type", "fxtcompanyid" })) return;
            string result = "";
            string type = GetRequest("type");
            //公司ID
            int fxtcompanyid = StringHelper.TryGetInt(GetRequest("fxtcompanyid")) == 365 ? 25 : StringHelper.TryGetInt(GetRequest("fxtcompanyid"));
            int cityid = StringHelper.TryGetInt(GetRequest("cityid"));
            int projectid = StringHelper.TryGetInt(GetRequest("projectid"));
            string purpose = GetRequest("purpose");//物业类型
            DateTime startdate = StringHelper.TryGetDateTime(GetRequest("startdate", DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss")));
            DateTime enddate = StringHelper.TryGetDateTime(GetRequest("enddate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            bool onlyhasprice = StringHelper.TryGetBool(GetRequest("onlyhasprice"));
            int areaid = StringHelper.TryGetInt(GetRequest("areaid"));//行政区ID

            //楼盘名称
            string projectname = GetRequest("projectname");

            //查勘坐标
            double surveyx = StringHelper.TryGetDouble(GetRequest("surveyx"));
            double surveyy = StringHelper.TryGetDouble(GetRequest("surveyy"));

            double buildingArea = StringHelper.TryGetDouble(GetRequest("buildingarea"));//物业面积
            int totalfloor = StringHelper.TryGetInt(GetRequest("totalfloor"));//总楼层数
            DateTime buildingenddate = StringHelper.TryGetDateTime(GetRequest("buildingenddate"));//竣工时间

            int daterange = (string.IsNullOrEmpty(GetRequest("daterange"))) ? 3 : StringHelper.TryGetInt(GetRequest("daterange"));//价格计算范围（默认为3）

            try
            {
                int buildingareatype = CodeHelper.GetBuildingAreaType(buildingArea);//面积类型
                int buildingtypecode = CodeHelper.GetBuildingTypeCode(totalfloor);//楼栋类型
                SYSCode purposecode = SYSCodeBL.GetCode(1002, purpose);
                int purposetypecode = (purposecode != null && purposecode.code != null) ? purposecode.code.Value : 0;//物业类型 

                switch (type)
                {
                    case "avgprice"://获取楼盘建筑类型及面积段分类均价信息（直接调取数据库数据）
                        int[] purposetype;//物业类型
                        if (purpose == "普通住宅")
                        {
                            SYSCode code = (string.IsNullOrEmpty(purpose)) ? null : SYSCodeBL.GetCode(1002, purpose);
                            purposetype = (code != null && code.code != null) ? new int[] { code.code.Value } : null;
                        }
                        else
                        {
                            purposetype = new int[] { 1002027, 1002005, 1002006, 1002007, 1002008 };
                        }
                        List<DATProjectAvgPrice> list = DATProjectAvgPriceBL.GetProjectAvgPriceList(fxtcompanyid, cityid, projectid, purposetype, startdate, enddate, onlyhasprice, daterange);
                        result = GetJson(list);
                        break;
                    /*case "avgpricetrend"://均价走势图(avgtype=1 楼盘,avgtype=2 行政区,avgtype=3 城市)                      
                        //此方法区分细分类型，因案例还未计算出城市、行政区细分类型均价、此方法暂停调用 caoq 2014-3-28
                        DataSet ds = DATProjectAvgPriceBL.GetAvgPriceTrend(fxtcompanyid, cityid, areaid, projectid, purposetypecode, buildingareatype, buildingtypecode, startdate, enddate, daterange);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            result = GetJson(ds.Tables[0]);
                        }
                        else
                        {
                            result = GetJson();
                        }
                        break;
                    */
                    case "avgpricetrend"://均价走势图(avgtype=1 楼盘,avgtype=2 行政区,avgtype=3 城市) 
                        if (purpose != "普通住宅") purposetypecode = 1002027;
                        //调用运维中心城市、行政区均价（6个月）                                               
                        TimeSpan dtShow = enddate.Subtract(startdate); //两个时间差                       
                        //调用月份
                        int monthcnt = Math.Abs(((enddate.Year - startdate.Year) * 12 + (enddate.Month - startdate.Month)));
                        DataSet ds = monthcnt > 0 ? DATAvgPriceMonthBL.GetCityAreaAvgPriceTrend(monthcnt, cityid, areaid, startdate, enddate) : null;
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            //调用WCF接口获取楼盘细分类型均价                       
                            for (int i = 0; i < monthcnt; i++)
                            {
                                wcf_projectavgprice.FxtAPIClient proprice = new wcf_projectavgprice.FxtAPIClient();
                                string curdate = DateTime.Now.ToString("yyyy-MM-dd"), avgstr = "";
                                DateTime avgpricedate = startdate.AddMonths(i);
                                var para = new { projectId = projectid, cityId = cityid, codeType = purposetypecode, date = avgpricedate.ToString("yyyy-MM") };
                                object objprice = proprice.Entrance(curdate, GetCode(curdate), "D", "CrossProjectByCodeType", JSONHelper.ObjectToJSON(para));
                                proprice.Abort();
                                if (objprice != null)
                                {
                                    avgstr = objprice.ToString();
                                    LogHelper.Info(avgstr);
                                }
                                if (!string.IsNullOrEmpty(avgstr))
                                {
                                    JObject objParam = JObject.Parse(avgstr);//参数 
                                    if (objprice != null && objParam.Value<int>("data") > 0)
                                    {
                                        DataRow row = ds.Tables[0].NewRow();
                                        row["AvgPriceDate"] = avgpricedate.ToString("yyyy-MM-01");
                                        row["AvgPrice"] = objParam.Value<int>("data");
                                        row["avgtype"] = 1;//楼盘均价
                                        ds.Tables[0].Rows.Add(row);
                                    }
                                }
                            }
                            result = GetJson(ds.Tables[0]);
                        }
                        else
                        {
                            result = GetJson();
                        }
                        break;
                    case "sameproject"://周边同质楼盘均价                        
                        DataSet sameprojectDs = DATCaseBL.GetSameProjectCasePrice(fxtcompanyid, cityid, projectid, buildingareatype, buildingtypecode, buildingenddate, areaid, surveyx, surveyy, startdate, enddate);
                        if (sameprojectDs != null && sameprojectDs.Tables.Count > 0)
                        {
                            result = GetJson(sameprojectDs.Tables[0]);
                        }
                        else
                        {
                            result = GetJson();
                        }
                        break;
                    case "otherchannel"://不同渠道楼盘均价对比
                        DataSet otherchannelDs = DATCaseBL.GetOtherChannelCasePrice(fxtcompanyid, cityid, projectid, purposetypecode, buildingareatype, buildingtypecode, startdate, enddate, daterange);
                        if (otherchannelDs != null && otherchannelDs.Tables.Count > 0)
                        {
                            result = GetJson(otherchannelDs.Tables[0]);
                        }
                        else
                        {
                            result = GetJson();
                        }
                        break;
                    case "mapprice"://地图价格、环比跌涨幅
                        if (purposetypecode != 1002001) purposetypecode = 1002027;//别墅类型

                        DataSet mapds = DATProjectAvgPriceBL.GetMapPrice(cityid, fxtcompanyid, projectid, projectname, surveyx, surveyy, purposetypecode, enddate, daterange);
                        if (mapds != null && mapds.Tables.Count > 0)
                        {
                            result = GetJson(mapds.Tables[0]);
                        }
                        else
                        {
                            result = GetJson();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result = GetJson(-1, "异常");
            }
            context.Response.Write(result);
        }
    }
}