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
            /// 获取楼盘案例总数
            /// </summary>
            public static string ProjectCaseCount = Common.GetSql("Project.ProjectCaseCount");  
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

            /// <summary>
            /// 根据类型获取code列表
            /// </summary>
            public const string SYSCodeListByDicType = @"select dictype,codetype,max(id) as id from dbo.sys_code with(nolock) group by dictype,codetype
                    having dictype>0";
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
            /// 获取Area信息
            /// </summary>
            public static string AreaList = Common.GetSql("CityArea.AreaList");
        }

        public class ProjectInfo 
        {
            public static string ProjectDetails = Common.GetSql("ProjectInfo.ProjectDetails");
        }
    }
}
