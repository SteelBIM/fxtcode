using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;
using CAS.Entity.FxtProject;
using CAS.Entity;
using CAS.Entity.FxtLog;
using System.Diagnostics;

namespace FxtCenterService.Logic
{
    public class DatProjectBL
    {
        /// <summary>
        /// 房讯通的companyId
        /// </summary>
        public const int FXTCOMPANYID = 25;
        #region (照片类型Code(ID:2009))
        /// <summary>
        /// 照片类型-其他
        /// </summary>
        public const int PHOTOTYPECODE_9 = 2009009;
        #endregion

        /// <summary>
        /// 获取楼盘列表
        /// </summary>
        public static List<DATProject> GetDATProjectList(SearchBase search, string key, int areaid, int subareaid, int buildingtypecode, int purposecode)
        {
            return DatProjectDA.GetDATProjectList(search, key, areaid, subareaid, buildingtypecode, purposecode);
        }
        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        public static List<Dictionary<string, object>> GetProjectDropDownList(SearchBase search, string strKey, int items)
        {
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();

            DataTable dt = null;
            var key = "" + strKey + "%";
            var param = "%" + strKey + "%";
            search.Top = items;
            dt = DatProjectDA.GetProjectDropDownList(search, key, param);
            listResult = JSONHelper.DataTableToList(dt);
            //            if (listResult.Count < items)
            //            {
            //                condition = @" and (([PinYin] like @param) or ([Address] like @param) or ([ProjectName] like @param) or ([OtherName] like @param) or ([PinYinAll] like @param) or (PinYin like @strKey))
            // and [Address] not like @strKey and [PinYin] not like @strKey and [OtherName] not like @strKey and [PinYinAll] not like @strKey and [ProjectName] not like @strKey and PinYin not like @strKey";
            //                param = "%" + strKey + "%";
            //                search.Top = items - listResult.Count;
            //                dt = DatProjectDA.GetProjectDropDownList(search, condition, key, param);
            //                listResult.AddRange(JSONHelper.DataTableToList(dt));
            //            }

            return listResult;
        }

        /// <summary>
        /// 获取楼盘下拉列表forMCAS
        /// </summary>
        public static List<Dictionary<string, object>> GetProjectDropDownList_MCAS(SearchBase search, string strKey, string buildingName, int items, string serialno, int producttypecode, int companyid,int priceorderby = 0)
        {
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();

            DataTable dt = null;
            string condition = "", key, param;
            condition = " and [ProjectName] like @strKey";
            key = "" + strKey + "%";
            param = "%" + strKey + "%";
            search.Top = items;

            if (producttypecode == 1003038 && companyid == 6)
            {
                dt = DatProjectDA.GetProjectDropDownList_MCAS_Mariadb(search, condition, key, buildingName, param, serialno, priceorderby);
            }
            else
            {
                dt = DatProjectDA.GetProjectDropDownList_MCAS(search, condition, key, buildingName, param, serialno, priceorderby);
            }
            //DataTable dtCopy = dt.Clone();
            //#region 排序操作
            //DataView dv;
            //Func<DataTable, DataView> tableSort = o =>
            //{
            //    o.Columns.Add("projectsnameorder");
            //    foreach (DataRow item in o.Rows)
            //    {
            //        if (System.Text.RegularExpressions.Regex.IsMatch(item["projectname"].ToString(), "[一|二|三|四|五|六|七|八|九|十]"))
            //        {
            //            item["projectsnameorder"] = item["projectname"].ToString()
            //                .Replace("一", " 1")
            //                .Replace("二", " 2")
            //                .Replace("三", " 3")
            //                .Replace("四", " 4")
            //                .Replace("五", " 5")
            //                .Replace("六", " 6")
            //                .Replace("七", " 7")
            //                .Replace("八", " 8")
            //                .Replace("九", " 9")
            //                .Replace("十", " 10");
            //        }
            //        else
            //        {
            //            item["projectsnameorder"] = item["projectname"].ToString();
            //        }
            //    }
            //    return o.DefaultView;
            //};
            //if (!string.IsNullOrEmpty(strKey))
            //{
            //    dt.Columns.Add("projectsimilarity", typeof(string), "projectname like '" + strKey.Replace("*", "[*]").Replace("%","[%]") + "%'");
            //    dv = tableSort(dt);
            //    dv.Sort = "projectsimilarity desc,projectsnameorder asc";
            //}
            //else
            //{
            //    dv = tableSort(dt);
            //    dv.Sort = "projectsnameorder asc";
            //}
            //dtCopy = dv.ToTable();
            //dtCopy.Columns.Remove("projectsnameorder");
            //if (dtCopy.Columns.Contains("projectsimilarity"))
            //{
            //    dtCopy.Columns.Remove("projectsimilarity");
            //}
            //#endregion
            //listResult = JSONHelper.DataTableToList(dtCopy);
            listResult = JSONHelper.DataTableToList(dt);
            return listResult;
        }

        /// <summary>
        /// 获取楼盘下拉列表forMCAS
        /// </summary>
        public static List<DATProject> GetProjectDropDownList_MCAS2(SearchBase search, string strKey, string buildingName, int items, string serialno)
        {
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();

            string condition = "", key, param;
            condition = " and [ProjectName] like @strKey";
            key = "" + strKey + "%";
            param = "%" + strKey + "%";
            search.Top = items;

            return DatProjectDA.GetProjectDropDownList_MCAS2(search, condition, key, buildingName, param, serialno);
        }

        public static DATProject GetDATProjectByPK(int id)
        {
            return DatProjectDA.GetDATProjectByPK(id);
        }

        /// <summary>
        /// 获得数据中心的楼盘信息，没有联合附表
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoById(int cityid, int projectid, int fxtcompanyid, int typecode)
        {
            return DatProjectDA.GetProjectInfoById(cityid, projectid, fxtcompanyid, typecode);
        }

        /// <summary>
        /// 获得数据中心的楼盘图片，没有联合附表
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static List<LNKPPhoto> GetProjectPhotoById(int cityid, int projectid, int fxtcompanyid, int typecode)
        {
            return DatProjectDA.GetProjectPhotoById(cityid, projectid, fxtcompanyid, typecode);
        }

        /// <summary>
        /// 获得数据中心的楼盘图片，没有联合附表forMCAS
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static List<LNKPPhoto> GetProjectPhotoById_MCAS(int cityid, int projectid, int buildingid, int fxtcompanyid, int typecode)
        {
            return DatProjectDA.GetProjectPhotoById_MCAS(cityid, projectid, buildingid, fxtcompanyid, typecode);
        }
        /// <summary>
        /// 楼盘案例
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DataTable GetProjectCase(SearchBase search, int projectid, int fxtcompanyid, int cityid)
        {
            return DatProjectDA.GetProjectCase(search, projectid, fxtcompanyid, cityid);
        }

        /// <summary>
        /// 楼盘案例forMCAS
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DataTable GetProjectCase_MCAS(SearchBase search, int projectid, int fxtcompanyid, int cityid)
        {
            return DatProjectDA.GetProjectCase_MCAS(search, projectid, fxtcompanyid, cityid);
        }
        /// <summary>
        /// 获取自动估价楼盘信息-返回是否可估
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<object> GetSearchProjectListByKey(SearchBase search, int fxtcompanyid, int cityid, string key)
        {
            var autoproject = DatProjectDA.GetSearchProjectListByKey(search, fxtcompanyid, cityid, key).Select(o => new
            {
                projectid = o.projectid,
                projectname = o.projectname,
                isevalue = o.isevalue,
                recordcount = o.recordcount,
                address = o.address,
                x = o.x,
                y = o.y,
                areaid = o.areaid,
                weight = o.weight
            });

            return autoproject;
        }
        /// <summary>
        /// 获取自动估价楼盘信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<DATProject> GetTestSearchProjectListByKey(SearchBase search, int fxtcompanyid, int cityid, string key)
        {
            return DatProjectDA.GetSearchProjectListByKey(search, fxtcompanyid, cityid, key);
        }

        /// <summary>
        /// 获取自动估价楼盘详细信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetProjectDetailsByProjectid(SearchBase search, int fxtcompanyid, int cityid, int projectid)
        {
            DATProject modelProject = DatProjectDA.GetProjectDetailsByProjectid(search, fxtcompanyid, cityid, projectid);
            var project = new
            {
                areaid = modelProject.areaid,
                address = modelProject.address,
                casecnt = modelProject.casecnt,
                isevalue = modelProject.isevalue,
                enddate = modelProject.enddate,
            };
            return project.ToJson();
        }

        /// <summary>
        /// 楼盘详细：楼盘ID，楼盘名，区域名，是否可估，停车位，管理费，地址，区域id,竣工时间，开发商，物业管理  
        /// hody,暂为易房保
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetProjectInfoDetailsByProjectid(SearchBase search, int fxtcompanyid, int cityid, int projectid)
        {
            DATProject modelProject = DatProjectDA.GetProjectInfoDetailsByProjectid(search, fxtcompanyid, cityid, projectid);

            //LogHelper.Info(string.Format("{0} 楼盘详细:x:{1},y:{2}", modelProject.projectname, modelProject.x, modelProject.y));

            var project = new
            {
                projectid = modelProject.projectid,
                projectname = modelProject.projectname,
                areaname = modelProject.areaname,
                isevalue = modelProject.isevalue,
                parkingnumber = modelProject.parkingnumber,
                managerprice = modelProject.managerprice,
                address = modelProject.address,
                areaid = modelProject.areaid,
                enddate = modelProject.enddate,
                x = modelProject.x,
                y = modelProject.y,
                developcompanyname = modelProject.devecompanyname,
                managercompanyname = modelProject.managercompanyname,
                casecnt = modelProject.casecnt
            };
            return project.ToJson();
        }
        /// <summary>
        /// 根据楼盘名查询楼盘信息
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="projectname"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoByName(int cityId, int areaId, int fxtCompanyId, string projectname, string typecode)
        {
            return DatProjectDA.GetProjectInfoByName(cityId, areaId, fxtCompanyId, projectname, typecode);
        }
        /// <summary>
        /// 根据公司ID和楼盘ID在子表中查询数据
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DATProject GetProjectSubByProjectIdAndCompanyId(int fxtCompanyId, int cityId, int projectId)
        {
            return DatProjectDA.GetProjectSubByProjectIdAndCompanyId(fxtCompanyId, cityId, projectId);
        }
        /// <summary>
        /// 根据公司ID和楼盘ID在主表中查询数据
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static DATProject GetProjectParentByProjectIdAndCompanyId(int fxtCompanyId, int cityId, int projectId)
        {
            return DatProjectDA.GetProjectParentByProjectIdAndCompanyId(fxtCompanyId, cityId, projectId);
        }
        /// <summary>
        /// 新增楼盘信息到主表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int Add(DATProject model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            DATProject.SetTableName<DATProject>(_tableName);
            return DatProjectDA.Add(model);

        }
        /// <summary>
        /// 新增楼盘信息到子表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int AddSub(DATProject model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            _tableName = tableName + "_sub";
            DATProject.SetTableName<DATProject>(_tableName);
            return DatProjectDA.Add(model);
        }
        /// <summary>
        /// 修改楼盘信息到主表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int Update(DATProject model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            DATProject.SetTableName<DATProject>(_tableName);
            return DatProjectDA.Update(model);

        }
        /// <summary>
        /// 修改楼盘信息到子表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int UpdateSub(DATProject model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName + "_sub";
            DATProject.SetTableName<DATProject>(_tableName);
            return DatProjectDA.Update(model);

        }

        /// <summary>
        /// 新增照片
        /// 创建人:曾智磊,日期:2014-07-07
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddPhoto(LNKPPhoto model)
        {
            return DatProjectDA.AddPhoto(model);
        }
        /// <summary>
        /// 根据楼盘ID获取楼盘信息(关联子表)
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoByProjectId(int cityId, int fxtCompanyId, int projectId, int typecode)
        {
            return DatProjectDA.GetProjectInfoByProjectId(cityId, fxtCompanyId, projectId, typecode);
        }
        /// <summary>
        /// 根据多个楼盘名称，获取楼盘信息(关联子表)
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectnames"></param>
        /// <returns></returns>
        public static List<DATProject> GetProjectInfoByNames(int cityId, int areaId, int fxtCompanyId, string[] projectnames, int typecode)
        {
            return DatProjectDA.GetProjectInfoByNames(cityId, areaId, fxtCompanyId, projectnames, typecode);
        }

        /// <summary>
        /// 获取楼盘附属房屋信息forMCAS kujj20150714
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMCASProjectSubHouse(int projectid, long buildingid, long houseid, int cityid, int fxtcompanyid, int systypecode)
        {
            return DatProjectDA.GetMCASProjectSubHouse(projectid, buildingid, houseid, cityid, fxtcompanyid, systypecode);
        }

        /// <summary>
        /// 获取楼盘附属房屋价格 tanql20150908
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProjectSubHouse(int projectid, SearchBase search)
        {
            return DatProjectDA.GetProjectSubHouse(projectid, search);
        }

        /// <summary>
        /// 获取装修单价列表 tanql20150909
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFitmentPriceList(SearchBase search)
        {
            return DatProjectDA.GetFitmentPriceList(search);
        }
        /// <summary>
        /// 获取楼栋详细(包含codeName) tanql20150911
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static DataSet GetProjectDetailInfo(int projectId, SearchBase search)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            DataSet result = DatProjectDA.GetProjectDetailInfo(projectId, search);

            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            LogHelper.Info("楼栋下拉列表forMCAS数据库执行时间：" + ts2.TotalMilliseconds + "ms.");

            return result;
        }

        public static List<DATProject> GetProjectBuildingHouseByProjectIds(string projectIds, SearchBase search)
        {

            string[] pIds = projectIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //for (int i = 0; i < pIds.Length; i++)
            //{
            List<DATProject> projects = DatProjectDA.GetProjectDetail(pIds, search);
            //楼栋
            if (projects != null && projects.Count > 0)
            {
                foreach (var p in projects)
                {
                    List<DATBuilding> buildings = DatBuildingDA.GetBuildingDetailInfoList(p.projectid, search);
                    //if (buildings != null && buildings.Count > 0)
                    //{
                    //    foreach (var b in buildings)
                    //    {
                    //        //房号
                    //        List<DATHouse> houses = DatHouseDA.GetAutoHouseListList(search, b.buildingid, null, "");
                    //        b.houselist = houses;
                    //    }
                    //}
                    p.buildinglist = buildings;
                }
            }
            //    projects.Add(project);
            //}
            return projects;
        }

        public static DatProjectTotal GetProjectBuildingHouseTotal(int projectId, SearchBase search)
        {
            DatProjectTotal total = DatProjectDA.GetProjectBuildingHouseTotal(projectId, search);
            return total;
        }
        //押品复估
        public static string GetCollateralReassessmentForVQ(SearchBase search, int projectId, int buildingId, int houseId, int floorno,
           int totalfloor, int frontcode, string relstartdate, string relenddate, int distance, decimal x, decimal y, int fxtCompanyIdLog, int producttypecode, int showlog)
        {
            RelAutoPrice autoPrice = new RelAutoPrice();//自动估价结果
            decimal plprice = 0;//低层建筑类型基准价
            decimal pmprice = 0;//多层建筑类型基准价
            decimal psprice = 0;//小高层建筑类型基准价
            decimal phprice = 0;//高层建筑类型基准价
            int casecount = 0;
            decimal oldPrice = 0;

            #region 估价记录
            int estimable = 1;
            decimal unitPrice = 0;
            var log = new AutoPriceLog();//估价记录
            log.AddTime = DateTime.Now;
            log.AutoType = 4;
            log.CityId = search.CityId;
            log.Estimable = estimable;
            log.FxtCompanyId = fxtCompanyIdLog;
            log.ProductTypeCode = producttypecode;
            log.ProjectId = projectId;
            //log.BuildingArea = buildingarea;
            log.BuildingId = buildingId;
            log.HouseId = houseId;
            log.TotalFloor = totalfloor;
            log.FloorNo = floorno;
            log.FrontCode = frontcode;
            #endregion
            #region 获取楼盘基准价
            DataSet ds = DATProjectAvgPriceBL.GetMCASProjectAutoPrice(search.CityId, projectId, search.FxtCompanyId, "");
            if (showlog > 0)
            {
                LogHelper.Info("押品复估，楼盘估价");
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region 新的
                autoPrice.unitprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//楼盘均价
                plprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["LowLayerPrice"].ToString());//低层建筑均价
                pmprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["MultiLayerPrice"].ToString());//多层建筑均价
                psprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["SmallHighLayerPrice"].ToString());//小高层建筑均价
                phprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["HighLayerPrice"].ToString());//高层建筑均价
                casecount = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["CaseCount"].ToString());//案例数
                int IsJzHousePrice = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["IsJzHousePrice"].ToString());//0:不是基准房价、1是基准房价
                if (autoPrice.unitprice <= 0)
                {
                    //不可估
                    autoPrice.pricetype = 0;
                    autoPrice.unitprice = 0;
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
                    if (IsJzHousePrice == 1)
                    {
                        //可估
                        autoPrice.pricetype = 1;
                        log.Estimable = 6;
                        oldPrice = autoPrice.unitprice;
                    }
                    else
                    {
                        autoPrice.pricetype = 2;
                        log.Estimable = 7;
                        oldPrice = 0;
                    }
                }
                #endregion
            }
            #endregion
            if (autoPrice.pricetype > 0)//楼盘估价成功
            {
                //查总楼层
                if (totalfloor < 1 && buildingId > 0)
                {
                    DATBuilding building = DatBuildingBL.GetBuildingById(search.CityId, projectId, search.FxtCompanyId, buildingId);
                    if (building != null && building.totalfloor.HasValue)
                    {
                        totalfloor = building.totalfloor.Value;
                    }
                }
                var price = DATProjectAvgPriceBL.GetMCASHouseAutoPrice(search, projectId, buildingId, houseId, totalfloor,
                    floorno, frontcode, 0, Convert.ToInt32(autoPrice.unitprice),
                    Convert.ToInt32(plprice), Convert.ToInt32(pmprice), Convert.ToInt32(psprice), Convert.ToInt32(phprice), out estimable, out unitPrice);
                if (showlog > 0)
                {
                    LogHelper.Info("押品复估，房号估价");
                }
                var housePrice = JSONHelper.JSONToObject<RelAutoPrice>(price);
                if (housePrice.unitprice > 0)
                {
                    decimal pprice = 0;
                    if (totalfloor <= 3)//低层
                    {
                        pprice = plprice;
                    }
                    else if (totalfloor <= 8)//多层
                    {
                        pprice = pmprice;
                    }
                    else if (totalfloor <= 12)//小高层
                    {
                        pprice = psprice;
                    }
                    else if (totalfloor > 12)//高层
                    {
                        pprice = phprice;
                    }
                    if (pprice == 0)
                    {
                        pprice = oldPrice;
                    }
                    //LogHelper.Info("estimable=" + estimable);
                    //房号估价成功
                    if (estimable == 2)
                    {
                        if (pprice > 0)
                        {
                            autoPrice.pricetype = 4;
                        }
                        else
                        {
                            autoPrice.pricetype = 5;
                        }
                    }
                    else if (estimable == 3 && frontcode != 0)
                    {
                        if (pprice > 0)
                        {
                            autoPrice.pricetype = 8;
                        }
                        else
                        {
                            autoPrice.pricetype = 9;
                        }
                    }
                    else if (estimable == 3)
                    {
                        if (pprice > 0)
                        {
                            autoPrice.pricetype = 6;
                        }
                        else
                        {
                            autoPrice.pricetype = 7;
                        }
                    }
                    else if (estimable == 4)
                    {
                        if (pprice > 0)
                        {
                            autoPrice.pricetype = 4;
                        }
                        else
                        {
                            autoPrice.pricetype = 5;
                        }
                    }
                    else if (estimable == 5 && frontcode != 0)
                    {
                        if (pprice > 0)
                        {
                            autoPrice.pricetype = 8;
                        }
                        else
                        {
                            autoPrice.pricetype = 9;
                        }
                    }
                    else if (estimable == 5)
                    {
                        if (pprice > 0)
                        {
                            autoPrice.pricetype = 6;
                        }
                        else
                        {
                            autoPrice.pricetype = 7;
                        }
                    }
                    autoPrice.unitprice = housePrice.unitprice;
                    log.UnitPrice = housePrice.unitprice;
                    //autoPrice.pricetype = 3;
                }
                log.Estimable = estimable;
            }
            else
            {
                ////不可估，查样本楼盘
                //autoPrice.relprojects = DatProjectBL.GetRelProject(search, projectId, distance, x, y, relstartdate, relenddate, showlog);
                //if (autoPrice.relprojects.Count > 0)
                //{
                //    autoPrice.pricetype = 3;
                //}
            }
            AutoPriceLogBL.Add(log);
            if (showlog > 0)
            {
                LogHelper.Info("押品复估，成功");
            }
            return autoPrice.ToJson();
        }

        /// <summary>
        /// 样本楼盘列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectId"></param>
        /// <param name="distance">距离</param>
        /// <returns></returns>
        public static List<RelProject> GetRelProject(SearchBase search, int projectId, int distance, decimal x, decimal y, string useMonth1, string useMonth2, int showlog)
        {
            var relProjects = DatProjectDA.GetRelProject(search, projectId, distance, x, y);
            if (showlog > 0)
            {
                LogHelper.Info("获取样本楼盘");

            }
            relProjects.ForEach((o) =>
            {
                var dds = DATProjectAvgPriceDA.GetMCASProjectHistoryAutoPrice(o.CityID, o.ProjectId, useMonth1);
                if (showlog > 0)
                {
                    LogHelper.Info("押品复估，样本楼盘估价1");
                }
                if (dds != null && dds.Tables.Count > 0 && dds.Tables[0].Rows.Count > 0)
                {
                    o.ProjectAvgPrice1 = StringHelper.TryGetDecimal(dds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//楼盘均价
                    int caseCount = StringHelper.TryGetInt(dds.Tables[0].Rows[0]["CaseCount"].ToString());//
                    int IsJzHousePrice = StringHelper.TryGetInt(dds.Tables[0].Rows[0]["IsJzHousePrice"].ToString());//

                    if (o.ProjectAvgPrice1 <= 0)
                    {
                        //不可估
                        o.pricetype1 = 0;
                    }
                    else if (o.ProjectAvgPrice1 > 0)
                    {
                        //可估
                        if (IsJzHousePrice == 1)
                        {
                            o.pricetype1 = 1;
                        }
                        else
                        {
                            o.pricetype1 = 2;
                        }
                    }
                }
                else
                {
                    o.pricetype1 = 0;
                }
                var dds2 = DATProjectAvgPriceDA.GetMCASProjectHistoryAutoPrice(o.CityID, o.ProjectId, useMonth2);
                if (showlog > 0)
                {
                    LogHelper.Info("押品复估，样本楼盘估价2");
                }
                if (dds2 != null && dds2.Tables.Count > 0 && dds2.Tables[0].Rows.Count > 0)
                {
                    o.ProjectAvgPrice2 = StringHelper.TryGetDecimal(dds2.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//楼盘均价
                    int caseCount = StringHelper.TryGetInt(dds2.Tables[0].Rows[0]["CaseCount"].ToString());//
                    int IsJzHousePrice = StringHelper.TryGetInt(dds2.Tables[0].Rows[0]["IsJzHousePrice"].ToString());//
                    if (o.ProjectAvgPrice2 <= 0)
                    {
                        //不可估
                        o.pricetype2 = 0;
                    }
                    else if (o.ProjectAvgPrice2 > 0)
                    {
                        //可估
                        if (IsJzHousePrice == 1)
                        {
                            o.pricetype2 = 1;
                        }
                        else
                        {
                            o.pricetype2 = 2;
                        }
                    }
                }
                else
                {
                    o.pricetype2 = 0;
                }
            });
            relProjects.OrderByDescending(m => m.ProjectAvgPrice1).OrderByDescending(m => m.ProjectAvgPrice2).OrderByDescending(m => m.Distance);
            return relProjects;
        }

        /// <summary>
        /// 获取楼盘数量
        /// </summary>
        /// <returns></returns>
        public static int GetProjectCountByCityId(SearchBase search)
        {
            return DatProjectDA.GetProjectCountByCityId(search);
        }
    }
}
