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
using CAS.Entity.FxtProject;
//using FxtOpenClient.ClientService;
//using FxtCommon.Openplatform.GrpcService;
//using FxtCommon.Openplatform.Data;
using CAS.Entity.FxtLog;

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
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            int type = StringHelper.TryGetInt(funinfo.Value<string>("type")) == 0 ? 0 : StringHelper.TryGetInt(funinfo.Value<string>("type"));
            int systypecode = company.parentproducttypecode; //StringHelper.TryGetInt(funinfo.Value<string>("systypecode")) == 0 ? 1003001 : StringHelper.TryGetInt(funinfo.Value<string>("systypecode"));
            string userid = string.IsNullOrEmpty(funinfo.Value<string>("userid")) ? "" : funinfo.Value<string>("userid");
            //int companyid = StringHelper.TryGetInt(funinfo.Value<string>("companyid")) == 0 ? company.parentshowdatacompanyid : StringHelper.TryGetInt(funinfo.Value<string>("companyid"));
            int companyid = company.companyid;
            search.FxtCompanyId = company.parentshowdatacompanyid;
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            string StartDate = string.Empty;
            string EndDate = string.Empty;
            CAS.Entity.AutoPrice autoPrice = null;//自动估价结果
            autoPrice = DatHouseBL.GetEValueByProjectId(search.CityId, projectId, buildingId, houseId, search.FxtCompanyId, type,
         companyid, company.username, buildingArea, StartDate, EndDate, systypecode);
            return autoPrice.ToJson();
        }

        /// <summary>
        /// 自动估价，不往数据中心插入自动估价记录
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetCASEValueByPId(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            //0，返回自动估价使用的案例；1，不返回自动估价使用的案例
            int type = StringHelper.TryGetInt(funinfo.Value<string>("type")) == 0 ? 0 : StringHelper.TryGetInt(funinfo.Value<string>("type"));
            //自动估价来源产品：默认为1003001（CAS）
            //int systypecode = StringHelper.TryGetInt(funinfo.Value<string>("systypecode")) == 0 ? 1003001 : StringHelper.TryGetInt(funinfo.Value<string>("systypecode"));
            int systypecode = company.parentproducttypecode;
            //账号
            string userid = string.IsNullOrEmpty(funinfo.Value<string>("userid")) ? "" : funinfo.Value<string>("userid");
            //客户公司id
            int companyid = company.companyid;// StringHelper.TryGetInt(funinfo.Value<string>("companyid")) == 0 ? company.companyid : StringHelper.TryGetInt(funinfo.Value<string>("companyid"));
            //自动估价目的；默认为1004001
            int queryTypeCode = StringHelper.TryGetInt(funinfo.Value<string>("queryTypeCode")) == 0 ? 1004001 : StringHelper.TryGetInt(funinfo.Value<string>("queryTypeCode"));
            //评估机构ID
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int qid = StringHelper.TryGetInt(funinfo.Value<string>("qid"));
            int subhousetype = StringHelper.TryGetInt(funinfo.Value<string>("subhousetype"));
            double subhousearea = StringHelper.TryGetDouble(funinfo.Value<string>("subhousearea"));
            double subhouseavgprice = StringHelper.TryGetDouble(funinfo.Value<string>("subhouseavgprice"));
            double subhousetotalprice = StringHelper.TryGetDouble(funinfo.Value<string>("subhousetotalprice"));
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            string StartDate = string.Empty;
            string EndDate = string.Empty;
            CAS.Entity.AutoPrice autoPrice = null;//自动估价结果
            autoPrice = DatHouseBL.GetCASEValueByPId(search.CityId, projectId, buildingId, houseId, search.FxtCompanyId, type,
             companyid, company.username, buildingArea, StartDate, EndDate, queryTypeCode, qid, systypecode, subhousetype, subhousearea, subhouseavgprice, subhousetotalprice);
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
            search.FxtCompanyId = company.parentshowdatacompanyid;// company.companyid;
            //search.FxtCompanyId = search.FxtCompanyId == 365 || search.FxtCompanyId == 218 ? 25 : search.FxtCompanyId;
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            int companyid = StringHelper.TryGetInt(funinfo.Value<string>("companyid"));
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            string purpose = funinfo.Value<string>("purpose");//用途
            int totalfloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));//总楼层数
            int floornumber = StringHelper.TryGetInt(funinfo.Value<string>("floornumber"));//评估楼层
            DateTime buildingenddate = StringHelper.TryGetDateTime(funinfo.Value<string>("buildingenddate"));//竣工时间
            int areaid = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));//行政区ID
            double fivemiles = StringHelper.TryGetDouble(funinfo.Value<string>("fivemiles"));//坐标定位偏差（默认5公里（0.5））
            //房号修正系数
            string direction_str = funinfo.Value<string>("direction");//朝向
            string landscape_str = funinfo.Value<string>("landscape");//景观
            string lv_str = funinfo.Value<string>("lv");//装修档次
            string decorationprobabilit_str = funinfo.Value<string>("decorationprobabilit");//装修成新率
            //查勘坐标
            double surveyx = StringHelper.TryGetDouble(funinfo.Value<string>("surveyx"));
            double surveyy = StringHelper.TryGetDouble(funinfo.Value<string>("surveyy"));

            int qid = StringHelper.TryGetInt(funinfo.Value<string>("qid"));
            //int systypecode = StringHelper.TryGetInt(funinfo.Value<string>("systypecode"));
            int systypecode = company.parentproducttypecode;
            int daterange = (string.IsNullOrEmpty(funinfo.Value<string>("daterange"))) ? 3 : StringHelper.TryGetInt(funinfo.Value<string>("daterange"));//价格计算范围（默认为3）
            if (companyid == 0)
                companyid = search.FxtCompanyId;
            string StartDate = string.Empty;
            string EndDate = string.Empty;
            decimal ygjunitprice = 0;//云估价均价值
            int pricetype = 0;//云估价价格来源类型 

            List<DATProjectAvgPrice> avgpricelist = null;//楼盘建筑类型及面积段分类均价

            //起始日期
            DateTime endtime = DateTime.Now;
            //获取城市信息（案例获取日期根据设置来） caoq 2014-4-24
            int casemonth = SYSCityBL.GetCityCaseMonth(search.CityId);
            DateTime starttime = endtime.AddMonths(casemonth * -1);//开始日期根据设置参数来

            #region 银行端自动估价
            CAS.Entity.AutoPrice autoPrice = null;//自动估价结果
            SYSCode code = SYSCodeBL.GetCode(1002, purpose);
            int purposetype = (code != null && code.code != null) ? code.code.Value : 0;//物业类型 
            if (purposetype == 1002001)//普通住宅
            {
                int buildingareatype = CodeHelper.GetBuildingAreaType(buildingArea);//面积类型
                int buildingtypecode = CodeHelper.GetBuildingTypeCode(totalfloor);//楼栋类型
                #region 普通住宅
                //获取楼盘建筑类型及面积段分类均价信息                  
                //avgpricelist = DATProjectAvgPriceBL.GetProjectAvgPriceList(fxtcompanyid, cityid, projectId, new int[] { purposetype }, starttime, endtime, true, daterange);
                //由本地读取改为调用WCF接口 获取楼盘细分类型价格 caoq 2014-3-28
                wcf_projectavgprice.FxtAPIClient proprice = new wcf_projectavgprice.FxtAPIClient();
                string curdate = DateTime.Now.ToString("yyyy-MM-dd"), avgstr = "";
                var para = new { projectId = projectId, cityId = search.CityId, codeType = purposetype, date = endtime.ToString("yyyy-MM") };
                object objprice = proprice.Entrance(curdate, DataCenterCommon.GetCode(curdate), "D", "Cross", JSONHelper.ObjectToJSON(para));
                proprice.Abort();
                if (objprice != null)
                {
                    avgstr = objprice.ToString();
                    //LogHelper.Info(avgstr);
                }
                if (!string.IsNullOrEmpty(avgstr))
                {
                    avgpricelist = JSONHelper.JSONStringToList<DATProjectAvgPrice>(avgstr);
                    if (avgpricelist != null)
                    {
                        avgpricelist = avgpricelist.Where(o => o.avgprice > 0).ToList();
                    }
                }

                //当运维中心中建了该楼盘数据，且房号系数已建可实现自动估价时，则住宅房号价格直接调用云估价房号自动估价结果
                string autoStartTime = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-01");//前三个月月初
                string autoEndTime = StringHelper.TryGetDateTime(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1).ToString("yyyy-MM-dd");//上个月月末

                DataSet ds = DatHouseBL.GetEValueByProjectId(search.CityId, projectId, buildingId, houseId, search.FxtCompanyId, 1, companyid, company.username, buildingArea, autoStartTime, autoEndTime, 0, qid, systypecode, 0, 0, 0, 0);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    autoPrice = new CAS.Entity.AutoPrice();
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
                //houseId > 0 && 去掉房号验证
                if (autoPrice != null && autoPrice.unitprice > 100)
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

                    if (avgpricelist != null && avgpricelist.Count() > 0 && avgpricelist.Where(o => o.avgprice > 0).Count() > 0)
                    {
                        #region 根据均价表信息计算均价
                        //均价信息存在相同建筑类型、面积段数据
                        if (avgpricelist.Where(o => o.buildingtypecode == buildingtypecode && o.buildingareatype == buildingareatype).Count() > 0)
                        {
                            //建筑类型&面积段的均价*房号修正系数
                            DATProjectAvgPrice avgprice = avgpricelist.Where(o => o.buildingtypecode == buildingtypecode && o.buildingareatype == buildingareatype).FirstOrDefault();
                            ygjunitprice = avgprice.avgprice;
                        }
                        //均价信息存在相同建筑类型数据
                        else if (avgpricelist.Where(o => o.buildingtypecode == buildingtypecode).Count() > 0)
                        {
                            //当数据库未建该房号数据时，且对应的楼盘分类均价表没有该建筑类型和面积段的价格，则优先选择相同建筑类型，但不同面积段的其他均价各自进行面积差修正后求平均值，得到该建筑类型下，对应面积段的均价值，再进行个别房号系数修正得到具体房号价格。
                            //建筑类型的均价*面积差*房号修正系数
                            List<DATProjectAvgPrice> btypelst = avgpricelist.Where(o => o.buildingtypecode == buildingtypecode).ToList();
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
                        else if (avgpricelist.Where(o => o.buildingareatype == buildingareatype).Count() > 0)
                        {
                            //当数据库未建该房号数据时，且该楼盘相同建筑类型的任意面积段均价都为0，则调用该楼盘相同面积段，不同建筑类型的均价进行求平均，得到的均价值进行个别房号系数修正算出该房号价格。
                            //面积段的均价*房号修正系数
                            List<DATProjectAvgPrice> btypelst = avgpricelist.Where(o => o.buildingareatype == buildingareatype).ToList();
                            ygjunitprice = (decimal)btypelst.Sum(o => o.avgprice) / (decimal)btypelst.Count();
                        }
                        else
                        {
                            //当数据库未建该房号数据时，且相同的建筑类型的均价都为0，相同的面积段均价都为0时，则用剩余的其他分类型均价的平均值进行个别房号系数修正得到具体房号价格。
                            ygjunitprice = (decimal)avgpricelist.Sum(o => o.avgprice) / (decimal)avgpricelist.Count();
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
                        List<Dat_AroundCase> caselist = DATCaseBL.GetProjectAroundCase(companyid, search.CityId, projectId, buildingareatype, buildingtypecode, buildingenddate, areaid, surveyx, surveyy, starttime, endtime, search.SysTypeCode, fivemiles);
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
                //avgpricelist = DATProjectAvgPriceBL.GetProjectAvgPriceList(fxtcompanyid, cityid, projectId, new int[] { 1002027, 1002005, 1002006, 1002007, 1002008 }, starttime, endtime, true, daterange);
                //由本地读取改为调用WCF接口 获取楼盘细分类型价格 caoq 2014-3-28
                wcf_projectavgprice.FxtAPIClient proprice = new wcf_projectavgprice.FxtAPIClient();
                string curdate = DateTime.Now.ToString("yyyy-MM-dd"), avgstr = "";
                //别墅类型可直接根据1002027获取所有别墅类型均价
                var para = new { fxtcompanyid = search.FxtCompanyId, projectId = projectId, cityId = search.CityId, codeType = 1002027, date = DateTime.Now.ToString("yyyy-MM") };
                object objprice = proprice.Entrance(curdate, DataCenterCommon.GetCode(curdate), "D", "Cross", JSONHelper.ObjectToJSON(para));
                proprice.Abort();
                if (objprice != null)
                {
                    avgstr = objprice.ToString();
                    //LogHelper.Info(avgstr);
                }
                if (!string.IsNullOrEmpty(avgstr))
                {
                    avgpricelist = JSONHelper.JSONStringToList<DATProjectAvgPrice>(avgstr);
                    if (avgpricelist != null)
                    {
                        avgpricelist = avgpricelist.Where(o => o.avgprice > 0).ToList();
                    }
                }

                if (avgpricelist != null && avgpricelist.Count() > 0 && avgpricelist.Where(o => o.avgprice > 0).Count() > 0)
                {
                    if (avgpricelist != null && avgpricelist.Count() > 0)
                    {
                        if (avgpricelist.Where(o => o.purposetype == purposetype).Count() > 0) //获取具体类型均价
                        {
                            ygjunitprice = avgpricelist.Where(o => o.purposetype == purposetype).FirstOrDefault().avgprice;
                        }
                        else if (avgpricelist.Where(o => o.purposetype == 1002027).Count() > 0) //获取别墅均价
                        {
                            ygjunitprice = avgpricelist.Where(o => o.purposetype == 1002027).FirstOrDefault().avgprice;
                        }
                        else
                        {
                            //根据各类型别墅均价的算术平均值
                            ygjunitprice = (decimal)avgpricelist.Sum(o => o.avgprice) / (decimal)avgpricelist.Count();
                        }
                        pricetype = 2;
                    }
                }
                else
                {
                    /*直接调用整个行政区内所有别墅楼盘该类型均价的平均值(三个月)*/
                    List<DATCase> caselist = DATCaseBL.GetAreaCase(search.FxtCompanyId, search.CityId, areaid, purposetype, starttime, endtime, search.SysTypeCode);
                    if (caselist != null && caselist.Count() > 0) caselist = caselist.Where(o => o.unitprice > 0).ToList();//只取存在价格的案例
                    if (caselist != null && caselist.Count() > 0)
                    {
                        pricetype = 4;
                        ygjunitprice = (decimal)caselist.Sum(o => o.unitprice * o.buildingarea) / (decimal)caselist.Sum(o => o.buildingarea);
                    }
                }
                #endregion
            }
            #endregion

            //返回结果
            if (ygjunitprice > 0 && pricetype > 0)
            {
                //avgpricelist 存储在银行数据库，确保每次显示一致
                autoPrice = new CAS.Entity.AutoPrice() { unitprice = ygjunitprice, pricetype = pricetype, avgpricelist = avgpricelist };
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
        /// 获取楼盘建筑类型及面积段分类均价信息（直接调取数据库数据）(暂未使用 caoq 2014-4-25)
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetProjectAvgPriceList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
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
            List<DATProjectAvgPrice> list = DATProjectAvgPriceBL.GetProjectAvgPriceList(search.FxtCompanyId, search.CityId, projectid, purposetype, startdate, enddate, onlyhasprice, daterange, search.SysTypeCode);
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
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string purpose = funinfo.Value<string>("purpose");
            bool onlyhasprice = StringHelper.TryGetBool(funinfo.Value<string>("onlyhasprice"));
            int daterange = (string.IsNullOrEmpty(funinfo.Value<string>("daterange"))) ? 3 : StringHelper.TryGetInt(funinfo.Value<string>("daterange"));//价格计算范围（默认为3）

            string begindate = funinfo.Value<string>("startdate");
            string lastdate = funinfo.Value<string>("enddate");
            if (string.IsNullOrEmpty(begindate))
                begindate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd HH:mm:ss");//默认获取一年的均价走势
            if (string.IsNullOrEmpty(lastdate))
                lastdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime startdate = StringHelper.TryGetDateTime(begindate);
            DateTime enddate = StringHelper.TryGetDateTime(lastdate);
            SYSCode purposecode = SYSCodeBL.GetCode(1002, purpose);
            int purposetypecode = (purposecode != null && purposecode.code != null) ? purposecode.code.Value : 0;//物业类型 
            if (purpose != "普通住宅") purposetypecode = 1002027;
            //调用运维中心城市、行政区均价                                             
            TimeSpan dtShow = enddate.Subtract(startdate); //两个时间差                       
            //调用月份
            int monthcnt = Math.Abs(((enddate.Year - startdate.Year) * 12 + (enddate.Month - startdate.Month)));
            try
            {
                DataSet ds = monthcnt > 0 ? DATAvgPriceMonthBL.GetCityAreaAvgPriceTrend(monthcnt, search.CityId, search.AreaId, startdate, enddate) : null;
                if (ds != null && ds.Tables.Count > 0)
                {
                    //调用WCF接口获取楼盘细分类型均价
                    if (1 == 2)//(暂不调取细分类型均价 caoq 2014-4-25)
                    {
                        #region 调取细分类型均价
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
                                //LogHelper.Info("九宫格接口返回：" + avgstr);
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
                        #endregion
                    }
                    return ds.Tables[0].ToJson();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                //return null;
                throw ex;
            }
        }
        /// <summary>
        /// 获取楼盘细分类型均价
        /// 创建人 caoq 2014-11-26
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetProjectTypePrice(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string purpose = funinfo.Value<string>("purpose");

            string avgdatestr = funinfo.Value<string>("avgdate");
            if (string.IsNullOrEmpty(avgdatestr))
                avgdatestr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime avgdate = StringHelper.TryGetDateTime(avgdatestr);
            SYSCode purposecode = SYSCodeBL.GetCode(1002, purpose);
            int purposetypecode = (purposecode != null && purposecode.code != null) ? purposecode.code.Value : 0;//物业类型 
            if (purpose != "普通住宅") purposetypecode = 1002027;
            try
            {
                wcf_projectavgprice.FxtAPIClient proprice = new wcf_projectavgprice.FxtAPIClient();
                string curdate = DateTime.Now.ToString("yyyy-MM-dd"), avgstr = "";
                //别墅类型可直接根据1002027获取所有别墅类型均价//fxtcompanyid = search.FxtCompanyId,
                var para = new { projectId = projectid, cityId = search.CityId, codeType = purposetypecode, date = avgdate.ToString("yyyy-MM") };
                object objprice = proprice.Entrance(curdate, DataCenterCommon.GetCode(curdate), "D", "Cross", JSONHelper.ObjectToJSON(para));
                proprice.Abort();
                if (objprice != null)
                {
                    avgstr = objprice.ToString();
                }
                List<DATProjectAvgPrice> avgpricelist = null;
                if (!string.IsNullOrEmpty(avgstr))
                {
                    avgpricelist = JSONHelper.JSONStringToList<DATProjectAvgPrice>(avgstr);
                    if (avgpricelist != null)
                    {
                        avgpricelist = avgpricelist.Where(o => o.avgprice > 0).ToList();
                    }
                }
                return avgpricelist.ToJson();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                //return null;
                throw ex;
            }
        }
        /// <summary>
        /// 周边同质楼盘均价(包含本月均价、上月均价、环比涨跌幅、坐标)
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetSameProjectCasePrice(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int areaid = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            string purpose = funinfo.Value<string>("purpose");
            int totalfloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));//总楼层数

            //查勘坐标
            double surveyx = StringHelper.TryGetDouble(funinfo.Value<string>("surveyx"));
            double surveyy = StringHelper.TryGetDouble(funinfo.Value<string>("surveyy"));

            string lastdate = funinfo.Value<string>("enddate");
            if (string.IsNullOrEmpty(lastdate))
                lastdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime enddate = StringHelper.TryGetDateTime(lastdate);
            //获取城市信息（案例获取日期根据设置来） caoq 2014-4-24
            int casemonth = SYSCityBL.GetCityCaseMonth(search.CityId);
            DateTime startdate = enddate.AddMonths(casemonth * -1);//开始日期根据设置参数来

            DateTime buildingenddate = StringHelper.TryGetDateTime(funinfo.Value<string>("buildingenddate"));//竣工时间
            int buildingtypecode = totalfloor > 0 ? CodeHelper.GetBuildingTypeCode(totalfloor) : 0;//未传递楼栋类型则不计算楼栋类型

            SYSCode purposecode = SYSCodeBL.GetCode(1002, purpose);
            int purposetypecode = (purposecode != null && purposecode.code != null) ? purposecode.code.Value : 0;//物业类型 

            try
            {
                DataSet sameprojectDs = DATCaseBL.GetSameProjectCasePrice(search.FxtCompanyId, search.CityId, projectid, buildingtypecode, buildingenddate, areaid, surveyx, surveyy, startdate, enddate, search.SysTypeCode);
                if (sameprojectDs != null && sameprojectDs.Tables.Count > 0)
                {
                    //当前楼盘上月价格调取
                    foreach (DataRow row in sameprojectDs.Tables[0].Rows)
                    {
                        if (StringHelper.TryGetInt(row["projectid"].ToString()) == projectid) //更新当前上月楼盘价格
                        {
                            //调用WCF接口 获取楼盘细分类型价格
                            wcf_projectavgprice.FxtAPIClient proprice = new wcf_projectavgprice.FxtAPIClient();
                            string curdate = DateTime.Now.ToString("yyyy-MM-dd"), avgstr = "";
                            if (purpose != "普通住宅") purposetypecode = 1002027;//别墅类型可直接根据1002027获取所有别墅类型均价
                            var para = new { fxtcompanyid = search.FxtCompanyId, projectId = projectid, cityId = search.CityId, codeType = purposetypecode, date = startdate.AddMonths(-1).ToString("yyyy-MM") };
                            object objprice = proprice.Entrance(curdate, DataCenterCommon.GetCode(curdate), "D", "Cross", JSONHelper.ObjectToJSON(para));
                            proprice.Abort();
                            if (objprice != null)
                            {
                                avgstr = objprice.ToString();
                                //LogHelper.Info(avgstr);
                            }
                            if (!string.IsNullOrEmpty(avgstr))
                            {
                                List<DATProjectAvgPrice> avgpricelist = JSONHelper.JSONStringToList<DATProjectAvgPrice>(avgstr);
                                //有细分类型均价，则获取细分类型均价价格，没有则调取楼盘均价
                                if (avgpricelist != null && avgpricelist.Where(o => o.avgprice > 0 && o.buildingtypecode == buildingtypecode).Count() > 0)
                                {
                                    avgpricelist = avgpricelist.Where(o => o.avgprice > 0 && o.buildingtypecode == buildingtypecode).ToList();
                                    row["preavgprice"] = avgpricelist.Sum(o => o.avgprice) / avgpricelist.Count();
                                }
                            }
                        }
                    }

                    result = sameprojectDs.Tables[0].ToJson();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
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
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;

            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string purpose = funinfo.Value<string>("purpose");
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            int totalfloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));//总楼层数

            int daterange = (string.IsNullOrEmpty(funinfo.Value<string>("daterange"))) ? 3 : StringHelper.TryGetInt(funinfo.Value<string>("daterange"));//价格计算范围（默认为3）

            string lastdate = funinfo.Value<string>("enddate");
            if (string.IsNullOrEmpty(lastdate))
                lastdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime enddate = StringHelper.TryGetDateTime(lastdate);
            //获取城市信息（案例获取日期根据设置来） caoq 2014-4-24
            int casemonth = SYSCityBL.GetCityCaseMonth(search.CityId);
            DateTime startdate = enddate.AddMonths(casemonth * -1);//开始日期根据设置参数来

            int buildingareatype = CodeHelper.GetBuildingAreaType(buildingArea);//面积类型
            int buildingtypecode = CodeHelper.GetBuildingTypeCode(totalfloor);//楼栋类型

            SYSCode purposecode = SYSCodeBL.GetCode(1002, purpose);
            int purposetypecode = (purposecode != null && purposecode.code != null) ? purposecode.code.Value : 0;//物业类型 
            try
            {
                DataSet otherchannelDs = DATCaseBL.GetOtherChannelCasePrice(search.FxtCompanyId, search.CityId, projectid, purposetypecode, buildingareatype, buildingtypecode, startdate, enddate, daterange, search.SysTypeCode);
                if (otherchannelDs != null && otherchannelDs.Tables.Count > 0)
                {
                    result = otherchannelDs.Tables[0].ToJson();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// 地图价格、环比跌涨幅 (暂未使用 caoq 2014-4-25)
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetMapPrice(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string projectname = funinfo.Value<string>("projectname");
            string purpose = funinfo.Value<string>("purpose");

            int daterange = (string.IsNullOrEmpty(funinfo.Value<string>("daterange"))) ? 3 : StringHelper.TryGetInt(funinfo.Value<string>("daterange"));//价格计算范围（默认为3）
            //查勘坐标
            double surveyx = StringHelper.TryGetDouble(funinfo.Value<string>("surveyx"));
            double surveyy = StringHelper.TryGetDouble(funinfo.Value<string>("surveyy"));

            string lastdate = funinfo.Value<string>("enddate");
            if (string.IsNullOrEmpty(lastdate))
                lastdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime enddate = StringHelper.TryGetDateTime(lastdate);
            //获取城市信息（案例获取日期根据设置来） caoq 2014-4-24
            int casemonth = SYSCityBL.GetCityCaseMonth(search.CityId);
            DateTime startdate = enddate.AddMonths(casemonth * -1);//开始日期根据设置参数来

            try
            {
                SYSCode purposecode = SYSCodeBL.GetCode(1002, purpose);
                int purposetypecode = (purposecode != null && purposecode.code != null) ? purposecode.code.Value : 0;//物业类型 
                if (purposetypecode != 1002001) purposetypecode = 1002027;//别墅类型

                //LogHelper.Info("地图价格、环比跌涨幅");
                DataSet mapds = DATProjectAvgPriceBL.GetMapPrice(search.CityId, search.FxtCompanyId, projectid, projectname, surveyx, surveyy, purposetypecode, enddate, daterange, search.SysTypeCode);
                if (mapds != null && mapds.Tables.Count > 0)
                {
                    result = mapds.Tables[0].ToJson();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                //throw ex;
            }
            return result;
        }
        #endregion


        /// <summary>
        /// 获取自动估价记录
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetDATQueryHistoryList(JObject funinfo, UserCheck company, JObject objSinfo, JObject objInfo)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            if (search.PageIndex == 0)
            {
                search.PageIndex = 1;
                search.PageRecords = 15;
                search.Page = true;
            }
            string username = funinfo.Value<string>("username");
            string wxopenid = funinfo.Value<string>("wxopenid");
            var datQueryHistory = DATQueryHistoryBL.GetDATQueryHistoryList(search, username, wxopenid, objSinfo, objInfo).Select(o => new
            {
                id = o.id,
                projectid = o.projectid,
                buildingid = o.buildingid,
                houseid = o.houseid,
                projectname = o.projectname,
                buildingname = o.buildingname,
                housename = o.housename,
                cityid = o.cityid,
                userid = o.userid,
                querydate = o.querydate,
                unitprice = o.unitprice,
                truename = o.truename,
                buildingarea = o.buildingarea,
                qid = o.qid,
                recordcount = o.recordcount
            });
            return datQueryHistory.ToJson();
        }



        /// <summary>
        /// 标准化楼盘楼栋房号匹配
        /// </summary>
        /// <param name="cityid">城市Id</param>
        /// <param name="projectname">楼盘名称</param>
        /// <param name="addresss">地址</param>
        /// <param name="buildingname">楼栋名称</param>
        /// <param name="housename">房号名称</param>
        /// <returns></returns>
        public static string GetMatchingData(JObject funinfo, UserCheck company)
        {
            int typecode = company.parentproducttypecode;
            int cityid = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            int p_projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int p_buildingid = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int fxtcompanyid = company.parentshowdatacompanyid;
            string projectname = funinfo.Value<string>("projectname");
            string addresss = funinfo.Value<string>("addresss");
            string buildingname = funinfo.Value<string>("buildingname");
            string housename = funinfo.Value<string>("housename");
            DataSet dset = DATQueryHistoryBL.GetMatchingData(cityid, projectname, addresss, buildingname, housename, p_projectid, p_buildingid, fxtcompanyid, typecode);
            if (dset != null && dset.Tables != null && dset.Tables[0].Rows.Count > 0)
            {
                var collmat = new
                {
                    projectid = dset.Tables[0].Columns.Contains("projectid") ? dset.Tables[0].Rows[0]["projectid"] : 0,
                    buildingid = dset.Tables[0].Columns.Contains("buildingid") ? dset.Tables[0].Rows[0]["buildingid"] : 0,
                    houseid = dset.Tables[0].Columns.Contains("houseid") ? dset.Tables[0].Rows[0]["houseid"] : 0,
                    projectname = dset.Tables[0].Columns.Contains("projectname") ? dset.Tables[0].Rows[0]["projectname"] : "",
                    address = dset.Tables[0].Columns.Contains("address") ? dset.Tables[0].Rows[0]["address"] : "",
                    buildingname = dset.Tables[0].Columns.Contains("buildingname") ? dset.Tables[0].Rows[0]["buildingname"] : "",
                    housename = dset.Tables[0].Columns.Contains("housename") ? dset.Tables[0].Rows[0]["housename"] : ""
                };
                return collmat.ToJson();
            }
            else
            {
                var collmat = new { projectid = "0", buildingid = "0", houseid = "0" };
                return collmat.ToJson();
            }

        }
        /// <summary>
        ///  MCAS自动估价:楼盘
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="FXTCompanyId">评估机构ID</param>
        /// <param name="CompanyId">客户单位Id</param>
        /// <param name="UserId">账号</param>
        /// <param name="BuildingArea">建筑面积</param>
        /// <param name="StartDate">可为空</param>
        /// <param name="EndDate">可为空</param>
        /// <param name="sysTypeCode">系统code</param>
        /// <returns>Tables[0]:询价结果</returns>
        public static string GetMCASProjectAutoPrice(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);

            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            int fxtCompanyId = company.parentshowdatacompanyid;
            string useMonth = DateTime.Now.ToString("yyyy-MM") + "-01";
            int fxtCompanyIdLog = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));

            //added by: dpc, 2015-12-30
            //使用RPC调用数据
            //if (FxtClientService.IfUseRpc())
            //{
            //    var _projectId = funinfo.Value<string>("projectid");
            //    cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            //    fxtCompanyId = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid")) == 0 ? 25 : StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));

            //    var request = new PriceRequest()
            //    {
            //        SerachParam = new SearchParam()
            //        {
            //            PageIndex = 0,
            //            PageRecords = 15,
            //            CompanyId = fxtCompanyId,
            //            CityId = cityId,
            //            SysTypeCode = company.producttypecode,
            //            BEncryptId = true
            //        },
            //        ProjectId = _projectId,
            //    };

            //    FxtCommon.Openplatform.GrpcService.AutoPrice projectPrice;
            //    FxtClientService.GetProjectPriceVQ(request, out projectPrice);

            //    return projectPrice.ToLowerJson();

            //}


            //int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            //int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            //int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            //int type = StringHelper.TryGetInt(funinfo.Value<string>("type")) == 0 ? 0 : StringHelper.TryGetInt(funinfo.Value<string>("type"));
            //int systypecode = StringHelper.TryGetInt(funinfo.Value<string>("systypecode")) == 0 ? 1003001 : StringHelper.TryGetInt(funinfo.Value<string>("systypecode"));
            //string username = string.IsNullOrEmpty(funinfo.Value<string>("username")) ? "" : funinfo.Value<string>("username");
            //int companyid = StringHelper.TryGetInt(funinfo.Value<string>("companyid")) == 0 ? company.companyid : StringHelper.TryGetInt(funinfo.Value<string>("companyid"));
            //search.FxtCompanyId = company.companyid;
            //search.FxtCompanyId = search.FxtCompanyId == 365 || search.FxtCompanyId == 218 ? 25 : search.FxtCompanyId;
            //double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            //string StartDate = string.Empty;
            //string EndDate = string.Empty;

            #region 估价记录
            var log = new AutoPriceLog();
            log.AddTime = DateTime.Now;
            log.AutoType = 1;
            log.CityId = cityId;
            log.Estimable = -1;
            log.FxtCompanyId = fxtCompanyIdLog;
            log.ProductTypeCode = company.producttypecode;
            log.ProjectId = projectId;
            #endregion

            CAS.Entity.AutoPrice autoPrice = new CAS.Entity.AutoPrice();//自动估价结果
            DataSet ds = DATProjectAvgPriceBL.GetMCASProjectAutoPrice(cityId, projectId, fxtCompanyId, useMonth);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region 新的
                autoPrice.unitprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//楼盘均价
                autoPrice.caseavg = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//
                autoPrice.avgprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//
                autoPrice.plprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["LowLayerPrice"].ToString());//
                autoPrice.pmprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["MultiLayerPrice"].ToString());//;
                autoPrice.psprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["SmallHighLayerPrice"].ToString());//;
                autoPrice.phprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["HighLayerPrice"].ToString());//;
                autoPrice.psvprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["SingleVillaPrice"].ToString());//;
                autoPrice.ppvprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["PlatoonVillaPrice"].ToString());//;
                autoPrice.pmbhprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["MoveBackHousePrice"].ToString());//;
                autoPrice.casemax = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["CaseMinPrice"].ToString());//
                autoPrice.casemin = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["CaseMaxPrice"].ToString());//
                autoPrice.casecount = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["CaseCount"].ToString());//
                autoPrice.startdate = ds.Tables[0].Rows[0]["StartDate"].ToString();//
                autoPrice.enddate = ds.Tables[0].Rows[0]["EndDate"].ToString();//

                int IsJzHousePrice = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["IsJzHousePrice"].ToString());//
                if (autoPrice.unitprice <= 0)
                {
                    //不可估
                    autoPrice.estimable = 0;
                    autoPrice.unitprice = 0;
                    //LogHelper.Info("楼盘估价:不可估");
                    if (IsJzHousePrice == 1)
                    {
                        log.Estimable = -1;
                    }
                    else
                    {
                        log.Estimable = -2;
                    }
                }
                else
                {
                    //可估
                    autoPrice.estimable = 1;
                    //LogHelper.Info("楼盘估价:可估");
                    //LogHelper.Info("单价:" + autoPrice.unitprice);
                    //LogHelper.Info("底层基准均价:" + autoPrice.plprice);
                    //LogHelper.Info("多层基准均价:" + autoPrice.pmprice);
                    //LogHelper.Info("小高层基准均价:" + autoPrice.psprice);
                    //LogHelper.Info("高层基准均价:" + autoPrice.phprice);
                    //LogHelper.Info("案例最小数:" + autoPrice.casemax);
                    //LogHelper.Info("案例最大数:" + autoPrice.casemin);
                    //LogHelper.Info("案例数:" + autoPrice.casecount);
                    if (IsJzHousePrice == 1)
                    {
                        log.Estimable = 6;
                    }
                    else
                    {
                        log.Estimable = 7;
                    }
                }

                log.UnitPrice = autoPrice.unitprice;

                //if (IsJzHousePrice == 1 && autoPrice.unitprice <= 0)
                //{
                //    //是基准房价，并且单价小于等于0，不可估
                //    autoPrice.estimable = 0;
                //    autoPrice.unitprice = 0;
                //}
                //else if (IsJzHousePrice == 0 && (autoPrice.unitprice <= 0 || autoPrice.casecount < 5))
                //{
                //    //不是基准房价，并且单价小于等于0或者案例数小于5，不可估
                //    autoPrice.estimable = 0;
                //    autoPrice.unitprice = 0;
                //}
                //else if (IsJzHousePrice == 1 && autoPrice.unitprice > 0)
                //{
                //    //是基准房价，并且单价大于0，可估
                //    autoPrice.estimable = 1;
                //}
                //else if (IsJzHousePrice == 0 && (autoPrice.unitprice > 0 || autoPrice.casecount >= 5))
                //{
                //    //不是基准房价，并且单价大于0或者案例数大于等于5，可估
                //    autoPrice.estimable = 1;
                //}

                #endregion

            }

            AutoPriceLogBL.Add(log);
            return autoPrice.ToJson();
        }
        /// <summary>
        /// MCAS自动估价:楼栋，房号
        /// </summary>
        /// <param name="CityId">城市ID</param>
        /// <param name="ProjectId">楼盘ID</param>
        /// <param name="BuildingId">楼栋ID</param>
        /// <param name="HouseId">房号ID</param>
        /// <param name="FXTCompanyId">公司ID</param>
        /// <param name="totalFloor">总楼层</param>
        /// <param name="FloorNumber">所在楼层</param>
        /// <param name="Frontcode">朝向</param>
        /// <param name="projectprice">楼盘建筑类型均价</param>
        /// <param name="buildarea">面积</param>
        /// <returns></returns>
        public static string GetMCASBHAutoPrice(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            search.FxtCompanyId = company.parentshowdatacompanyid;
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            double projectprice = StringHelper.TryGetDouble(funinfo.Value<string>("projectprice"));//楼盘建筑类型均价
            int floorNumber = StringHelper.TryGetInt(funinfo.Value<string>("floornumber"));//所在楼层
            int totalFloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));//总楼层
            int frontCode = StringHelper.TryGetInt(funinfo.Value<string>("frontCode"));//朝向
            string StartDate = string.Empty;
            string EndDate = string.Empty;






            CAS.Entity.AutoPrice autoPrice = new CAS.Entity.AutoPrice();//自动估价结果
            DataSet ds = DATProjectAvgPriceBL.GetMCASBHAutoPrice(search.CityId, projectId, buildingId, houseId, search.FxtCompanyId, totalFloor,
                floorNumber, frontCode, projectprice, buildingArea);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //物业全称
                autoPrice.unitprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["UnitPrice"].ToString());
                autoPrice.totalprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["TotalPrice"].ToString());
            }
            return autoPrice.ToJson();
        }

        //获取行政区价格监测 kujj 20150421
        public static string GetProcAreaAvgList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            DateTime avgPriceDate = StringHelper.TryGetDateTime(funinfo.Value<string>("avgpricedate"));
            string avgPriceDatestr = avgPriceDate.ToString("yyyyMM");
            DataSet ds = DATProjectAvgPriceBL.GetProcAreaAvgList(search.FxtCompanyId, search.CityId, avgPriceDatestr);
            return (ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson());
        }

        //获取片区价格监测 kujj 20150422
        public static string GetProcSubAreaAvgList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int areaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            DateTime avgPriceDate = StringHelper.TryGetDateTime(funinfo.Value<string>("avgpricedate"));
            string avgPriceDatestr = avgPriceDate.ToString("yyyyMM");
            DataSet ds = DATProjectAvgPriceBL.GetProcSubAreaAvgList(search.FxtCompanyId, search.CityId, areaId, avgPriceDatestr);
            return (ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson());
        }

        //获取楼盘价格监测 kujj 20150422
        public static string GetProcProjectAvgList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int areaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int subAreaId = StringHelper.TryGetInt(funinfo.Value<string>("subareaid"));
            DateTime avgPriceDate = StringHelper.TryGetDateTime(funinfo.Value<string>("avgpricedate"));
            string avgPriceDatestr = avgPriceDate.ToString("yyyyMM");
            DataSet ds = DATProjectAvgPriceBL.GetProcProjectAvgList(search.FxtCompanyId, search.CityId, areaId, subAreaId, avgPriceDatestr);
            return (ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson());
        }

        //获取行政区、片区近一年均价 kujj 20150507
        public static string GetAreaYearAvgList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int areaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int subAreaId = StringHelper.TryGetInt(funinfo.Value<string>("subareaid"));
            DataSet ds = DATProjectAvgPriceBL.GetAreaYearAvgList(search.FxtCompanyId, search.CityId, areaId, subAreaId);
            return (ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson());
        }

        //获取行政区、片区、楼盘近一年走势 kujj 20150507
        public static string GetDiffTypeAvgList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int type = StringHelper.TryGetInt(funinfo.Value<string>("type"));
            string begin = funinfo.Value<string>("begin");
            string end = funinfo.Value<string>("end");
            int areaid = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int subareaid = StringHelper.TryGetInt(funinfo.Value<string>("subareaid"));
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingareatype = StringHelper.TryGetInt(funinfo.Value<string>("buildingareatype"));
            int buildingdatetype = StringHelper.TryGetInt(funinfo.Value<string>("buildingdatetype"));
            int buildingtypecode = StringHelper.TryGetInt(funinfo.Value<string>("buildingtypecode"));
            int housetype = StringHelper.TryGetInt(funinfo.Value<string>("housetype"));
            int purposetype = StringHelper.TryGetInt(funinfo.Value<string>("purposetype"));
            int e_housetype = StringHelper.TryGetInt(funinfo.Value<string>("ehousetype"));
            DataSet ds = DATProjectAvgPriceBL.GetDiffTypeAvgList(type, search.FxtCompanyId, search.CityId, areaid, subareaid, projectid, buildingareatype, buildingdatetype, buildingtypecode, housetype, purposetype, e_housetype, begin, end);
            return (ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson());
        }

        //获取楼盘建筑类型均价 kujj 20150612
        public static string GetMCASWeightProjectPrice(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.FxtCompanyId = company.parentshowdatacompanyid;
            DateTime beginDate = StringHelper.TryGetDateTime(funinfo.Value<string>("begindate"));
            DateTime endDate = StringHelper.TryGetDateTime(funinfo.Value<string>("enddate"));
            CAS.Entity.AutoPrice autoPrice = new CAS.Entity.AutoPrice();
            DataSet ds = DATProjectAvgPriceBL.GetMCASWeightProjectPrice(search.CityId, search.FxtCompanyId, projectId, beginDate, endDate);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                autoPrice.avgprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());
                autoPrice.plprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["LowLayerPrice"].ToString());
                autoPrice.pmprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["MultiLayerPrice"].ToString());
                autoPrice.psprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["SmallHighLayerPrice"].ToString());
                autoPrice.phprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["HighLayerPrice"].ToString());
                autoPrice.psvprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["SingleVillaPrice"].ToString());
                autoPrice.ppvprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["PlatoonVillaPrice"].ToString());
                autoPrice.pmbhprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["MoveBackHousePrice"].ToString());
            }
            return autoPrice.ToJson();
        }

        public static string GetProjectPriceRank(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            string cityname = funinfo.Value<string>("cityname");
            string areaname = funinfo.Value<string>("areaname");
            string projectname = funinfo.Value<string>("projectname");
            int FxtCompanyId = company.parentshowdatacompanyid;
            int systypecode = company.parentproducttypecode;

            DataSet ds = DATProjectAvgPriceBL.GetProjectPriceRank(cityname, areaname, projectname, FxtCompanyId, systypecode);
            return ds == null || ds.Tables.Count <= 0 ? "" : new
            {
                cityid = ds.Tables[0].Rows[0]["cityid"],
                cityname = ds.Tables[0].Rows[0]["cityname"],
                areaid = ds.Tables[0].Rows[0]["areaid"],
                areaname = ds.Tables[0].Rows[0]["areaname"],
                projectid = ds.Tables[0].Rows[0]["projectid"],
                projectname = ds.Tables[0].Rows[0]["projectname"],
                projectavgprice = ds.Tables[0].Rows[0]["projectavgprice"],
                projectrank = ds.Tables[0].Rows[0]["projectrank"],
                projectrankper = ds.Tables[0].Rows[0]["projectrankper"],
                areaavgprice = ds.Tables[0].Rows[0]["areaavgprice"],
                address = ds.Tables[0].Rows[0]["address"]
            }.ToJson();
        }
    }
}
