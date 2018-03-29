using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace FxtCenterService.DataAccess.SQL
{
    public class Common
    {
        /// <summary>
        /// 读取SQL语句，注意文件名和传入的参数大小写要匹配
        /// SQL文件必须修改为“嵌入的资源”
        /// 如果不用嵌入也可以使用文件方式读取，好处是应用程序不会重启，但明文存放可能引起容易被篡改等安全问题
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetSql(string sql)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string resourceName = "FxtCenterService.DataAccess.SQL." + sql + ".sql";
            string result = "";
            try
            {
                Stream stream = _assembly.GetManifestResourceStream(resourceName);
                StreamReader myread = new StreamReader(stream);
                result = myread.ReadToEnd();
                myread.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //转为小写，避免sql与前台json大小写不一致
            return result.ToLower();
        }
    }

    public class SQLName
    {
        public class Project
        {
            public static string ProjectInfoGetByName = Common.GetSql("Project.ProjectInfoGetByName");
            public static string DATProjectList = Common.GetSql("Project.DATProjectList");
            public static string ProjectDropDownList = Common.GetSql("Project.ProjectDropDownList");
            public static string BuildingBaseList = Common.GetSql("Project.BuildingBaseList");
            public static string DATBuildingList = Common.GetSql("Project.DATBuildingList");
            public static string FloorOrUnitDropDownList = Common.GetSql("Project.FloorOrUnitDropDownList");
            public static string HouseDropDownList = Common.GetSql("Project.HouseDropDownList");
            public static string GetDatCaseList = Common.GetSql("Project.getdatcaselist");
            public static string GetDatCaseListByCalculate = Common.GetSql("Project.getdatcaselistbycalculate");

            /// <summary>
            /// 根据楼栋名称获取楼栋信息
            /// </summary>
            public static string BuildingInfoGetByName = Common.GetSql("Project.BuildingInfoGetByName");
            /// <summary>
            /// 根据房号名称获取房号信息
            /// </summary>
            public static string HouseInfoGetByName = Common.GetSql("Project.HouseInfoGetByName");

            #region 特殊客户
            public static string GetDatCaseListForSpecial = Common.GetSql("Project.getdatcaselistForSpecial");
            public static string GetDatCaseListByCalculateForSpecial = Common.GetSql("Project.getdatcaselistbycalculateForSpecial");
            #endregion

            /// <summary>
            /// 楼盘信息
            /// </summary>
            public static string ProjectDetailInfo = Common.GetSql("Project.ProjectDetailInfo");
            /// <summary>
            /// 楼盘图片
            /// </summary>
            public static string ProjectPhoto = Common.GetSql("Project.ProjectPhoto");
            /// <summary>
            /// 楼盘价格走势
            /// </summary>
            public static string ProjectTrend = Common.GetSql("Project.ProjectTrend");
            /// <summary>
            /// 楼盘案例
            /// </summary>
            public static string ProjectCase = Common.GetSql("Project.ProjectCase");
            /// <summary>
            /// 楼盘周边案例
            /// </summary>
            public static string GetProjectAroundCase = Common.GetSql("Project.GetProjectAroundCase");
            /// <summary>
            /// 获取三个月内案例
            /// </summary>
            public static string GetBaseCaseList = Common.GetSql("Project.GetBaseCaseList");    
            /// <summary>
            /// 建筑类型及面积段分类均价表
            /// </summary>
            public static string ProjectAvgPrice = Common.GetSql("Project.ProjectAvgPrice");
            /// <summary>
            /// 楼盘细分类型均价走势
            /// </summary>
            public static string AvgPriceTrend = Common.GetSql("Project.AvgPriceTrend");
            /// <summary>
            /// 周边同质楼盘均价
            /// </summary>
            public static string SameProjectCasePrice = Common.GetSql("Project.SameProjectCasePrice");
            /// <summary>
            /// 不同渠道楼盘均价获取
            /// </summary>
            public static string OtherChannelCasePrice = Common.GetSql("Project.OtherChannelCasePrice");
            /// <summary>
            /// 周边楼盘价格、环比涨跌幅
            /// </summary>
            public static string GetMapPrice = Common.GetSql("Project.GetMapPrice");  
            /// <summary>
            /// 获取城市，行政区均价走势（不区分类型）
            /// </summary>
            public static string CityAreaAvgPriceTrend = Common.GetSql("Project.CityAreaAvgPriceTrend");
            /// <summary>
            /// 获取行政区均价（不区分类型）
            /// </summary>
            public static string AreaAvgPriceTrend = Common.GetSql("Project.AreaAvgPriceTrend");
            /// <summary>
            /// 获取楼盘案例总数
            /// </summary>
            public static string ProjectCaseCount = Common.GetSql("Project.ProjectCaseCount");

            /// <summary>
            /// 获取自动估价记录
            /// </summary>
            public static string GetQueryHistoryList = Common.GetSql("Project.GetQueryHistoryList");
            /// <summary>
            /// 根据楼盘ID和机构ID查询主表信息
            /// </summary>
            public static string ProjectParentInfoByProjIdAndComId = Common.GetSql("Project.ProjectParentInfoByProjIdAndComId");
            /// <summary>
            /// 根据楼盘ID和机构ID查询子表信息
            /// </summary>
            public static string ProjectSubInfoByProjIdAndComId = Common.GetSql("Project.ProjectSubInfoByProjIdAndComId");
            /// <summary>
            /// 根据楼盘ID查询配套信息
            /// </summary>
            public static string GetPAppendageByProjectId = Common.GetSql("Project.GetPAppendageByProjectId");
            /// <summary>
            /// 根据楼盘ID查询楼盘信息信息(关联子表)
            /// </summary>
            public static string ProjectInfoGetById = Common.GetSql("Project.ProjectInfoGetById");
            /// <summary>
            /// 根据楼盘ID+楼栋ID查询楼栋信息(关联子表)
            /// </summary>
            public static string BuildingInfoGetById = Common.GetSql("Project.BuildingInfoGetById");
            /// <summary>
            /// 根据楼栋ID+房号ID查询房号信息(关联子表)
            /// </summary>
            public static string HouseInfoGetById = Common.GetSql("Project.HouseInfoGetById");
            /// <summary>
            /// 根据多个楼盘名称搜索楼盘信息
            /// </summary>
            public static string ProjectInfoGetByNames = Common.GetSql("Project.ProjectInfoGetByNames");
            /// <summary>
            /// 根据多个楼盘名称搜索楼盘信息
            /// </summary>
            public static string GetMatchingData = Common.GetSql("Project.GetMatchingData");

            public static string GetCaseCountByProjectId_MCAS = Common.GetSql("Project.GetCaseCountByProjectIdMCAS");//20150210
            public static string GetCaseCountByProjectIds_MCAS = Common.GetSql("Project.GetCaseCountByProjectIdsMCAS");//20150922
            public static string GetProjectListInfo_MCAS = Common.GetSql("Project.GetProjectListInfoMCAS");//20150210
            
            //楼盘列表forMCAS
            public static string ProjectDropDownList_MCAS = Common.GetSql("Project.ProjectDropDownListMCAS");//20150313
            public static string ProjectDropDownList_MCAS_SDK = Common.GetSql("Project.ProjectDropDownListMCAS_SDK");//20161102 zhoub
            public static string Mariadb_ProjectDropDownList_MCAS = Common.GetSql("Project.Mariadb_ProjectDropDownListMCAS");   //20160825 zhoub
            //楼栋列表forMCAS
            public static string DATBuildingList_MCAS = Common.GetSql("Project.DATBuildingListMCAS");//20150316
            public static string Mariadb_DATBuildingList_MCAS = Common.GetSql("Project.Mariadb_DATBuildingListMCAS");//20160826 zhoub
            //房号列表forMCAS
            public static string HouseDropDownList_MCAS = Common.GetSql("Project.HouseDropDownListMCAS");//20150316
            public static string Mariadb_HouseDropDownList_MCAS = Common.GetSql("Project.Mariadb_HouseDropDownListMCAS");//20150316
            //楼盘案例forMCAS
            public static string ProjectCase_MCAS = Common.GetSql("Project.ProjectCase"); //20150316,sql可以共用。
            //楼盘图片forMCAS
            public static string ProjectPhoto_MCAS = Common.GetSql("Project.ProjectPhotoMCAS");//20150317
            //城市区域均价、环比、同比
            public static string AvgPriceList = Common.GetSql("Project.AvgPriceList");//20150413
            //获取住宅案例列表
            public static string GetCaseListNew = Common.GetSql("Project.GetCaseListNew");//20150415
            //获取住宅案例列表
            public static string AreaYearAvgList = Common.GetSql("Project.AreaYearAvgList");//20150507
            //获取楼盘附属房屋信息forMCAS
            public static string ProjectSubHouse_MCAS = Common.GetSql("Project.ProjectSubHouseMCAS");//20150714
            //获取单个房号、楼栋、楼盘
            public static string HouseBuildingProjectByHouseID = Common.GetSql("Project.HouseBuildingProjectByHouseID");//20150907
            //获取单个楼栋、楼盘
            public static string BuildingProjectByBuildingID = Common.GetSql("Project.BuildingProjectByBuildingID");//20150911
            //获取楼栋详细
            public static string BuildingDetailInfo = Common.GetSql("Project.BuildingDetailInfo");//20150911
            //获取楼栋详细列表
            public static string BuildingDetailInfoList = Common.GetSql("Project.BuildingDetailInfoList");//20160614
            //获取房号详细
            public static string HouseDetailInfo = Common.GetSql("Project.HouseDetailInfo");//20150911
            //获取楼栋详细(包含codeName)
            public static string ProjectDetailInfoContainCodeName = Common.GetSql("Project.ProjectDetailInfoContainCodeName");//20150911
            //根据楼盘ID获取楼栋、房号数
            public static string BuildingAndHouseTotalByProjectId = Common.GetSql("Project.BuildingAndHouseTotalByProjectId");//20160307
            //根据楼盘ID获取关联样本楼盘
            public static string RelProjectList = Common.GetSql("Project.RelProjectList");//20160418
            //根据楼盘ID获取关联样本楼盘
            public static string RelProjectListByLimit = Common.GetSql("Project.RelProjectListByLimit");//20161008
            //获取楼盘数量
            public static string ProjectCount = Common.GetSql("Project.ProjectCount");//20160622
            //楼盘列表
            public static string ProjectList = Common.GetSql("Project.ProjectList");//20160906
            //浦发楼盘列表
            public static string ProjectListForSPDB = Common.GetSql("SPDB.ProjectListSPDB");//20161013
            //浦发楼盘案例列表
            public static string ProjectCaseListForSPDB = Common.GetSql("SPDB.ProjectCaseListSPDB");//20161014
            //案例统计价格
            public static string GetCasePrice = Common.GetSql("Project.GetCasePrice");//20161020
            //楼盘修改记录
            public static string GetOperatedRecord = Common.GetSql("Project.GetOperatedRecord");
        }

        public class Configuration
        {
            public static string SYSCityTableList = Common.GetSql("Configuration.SYSCityTableList");
        }

        /// <summary>
        /// 自动估价
        /// </summary>
        public class AutoPrice
        {
            /// <summary>
            /// 获取楼盘列表
            /// </summary>
            public static string ProjectList = Common.GetSql("AutoPrice.ProjectList");
            /// <summary>
            /// 根据楼栋门牌获取楼盘列表
            /// </summary>
            public static string ProjectListSpecial = Common.GetSql("AutoPrice.ProjectListSpecial");
            /// <summary>
            /// 获取楼盘详细信息
            /// </summary>
            public static string ProjectDetail = Common.GetSql("AutoPrice.ProjectDetail");
            /// <summary>
            /// 获取楼栋列表
            /// </summary>
            public static string BuildingList = Common.GetSql("AutoPrice.BuildingList"); 

            /// <summary>
            /// 获取楼层列表
            /// </summary>
            public static string FloorNoList = Common.GetSql("AutoPrice.FloorNoList");
            /// <summary>
            /// 获取房号列表
            /// </summary>
            public static string HouseList = Common.GetSql("AutoPrice.HouseList");

            //获取楼层列表forMCAS
            public static string FloorNoList_MCAS = Common.GetSql("AutoPrice.FloorNoListMCAS"); //20150316
            public static string Mariadb_FloorNoList_MCAS = Common.GetSql("AutoPrice.Mariadb_FloorNoListMCAS");   //20160826 zhoub
            public static string WeightProjectPrice = Common.GetSql("AutoPrice.WeightProjectPrice");//20150612

            //获取楼层列表forOUT
            public static string FloorNoList_OUT = Common.GetSql("AutoPrice.FloorNoListOUT"); //20160317
        }

        /// <summary>
        /// CODE信息
        /// </summary>
        public class SysCode 
        { 
            /// <summary>
            /// 获取CODE信息
            /// </summary>
            public static string CodeList = Common.GetSql("Code.CodeList");

            /// <summary>
            /// 获取CODE影响价格的百分比
            /// </summary>
            public static string CodePriceList = Common.GetSql("Code.CodePriceList");

        }

        /// <summary>
        /// 城市、区域信息
        /// </summary>
        public class CityArea 
        {
            /// <summary>
            /// 获取Province信息
            /// </summary>
            public static string ProvinceList = Common.GetSql("CityArea.ProvinceList");
            /// <summary>
            /// 获取City信息
            /// </summary>
            public static string CityList = Common.GetSql("CityArea.CityList");
            /// <summary>
            /// 获取City信息
            /// </summary>
            public static string CityInfo = Common.GetSql("CityArea.CityInfo");
            /// <summary>
            /// 获取City信息（根据省份zipcode）
            /// </summary>
            public static string CityListByProvinceZipCode = Common.GetSql("CityArea.CityListByProvinceZipCode");
            /// <summary>
            /// 获取Area信息
            /// </summary>
            public static string AreaList = Common.GetSql("CityArea.AreaList");
            /// <summary>
            /// 获取SubArea信息
            /// </summary>
            public static string SubAreaList = Common.GetSql("CityArea.SubArea");
            /// <summary>
            /// 获取SubAreaBiz信息
            /// </summary>
            public static string SubAreaBizList = Common.GetSql("CityArea.SubAreaBiz");
            /// <summary>
            /// 获取SubAreaOffice信息
            /// </summary>
            public static string SubAreaOfficeList = Common.GetSql("CityArea.SubAreaOffice");
            /// <summary>
            /// 获取City设置获取案例月份数
            /// </summary>
            public static string CityCaseMonth = Common.GetSql("CityArea.GetCityCaseMonth");
            ///// <summary>
            ///// 获取公司开通的城市列表
            ///// </summary>
            //public static string CityListByCompany = Common.GetSql("CityArea.CityListByCompany");
        }

        public class ProjectInfo 
        {
            public static string ProjectDetails = Common.GetSql("ProjectInfo.ProjectDetails");
        }
        public class PriviCompany
        {
            public static string GetPriviCompanyByName = Common.GetSql("PriviCompany.GetPriviCompanyByName");
        }
        public class DCompany
        {
            public static string GetDATCompanyByName = Common.GetSql("DATCompany.GetDATCompanyByName");
        }
        public class PCompany
        {
            public static string GetPCompanyByCompanyIdAndProjectId = Common.GetSql("PCompany.GetPCompanyByCompanyIdAndProjectId");
            public static string GetPCompanyByProjectId = Common.GetSql("PCompany.GetPCompanyByProjectId");
        }
        public class BizOffice
        {
            public static string CaseBiz = Common.GetSql("BizOffice.CaseBiz");
            public static string CaseBiz_MCAS = Common.GetSql("BizOffice.CaseBiz"); //可以共用sql
            public static string CaseBizInfo = Common.GetSql("BizOffice.CaseBizInfo");
            public static string CaseOffice_MCAS = Common.GetSql("BizOffice.CaseOfficeMCAS");
            public static string CaseOfficeInfo = Common.GetSql("BizOffice.CaseOfficeInfo");
            public static string CaseLandInfo = Common.GetSql("BizOffice.CaseLandInfo");
            public static string CaseInfo = Common.GetSql("BizOffice.CaseInfo");
            public static string CaseIndustryInfo = Common.GetSql("BizOffice.CaseIndustryInfo");
        }
        public class ProjectBiz
        {
            public static string ProjectBizList = Common.GetSql("ProjectBiz.ProjectBizList");
            public static string BuildingBizList = Common.GetSql("ProjectBiz.BuildingBizList");
            public static string FloorBizList = Common.GetSql("ProjectBiz.FloorBizList");
            public static string HouseBizList = Common.GetSql("ProjectBiz.HouseBizList");
        }
        public class SYL
        {
            public static string GetSYLDat = Common.GetSql("SYL.GetSYLDat");
        }
        //public class Share
        //{
        //    public static string GetCompanyShowData = Common.GetSql("Share.GetCompanyShowData");
        //}
        //public class CompanyProductModule
        //{
        //    public static string GetCompanyProductModule = Common.GetSql("CompanyProductModule.GetCompanyProductModule");//2015-04-07判断是否开通产品模块权限
        //    public static string GetCompanyProductCity = Common.GetSql("CompanyProductModule.GetCompanyProductCity");//2015-11-26判断是否开通产品城市权限
        //}

        public class Sys
        {
            public static string GetRoleUser = Common.GetSql("Sys.GetRoleUser");

        }

        public class Building
        {
            public static string GetOperatedRecord = Common.GetSql("Building.GetOperatedRecord");
        }

        public class House
        {
            public static string GetOperatedRecord = Common.GetSql("House.GetOperatedRecord");
        }
    }
}
