using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity.DBEntity;
using CAS.Entity.GJBEntity;
using CAS.Entity;

namespace FxtCenterService.API.handlers
{
    /// <summary>
    /// autoprice 的摘要说明
    /// </summary>
    public class autoprice : HttpHandlerBase
    {

        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequest(new string[] { "cityid", "projectid", "type", "fxtcompanyid" })) return;
            string result = "";
            string key = HttpUtility.UrlDecode(GetRequest("key"));
            int fxtcompanyid = StringHelper.TryGetInt(GetRequest("fxtcompanyid")) == 365 ? 25 : StringHelper.TryGetInt(GetRequest("fxtcompanyid"));
            int cityid = StringHelper.TryGetInt(GetRequest("cityid"));
            int projectId = StringHelper.TryGetInt(GetRequest("projectid"));
            int buildingId = StringHelper.TryGetInt(GetRequest("buildingid"));
            int houseId = StringHelper.TryGetInt(GetRequest("houseid"));
            int companyid = StringHelper.TryGetInt(GetRequest("companyid")) == 0 ? fxtcompanyid : StringHelper.TryGetInt(GetRequest("companyid"));
            string userId = GetRequest("username");
            string type = GetRequest("type");
            double buildingArea = StringHelper.TryGetDouble(GetRequest("buildingarea"));//物业面积
            string purpose = GetRequest("purpose");//物业类型
            int totalfloor = StringHelper.TryGetInt(GetRequest("totalfloor"));//总楼层数
            int floornumber = StringHelper.TryGetInt(GetRequest("floornumber"));//评估楼层
            DateTime buildingenddate = StringHelper.TryGetDateTime(GetRequest("buildingenddate"));//竣工时间
            int areaid = StringHelper.TryGetInt(GetRequest("areaid"));//行政区ID
            //房号修正系数
            string direction_str = GetRequest("direction");//朝向
            string landscape_str = GetRequest("landscape");//景观
            string lv_str = GetRequest("lv");//装修档次
            string decorationprobabilit_str = GetRequest("decorationprobabilit");//装修成新率
            //查勘坐标
            double surveyx = StringHelper.TryGetDouble(GetRequest("surveyx"));
            double surveyy = StringHelper.TryGetDouble(GetRequest("surveyy"));

            int qid = StringHelper.TryGetInt(GetRequest("qid"));
            int systypecode = StringHelper.TryGetInt(GetRequest("systypecode"));
            int daterange = (string.IsNullOrEmpty(GetRequest("daterange"))) ? 3 : StringHelper.TryGetInt(GetRequest("daterange"));//价格计算范围（默认为3）

            string StartDate = string.Empty;
            string EndDate = string.Empty;
            try
            {
                AutoPrice autoPrice = null;//自动估价结果
                switch (type)
                {
                    case "autoprice"://自动估价
                        autoPrice = DatHouseBL.GetEValueByProjectId(search.CityId, projectId, buildingId, houseId, fxtcompanyid,
          companyid, userId, buildingArea, StartDate, EndDate);
                        result = GetJson(autoPrice);
                        break;
                    case "cmbautoprice"://银行自动估价
                        decimal ygjunitprice = 0;//云估价均价值
                        int pricetype = 0;//云估价价格来源类型                       

                        //起始日期
                        DateTime starttime = DateTime.Now.AddMonths(-3);
                        DateTime endtime = DateTime.Now;

                        #region 银行端自动估价
                        SYSCode code = SYSCodeBL.GetCode(1002, purpose);
                        int purposetype = (code != null && code.code != null) ? code.code.Value : 0;//物业类型 
                        if (purposetype == 1002001)//普通住宅
                        {
                            int buildingareatype = CodeHelper.GetBuildingAreaType(buildingArea);//面积类型
                            int buildingtypecode = CodeHelper.GetBuildingTypeCode(totalfloor);//楼栋类型
                            #region 普通住宅
                            //当运维中心中建了该楼盘数据，且房号系数已建可实现自动估价时，则住宅房号价格直接调用云估价房号自动估价结果
                            DataSet ds = DatHouseBL.GetEValueByProjectId(cityid, projectId, buildingId, houseId, fxtcompanyid, 1, companyid, userId, buildingArea, starttime.ToString("yyyy-MM-dd"), endtime.ToString("yyyy-MM-dd"), 0, qid, systypecode, 0, 0, 0, 0);
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                autoPrice = new AutoPrice();
                                autoPrice.avgprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["avgPrice"].ToString());
                                autoPrice.beprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["BEPrice"].ToString());
                                autoPrice.casecount = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casecount"].ToString());
                                autoPrice.casemax = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casemax"].ToString());
                                autoPrice.casemin = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casemin"].ToString());
                                autoPrice.heprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["HEPrice"].ToString());
                                autoPrice.unitprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["UnitPrice"].ToString());
                                autoPrice.eprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["EPrice"].ToString());
                            }
                            if (houseId > 0 && autoPrice != null && autoPrice.unitprice > 100)
                            {
                                ygjunitprice = autoPrice.unitprice;
                                pricetype = 1;//自动估价
                            }
                            else //不能自动估价
                            {
                                decimal houseAreaCoefficient = 0;//获取房号面积修正系数
                                //获取房号面积修正系数
                                List<SysCodePrice> codepricelist = SysCodePriceBL.GetCodePriceList(cityid, purposetype, 1033005);
                                if (codepricelist != null && codepricelist.Count() > 0 && codepricelist.Where(o => o.code == buildingareatype).Count() > 0)
                                {
                                    houseAreaCoefficient = codepricelist.Where(o => o.code == buildingareatype).FirstOrDefault().price;
                                }

                                //获取房号修正系数CODE
                                int direction = 0;//朝向
                                int[] landscape = null;//景观
                                int lv = 0;//装修档次
                                int decorationprobabilit = 0;//装修成新率

                                #region 获取房号修正系数CODE
                                Dictionary<int, string> dic = new Dictionary<int, string>();//code dic
                                dic.Add(2004, direction_str);//朝向
                                dic.Add(2006, landscape_str);//景观
                                dic.Add(6026, lv_str);//装修档次
                                dic.Add(6012, decorationprobabilit_str);//装修成新率
                                List<SYSCode> codelist = SYSCodeBL.GetCodeList(dic);

                                if (codelist != null && codelist.Count() > 0)
                                {
                                    codelist = codelist.Where(o => o.code != null).ToList();//仅取具有code值的数据
                                    if (codelist.Where(o => o.id == 2004).Count() > 0) //朝向
                                    {
                                        direction = codelist.Where(o => o.id == 2004).FirstOrDefault().code.Value;
                                    }
                                    if (codelist.Where(o => o.id == 2006).Count() > 0)//景观
                                    {
                                        landscape = codelist.Where(o => o.id == 2006).Select(o => o.code.Value).ToArray();
                                    }
                                    if (codelist.Where(o => o.id == 6026).Count() > 0)//装修档次
                                    {
                                        lv = codelist.Where(o => o.id == 6026).FirstOrDefault().code.Value;
                                    }
                                    if (codelist.Where(o => o.id == 6012).Count() > 0)//装修成新率
                                    {
                                        decorationprobabilit = codelist.Where(o => o.id == 6012).FirstOrDefault().code.Value;
                                    }
                                }
                                #endregion

                                //获取楼盘建筑类型及面积段分类均价信息
                                List<DATProjectAvgPrice> list = null;
                                //list = DATProjectAvgPriceBL.GetProjectAvgPriceList(fxtcompanyid, cityid, projectId, new int[] { purposetype }, starttime, endtime, true, daterange);
                                //由本地读取改为调用WCF接口 获取九宫格数据 caoq 2014-3-28
                                wcf_projectavgprice.FxtAPIClient proprice = new wcf_projectavgprice.FxtAPIClient();
                                string curdate = DateTime.Now.ToString("yyyy-MM-dd"), avgstr = "";
                                var para = new { projectId = projectId, cityId = cityid, codeType = purposetype, date = endtime.ToString("yyyy-MM") };
                                object objprice = proprice.Entrance(curdate, GetCode(curdate), "D", "Cross", JSONHelper.ObjectToJSON(para));
                                proprice.Abort();
                                if (objprice != null)
                                {
                                    avgstr = objprice.ToString();
                                    LogHelper.Info(avgstr);
                                }
                                if (!string.IsNullOrEmpty(avgstr))
                                {
                                    list = JSONHelper.JSONStringToList<DATProjectAvgPrice>(avgstr);
                                    if (list != null)
                                    {
                                        list = list.Where(o => o.avgprice > 0).ToList();
                                    }
                                }

                                if (list != null && list.Count() > 0 && list.Where(o => o.avgprice > 0).Count() > 0)
                                {
                                    #region 根据均价表信息计算均价
                                    //均价信息存在相同建筑类型、面积段数据
                                    if (list.Where(o => o.buildingtypecode == buildingtypecode && o.buildingareatype == buildingareatype).Count() > 0)
                                    {
                                        //建筑类型&面积段的均价*房号修正系数
                                        DATProjectAvgPrice avgprice = list.Where(o => o.buildingtypecode == buildingtypecode && o.buildingareatype == buildingareatype).FirstOrDefault();
                                        ygjunitprice = avgprice.avgprice;
                                    }
                                    //均价信息存在相同建筑类型数据
                                    else if (list.Where(o => o.buildingtypecode == buildingtypecode).Count() > 0)
                                    {
                                        //当数据库未建该房号数据时，且对应的楼盘分类均价表没有该建筑类型和面积段的价格，则优先选择相同建筑类型，但不同面积段的其他均价各自进行面积差修正后求平均值，得到该建筑类型下，对应面积段的均价值，再进行个别房号系数修正得到具体房号价格。
                                        //建筑类型的均价*面积差*房号修正系数
                                        List<DATProjectAvgPrice> btypelst = list.Where(o => o.buildingtypecode == buildingtypecode).ToList();
                                        //3个月之间，周边5公里范围内，竣工时间偏差3年,建筑类型一样条件的10条案例,进行面积差修正，再进行算术平均值作为该对应楼盘的均价
                                        decimal sumprice = 0;
                                        decimal areacoefficient = 0;
                                        int cnt = 0;
                                        btypelst.ForEach(o =>
                                        {
                                            //获取修正系数
                                            if (codepricelist != null && codepricelist.Count() > 0 && codepricelist.Where(t => t.code == o.buildingareatype).Count() > 0)
                                            {
                                                areacoefficient = codepricelist.Where(t => t.code == o.buildingareatype).FirstOrDefault().price;
                                                sumprice += o.avgprice / areacoefficient;
                                                cnt++;//进行过系数修正的值才列入总数
                                            }
                                        });
                                        ygjunitprice = sumprice / cnt * houseAreaCoefficient;
                                    }
                                    //均价信息存在相同面积段数据
                                    else if (list.Where(o => o.buildingareatype == buildingareatype).Count() > 0)
                                    {
                                        //当数据库未建该房号数据时，且该楼盘相同建筑类型的任意面积段均价都为0，则调用该楼盘相同面积段，不同建筑类型的均价进行求平均，得到的均价值进行个别房号系数修正算出该房号价格。
                                        //面积段的均价*房号修正系数
                                        List<DATProjectAvgPrice> btypelst = list.Where(o => o.buildingareatype == buildingareatype).ToList();
                                        ygjunitprice = (decimal)btypelst.Sum(o => o.avgprice) / (decimal)btypelst.Count();
                                    }
                                    else
                                    {
                                        //当数据库未建该房号数据时，且相同的建筑类型的均价都为0，相同的面积段均价都为0时，则用剩余的其他分类型均价的平均值进行个别房号系数修正得到具体房号价格。
                                        ygjunitprice = (decimal)list.Sum(o => o.avgprice) / (decimal)list.Count();
                                    }
                                    //房号系数修正
                                    ygjunitprice = HouseCoefficient(ygjunitprice, cityid, purposetype, totalfloor, floornumber, direction, landscape, lv, decorationprobabilit);
                                    //取两位有效小数
                                    ygjunitprice = Math.Round(ygjunitprice, 2);

                                    pricetype = 2;//均价修正
                                    #endregion
                                }
                                else if (buildingenddate > default(DateTime))//需要获取竣工时间偏差3年案例，必须存在竣工时间
                                {
                                    #region 获取楼盘周边案例
                                    List<Dat_AroundCase> caselist = DATCaseBL.GetProjectAroundCase(companyid, cityid, projectId, buildingareatype, buildingtypecode, buildingenddate, areaid, surveyx, surveyy, starttime, endtime);
                                    if (caselist != null && caselist.Count() > 0) caselist = caselist.Where(o => o.unitprice > 0).ToList();//只取存在价格的案例
                                    if (caselist != null && caselist.Count() >= 10) //10条案例才可计算起算
                                    {
                                        int casedatatype = caselist.FirstOrDefault().datatype;
                                        if (casedatatype == 1)
                                        {
                                            //3个月之间，周边5公里范围内，竣工时间偏差3年,建筑类型、面积段一样条件的10条案例的算术平均值作为该对应楼盘的均价
                                            ygjunitprice = caselist.Average(o => o.unitprice);
                                            pricetype = 3;//5公里内案例修正
                                        }
                                        else if (casedatatype == 2)
                                        {
                                            //3个月之间，周边5公里范围内，竣工时间偏差3年,建筑类型一样条件的10条案例,进行面积差修正，再进行算术平均值作为该对应楼盘的均价
                                            decimal sumprice = 0;
                                            decimal areacoefficient = 0;
                                            int cnt = 0;
                                            caselist.ForEach(o =>
                                            {
                                                //获取修正系数
                                                if (codepricelist != null && codepricelist.Count() > 0 && codepricelist.Where(t => t.code == o.buildingareatype).Count() > 0)
                                                {
                                                    areacoefficient = codepricelist.Where(t => t.code == o.buildingareatype).FirstOrDefault().price;
                                                    sumprice += o.unitprice / areacoefficient;
                                                    cnt++;//进行过系数修正的值才列入总数
                                                }
                                            });
                                            ygjunitprice = sumprice / cnt;
                                            pricetype = 3;//5公里内案例修正
                                        }
                                        else
                                        {
                                            //3个月之间，所在行政区，竣工时间偏差3年之内，相同建筑类型和面积段的各分类楼盘均价的平均值
                                            ygjunitprice = caselist.Average(o => o.unitprice);
                                            pricetype = 4;//行政区案例修正
                                        }
                                        ygjunitprice = Math.Round(ygjunitprice, 2);
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        else if (purposetype == 1002005 || purposetype == 1002006 || purposetype == 1002007 || purposetype == 1002008)
                        {
                            #region 别墅
                            //获取楼盘各别墅类型均价(别墅内不区分细分类型均价是code为：1002027) 别墅（独立1002005、联排1002006、叠加1002007、双拼1002008）
                            List<DATProjectAvgPrice> list = null;
                            //list = DATProjectAvgPriceBL.GetProjectAvgPriceList(fxtcompanyid, cityid, projectId, new int[] { 1002027, 1002005, 1002006, 1002007, 1002008 }, starttime, endtime, true, daterange);
                            //由本地读取改为调用WCF接口 获取九宫格数据 caoq 2014-3-28
                            wcf_projectavgprice.FxtAPIClient proprice = new wcf_projectavgprice.FxtAPIClient();
                            string curdate = DateTime.Now.ToString("yyyy-MM-dd"), avgstr = "";
                            //别墅类型可直接根据1002027获取所有别墅类型均价
                            var para = new { fxtcompanyid = fxtcompanyid, projectId = projectId, cityId = cityid, codeType = 1002027, date = DateTime.Now.ToString("yyyy-MM") };
                            object objprice = proprice.Entrance(curdate, GetCode(curdate), "D", "Cross", JSONHelper.ObjectToJSON(para));
                            proprice.Abort();
                            if (objprice != null)
                            {
                                avgstr = objprice.ToString();
                                LogHelper.Info(avgstr);
                            }
                            if (!string.IsNullOrEmpty(avgstr))
                            {
                                list = JSONHelper.JSONStringToList<DATProjectAvgPrice>(avgstr);
                                if (list != null)
                                {
                                    list = list.Where(o => o.avgprice > 0).ToList();
                                }
                            }

                            if (list != null && list.Count() > 0 && list.Where(o => o.avgprice > 0).Count() > 0)
                            {
                                if (list != null && list.Count() > 0)
                                {
                                    if (list.Where(o => o.purposetype == purposetype).Count() > 0) //获取具体类型均价
                                    {
                                        ygjunitprice = list.Where(o => o.purposetype == purposetype).FirstOrDefault().avgprice;
                                    }
                                    else if (list.Where(o => o.purposetype == 1002027).Count() > 0) //获取别墅均价
                                    {
                                        ygjunitprice = list.Where(o => o.purposetype == 1002027).FirstOrDefault().avgprice;
                                    }
                                    else
                                    {
                                        //根据各类型别墅均价的算术平均值
                                        ygjunitprice = (decimal)list.Sum(o => o.avgprice) / (decimal)list.Count();
                                    }
                                    pricetype = 2;
                                }
                            }
                            else
                            {
                                /*直接调用整个行政区内所有别墅楼盘该类型均价的平均值(三个月)*/
                                List<DATCase> caselist = DATCaseBL.GetAreaCase(fxtcompanyid, cityid, areaid, purposetype, starttime, endtime);
                                if (caselist != null && caselist.Count() > 0) caselist = caselist.Where(o => o.unitprice > 0).ToList();//只取存在价格的案例
                                if (caselist != null && caselist.Count() > 0)
                                {
                                    pricetype = 4;
                                    ygjunitprice = (decimal)caselist.Sum(o => o.unitprice) / (decimal)caselist.Count();
                                }
                            }
                            #endregion
                        }
                        #endregion

                        //返回结果
                        if (ygjunitprice > 0 && pricetype > 0)
                        {
                            autoPrice = new AutoPrice() { unitprice = ygjunitprice, pricetype = pricetype };
                        }
                        result = GetJson(autoPrice);
                        break;
                    default:
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

        /// <summary>
        /// 房号修正系数
        /// </summary>
        /// <param name="houseunitprice"></param>
        /// <param name="cityid"></param>
        /// <param name="totalfloor"></param>
        /// <param name="floornumber"></param>
        /// <param name="direction"></param>
        /// <param name="landscape"></param>
        /// <param name="lv"></param>
        /// <param name="decorationprobabilit"></param>
        /// <returns></returns>
        public decimal HouseCoefficient(decimal houseunitprice, int cityid, int purposecode, int totalfloor, int floornumber, int direction, int[] landscape, int lv, int decorationprobabilit)
        {
            if (houseunitprice <= 0) return 0;
            //获取房号需要修正的系数(楼层、装修)
            List<SysCodePrice> cofficientList = SysCodePriceBL.GetCodePriceList(cityid, purposecode, totalfloor, floornumber, lv, decorationprobabilit);

            if (cofficientList != null && cofficientList.Count() > 0)
            {
                //修正房号价格
                foreach (SysCodePrice item in cofficientList)
                {
                    houseunitprice += houseunitprice * item.price;
                }
            }
            //获取房号需要修正的系数(朝向、景观)
            List<int> codelst = new List<int>();
            if (landscape != null && landscape.Count() > 0)
                codelst.AddRange(landscape);
            if (direction > 0)
                codelst.Add(direction);
            if (codelst.Count() > 0)
            {
                List<SysCodePrice> codepriceList = SysCodePriceBL.GetCodePriceList(cityid, purposecode, codelst.ToArray());
                if (codepriceList != null && codepriceList.Count() > 0)
                {
                    //修正房号价格
                    foreach (SysCodePrice item in codepriceList)
                    {
                        houseunitprice += houseunitprice * item.price;
                    }
                }
            }
            return houseunitprice;
        }
    }
}