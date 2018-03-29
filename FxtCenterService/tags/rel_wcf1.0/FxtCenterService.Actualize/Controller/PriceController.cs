using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity;
using CAS.Common;
using Newtonsoft.Json.Linq;
using FxtCenterService.Logic;
using CAS.Entity.DBEntity;
using System.Data;
using CAS.Entity.GJBEntity;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>
        /// 自动估价
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetProjectEValue(JObject funinfo, UserCheck company) 
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            int companyid = StringHelper.TryGetInt(funinfo.Value<string>("companyid")) == 0 ? company.companyid : StringHelper.TryGetInt(funinfo.Value<string>("companyid"));
            search.FxtCompanyId = search.FxtCompanyId == 365 ? 25 : search.FxtCompanyId;
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            string StartDate = string.Empty;
            string EndDate = string.Empty;
            AutoPrice autoPrice = null;//自动估价结果
            autoPrice = DatHouseBL.GetEValueByProjectId(search.CityId, projectId, buildingId, houseId, search.FxtCompanyId,
         companyid, company.username, buildingArea, StartDate, EndDate);
            return autoPrice.ToJson();
        }

        /// <summary>
        /// 银行自动估价
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetEValueByProjectIdWithCmb(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid;
            search.FxtCompanyId = search.FxtCompanyId == 365 ? 25 : search.FxtCompanyId;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            int companyid = StringHelper.TryGetInt(funinfo.Value<string>("companyid"));
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            string purpose = funinfo.Value<string>("purpose");
            int totalfloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));//总楼层数
            int floornumber = StringHelper.TryGetInt(funinfo.Value<string>("floornumber"));//评估楼层
            DateTime buildingenddate = StringHelper.TryGetDateTime(funinfo.Value<string>("buildingenddate"));//竣工时间
            int areaid = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));//行政区ID
            //房号修正系数
            string direction_str = funinfo.Value<string>("direction");//朝向
            string landscape_str = funinfo.Value<string>("landscape");//景观
            string lv_str = funinfo.Value<string>("lv");//装修档次
            string decorationprobabilit_str = funinfo.Value<string>("decorationprobabilit");//装修成新率
            //查勘坐标
            double surveyx = StringHelper.TryGetDouble(funinfo.Value<string>("surveyx"));
            double surveyy = StringHelper.TryGetDouble(funinfo.Value<string>("surveyy"));

            int qid = StringHelper.TryGetInt(funinfo.Value<string>("qid"));
            int systypecode = StringHelper.TryGetInt(funinfo.Value<string>("systypecode"));
            int daterange = (string.IsNullOrEmpty(funinfo.Value<string>("daterange"))) ? 3 : StringHelper.TryGetInt(funinfo.Value<string>("daterange"));//价格计算范围（默认为3）
            if (companyid == 0)
                companyid = search.FxtCompanyId;
            string StartDate = string.Empty;
            string EndDate = string.Empty;
            decimal ygjunitprice = 0;//云估价均价值
            int pricetype = 0;//云估价价格来源类型                       

            //起始日期
            DateTime starttime = DateTime.Now.AddMonths(-3);
            DateTime endtime = DateTime.Now;

            #region 银行端自动估价
            AutoPrice autoPrice = null;//自动估价结果
            SYSCode code = SYSCodeBL.GetCode(1002, purpose);
            int purposetype = (code != null && code.code != null) ? code.code.Value : 0;//物业类型 
            if (purposetype == 1002001)//普通住宅
            {
                int buildingareatype = CodeHelper.GetBuildingAreaType(buildingArea);//面积类型
                int buildingtypecode = CodeHelper.GetBuildingTypeCode(totalfloor);//楼栋类型
                #region 普通住宅
                //当运维中心中建了该楼盘数据，且房号系数已建可实现自动估价时，则住宅房号价格直接调用云估价房号自动估价结果
                DataSet ds = DatHouseBL.GetEValueByProjectId(search.CityId, projectId, buildingId, houseId, search.FxtCompanyId, 1, companyid, company.username, buildingArea, starttime.ToString("yyyy-MM-dd"), endtime.ToString("yyyy-MM-dd"), 0, qid, systypecode, 0, 0, 0, 0);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    autoPrice = new AutoPrice();
                    autoPrice.avgprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["avgPrice"].ToString());
                    autoPrice.beprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["BEPrice"].ToString());
                    if (ds.Tables.Count > 1)
                    {
                        autoPrice.casecount = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casecount"].ToString());
                        autoPrice.casemax = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casemax"].ToString());
                        autoPrice.casemin = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casemin"].ToString());
                    }
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
                    List<SysCodePrice> codepricelist = SysCodePriceBL.GetCodePriceList(search.CityId, purposetype, 1033005);
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
                    var para = new { projectId = projectId, cityId = search.CityId, codeType = purposetype, date = endtime.ToString("yyyy-MM") };
                    object objprice = proprice.Entrance(curdate, DataCenterCommon.GetCode(curdate), "D", "Cross", JSONHelper.ObjectToJSON(para));
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
                        ygjunitprice = HouseCoefficient(ygjunitprice, search.CityId, purposetype, totalfloor, floornumber, direction, landscape, lv, decorationprobabilit);
                        //取两位有效小数
                        ygjunitprice = Math.Round(ygjunitprice, 2);

                        pricetype = 2;//均价修正
                        #endregion
                    }
                    else if (buildingenddate > default(DateTime))//需要获取竣工时间偏差3年案例，必须存在竣工时间
                    {
                        #region 获取楼盘周边案例
                        List<Dat_AroundCase> caselist = DATCaseBL.GetProjectAroundCase(companyid, search.CityId, projectId, buildingareatype, buildingtypecode, buildingenddate, areaid, surveyx, surveyy, starttime, endtime);
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
                var para = new { fxtcompanyid = search.FxtCompanyId, projectId = projectId, cityId = search.CityId, codeType = 1002027, date = DateTime.Now.ToString("yyyy-MM") };
                object objprice = proprice.Entrance(curdate, DataCenterCommon.GetCode(curdate), "D", "Cross", JSONHelper.ObjectToJSON(para));
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
                    List<DATCase> caselist = DATCaseBL.GetAreaCase(search.FxtCompanyId, search.CityId, areaid, purposetype, starttime, endtime);
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
            result = autoPrice.ToJson();
            return result;
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
        private static decimal HouseCoefficient(decimal houseunitprice, int cityid, int purposecode, int totalfloor, int floornumber, int direction, int[] landscape, int lv, int decorationprobabilit)
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

        #region 银行使用数据接口
        /// <summary>
        /// 获取楼盘建筑类型及面积段分类均价信息（直接调取数据库数据）
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetProjectAvgPriceList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid;
            search.FxtCompanyId = search.FxtCompanyId == 365 ? 25 : search.FxtCompanyId;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string purpose = funinfo.Value<string>("purpose");
            bool onlyhasprice = StringHelper.TryGetBool(funinfo.Value<string>("onlyhasprice"));
            int daterange = (string.IsNullOrEmpty(funinfo.Value<string>("daterange"))) ? 3 : StringHelper.TryGetInt(funinfo.Value<string>("daterange"));//价格计算范围（默认为3）

            string begindate = funinfo.Value<string>("startdate");
            string lastdate = funinfo.Value<string>("enddate");
            if (string.IsNullOrEmpty(begindate))
                begindate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrEmpty(lastdate))
                lastdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime startdate = StringHelper.TryGetDateTime(begindate);
            DateTime enddate = StringHelper.TryGetDateTime(lastdate);
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
            List<DATProjectAvgPrice> list = DATProjectAvgPriceBL.GetProjectAvgPriceList(search.FxtCompanyId,search.CityId, projectid, purposetype, startdate, enddate, onlyhasprice, daterange);
            return list.ToJson();
        }
        /// <summary>
        /// 均价走势图
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetCityAreaAvgPriceTrend(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid;
            search.FxtCompanyId = search.FxtCompanyId == 365 ? 25 : search.FxtCompanyId;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string purpose = funinfo.Value<string>("purpose");
            bool onlyhasprice = StringHelper.TryGetBool(funinfo.Value<string>("onlyhasprice"));
            int daterange = (string.IsNullOrEmpty(funinfo.Value<string>("daterange"))) ? 3 : StringHelper.TryGetInt(funinfo.Value<string>("daterange"));//价格计算范围（默认为3）

            string begindate = funinfo.Value<string>("startdate");
            string lastdate = funinfo.Value<string>("enddate");
            if (string.IsNullOrEmpty(begindate))
                begindate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrEmpty(lastdate))
                lastdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime startdate = StringHelper.TryGetDateTime(begindate);
            DateTime enddate = StringHelper.TryGetDateTime(lastdate);
            SYSCode purposecode = SYSCodeBL.GetCode(1002, purpose);
            int purposetypecode = (purposecode != null && purposecode.code != null) ? purposecode.code.Value : 0;//物业类型 
            if (purpose != "普通住宅") purposetypecode = 1002027;
            //调用运维中心城市、行政区均价（6个月）                                               
            TimeSpan dtShow = enddate.Subtract(startdate); //两个时间差                       
            //调用月份
            int monthcnt = Math.Abs(((enddate.Year - startdate.Year) * 12 + (enddate.Month - startdate.Month)));
            DataSet ds = monthcnt > 0 ? DATAvgPriceMonthBL.GetCityAreaAvgPriceTrend(monthcnt, search.CityId, search.AreaId, startdate, enddate) : null;
            if (ds != null && ds.Tables.Count > 0)
            {
                //调用WCF接口获取楼盘细分类型均价                       
                for (int i = 0; i < monthcnt; i++)
                {
                    wcf_projectavgprice.FxtAPIClient proprice = new wcf_projectavgprice.FxtAPIClient();
                    string curdate = DateTime.Now.ToString("yyyy-MM-dd"), avgstr = "";
                    DateTime avgpricedate = startdate.AddMonths(i);
                    var para = new { projectId = projectid, cityId = search.CityId, codeType = purposetypecode, date = avgpricedate.ToString("yyyy-MM") };
                    object objprice = proprice.Entrance(curdate, DataCenterCommon.GetCode(curdate), "D", "CrossProjectByCodeType", JSONHelper.ObjectToJSON(para));
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
            }
            return ds.Tables[0].ToJson();
        }
        /// <summary>
        /// 周边同质楼盘均价
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetSameProjectCasePrice(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid;
            search.FxtCompanyId = search.FxtCompanyId == 365 ? 25 : search.FxtCompanyId;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int areaid = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            string purpose = funinfo.Value<string>("purpose");
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            int totalfloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));//总楼层数

            //查勘坐标
            double surveyx = StringHelper.TryGetDouble(funinfo.Value<string>("surveyx"));
            double surveyy = StringHelper.TryGetDouble(funinfo.Value<string>("surveyy"));

            string begindate = funinfo.Value<string>("startdate");
            string lastdate = funinfo.Value<string>("enddate");
            if (string.IsNullOrEmpty(begindate))
                begindate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrEmpty(lastdate))
                lastdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime startdate = StringHelper.TryGetDateTime(begindate);
            DateTime enddate = StringHelper.TryGetDateTime(lastdate);

            DateTime buildingenddate = StringHelper.TryGetDateTime(funinfo.Value<string>("buildingenddate"));//竣工时间
            int buildingareatype = CodeHelper.GetBuildingAreaType(buildingArea);//面积类型
            int buildingtypecode = CodeHelper.GetBuildingTypeCode(totalfloor);//楼栋类型

            DataSet sameprojectDs = DATCaseBL.GetSameProjectCasePrice(search.FxtCompanyId, search.CityId, projectid, buildingareatype, buildingtypecode, buildingenddate, areaid, surveyx, surveyy, startdate, enddate);
            if (sameprojectDs != null && sameprojectDs.Tables.Count > 0)
            {
                result = sameprojectDs.Tables[0].ToJson();
            }
            return result;
        }
        /// <summary>
        /// 不同渠道楼盘均价对比
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetOtherChannelCasePrice(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid;
            search.FxtCompanyId = search.FxtCompanyId == 365 ? 25 : search.FxtCompanyId;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string purpose = funinfo.Value<string>("purpose");
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            int totalfloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));//总楼层数

            int daterange = (string.IsNullOrEmpty(funinfo.Value<string>("daterange"))) ? 3 : StringHelper.TryGetInt(funinfo.Value<string>("daterange"));//价格计算范围（默认为3）

            string begindate = funinfo.Value<string>("startdate");
            string lastdate = funinfo.Value<string>("enddate");
            if (string.IsNullOrEmpty(begindate))
                begindate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrEmpty(lastdate))
                lastdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime startdate = StringHelper.TryGetDateTime(begindate);
            DateTime enddate = StringHelper.TryGetDateTime(lastdate);

            int buildingareatype = CodeHelper.GetBuildingAreaType(buildingArea);//面积类型
            int buildingtypecode = CodeHelper.GetBuildingTypeCode(totalfloor);//楼栋类型

            SYSCode purposecode = SYSCodeBL.GetCode(1002, purpose);
            int purposetypecode = (purposecode != null && purposecode.code != null) ? purposecode.code.Value : 0;//物业类型 

            DataSet otherchannelDs = DATCaseBL.GetOtherChannelCasePrice(search.FxtCompanyId,search.CityId, projectid, purposetypecode, buildingareatype, buildingtypecode, startdate, enddate, daterange);
            if (otherchannelDs != null && otherchannelDs.Tables.Count > 0)
            {
                result = otherchannelDs.Tables[0].ToJson();
            }
            return result;
        }
        /// <summary>
        /// 地图价格、环比跌涨幅
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetMapPrice(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid;
            search.FxtCompanyId = search.FxtCompanyId == 365 ? 25 : search.FxtCompanyId;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string projectname = funinfo.Value<string>("projectname");
            string purpose = funinfo.Value<string>("purpose");

            int daterange = (string.IsNullOrEmpty(funinfo.Value<string>("daterange"))) ? 3 : StringHelper.TryGetInt(funinfo.Value<string>("daterange"));//价格计算范围（默认为3）
            //查勘坐标
            double surveyx = StringHelper.TryGetDouble(funinfo.Value<string>("surveyx"));
            double surveyy = StringHelper.TryGetDouble(funinfo.Value<string>("surveyy"));

            string begindate = funinfo.Value<string>("startdate");
            string lastdate = funinfo.Value<string>("enddate");
            if (string.IsNullOrEmpty(begindate))
                begindate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrEmpty(lastdate))
                lastdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime startdate = StringHelper.TryGetDateTime(begindate);
            DateTime enddate = StringHelper.TryGetDateTime(lastdate);

            SYSCode purposecode = SYSCodeBL.GetCode(1002, purpose);
            int purposetypecode = (purposecode != null && purposecode.code != null) ? purposecode.code.Value : 0;//物业类型 
            if (purposetypecode != 1002001) purposetypecode = 1002027;//别墅类型
            DataSet mapds = DATProjectAvgPriceBL.GetMapPrice(search.CityId, search.FxtCompanyId, projectid, projectname, surveyx, surveyy, purposetypecode, enddate, daterange);
            if (mapds != null && mapds.Tables.Count > 0)
            {
                result = mapds.Tables[0].ToJson();
            }
            return result;
        }
        #endregion 
    }
}
