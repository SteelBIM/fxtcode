using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Actualize
{
    /// <summary>
    /// 通过代理方法名查找实际方法
    /// 为了防止代理方法名冲突，key存放在常量里
    /// </summary>
    public class MethodDictionary
    {
        private const string splist = "splist";//获取楼盘列表 所属：ProjectController
        private const string pev = "pev";//获取楼盘详细信息 所属：ProjectController
        private const string pdbp = "pdbp";//自动估价 所属：PriceController
        private const string cmbautoprice = "cmbautoprice";//银行自动估价 
        private const string queryhistorylist = "queryhistorylist";//自动估价记录 所属：PriceController
        private const string casautoprice = "casautoprice";//自动估价，将不往数据中心插入自动估价记录 所属：PriceController

        private const string garealist = "garealist";//获取区域列表 所属:CityAreaController
        private const string gscodelist = "gscodelist";//根据编号获取syscode 所属:SysCodeController

        private const string plist = "plist";//获取楼盘列表
        private const string projectdropdownlist = "projectdropdownlist";//获取楼盘下拉框
        private const string projectdetail = "queryprojectdetail";//获取楼盘详细数据 
        private const string projectphoto = "queryprojectphoto";//获取楼盘照片
        private const string projectcase = "queryprojectcase";//获取楼盘案例 GetProjectCase

        private const string autoprojectdetails = "autoprojectdetails";//获取自动估价楼盘详细信息 

        private const string buildinglist = "querybuildinglist";//获取楼栋列表
        private const string buildingbaseinfolist = "querybuildingbaseinfolist";//获取楼栋下拉列表
        private const string autobuildinginfolist = "queryautobuildinginfolist";//获取楼栋下拉列表

        private const string houseunitlist= "queryhouseunitlist";//获取单元列表
        private const string housefloorlist = "queryhousefloorlist";//获取楼层列表

        private const string housedropdownlist = "queryhousedropdownlist";//获取房号下拉列表
        private const string autohouselistlist = "queryautohouselist";//获取房号列表

        private const string datavgpricemonthlist = "avgpricemonth";//价格走势

        private const string caselist = "caselist";//获取案例 
        private const string caselistbywhere = "caselistbywhere";//获取案例 
        private const string casecount = "casecount";//案例条数

        private const string avgprice = "avgprice";//获取楼盘建筑类型及面积段分类均价信息
        private const string avgpricetrend = "avgpricetrend";//均价走势图
        private const string sameproject = "sameproject";//周边同质楼盘均价
        private const string otherchannel = "otherchannel";//不同渠道楼盘均价对比
        private const string mapprice = "mapprice";//地图价格、环比跌涨幅

        private const string addcase = "addcase";//新增案例
        private const string importprojectdata = "importprojectdata";//从采集系统导入楼盘信息到数据中心
        private const string addprojectphoto = "addprojectphoto";//给楼盘上传照片
        private const string collateralreassessment = "collateralreassessment";//获取押品复估价格

        private const string provincelist = "provincelist";//获取省份列表 
        private const string citylist = "citylist";//获取城市列表 
        private const string matchingdata = "matchingdata";//标准化楼盘楼栋房号匹配


        /// <summary>
        /// 存放字典
        /// </summary>
        public static readonly Dictionary<string, string> MethodDic = new Dictionary<string, string>
            {
                //方法注释 QueryFromNoInternal  
                { splist, "GetSearchProjectListByKey" },
                { pev, "GetProjectDetailsByProjectid" },
                { pdbp, "GetProjectEValue" },
                {garealist,"GetSYSAreaList"},
                {gscodelist,"GetSYSCodeList"},
                {buildinglist,"GetBuildingListByPid"},
                {buildingbaseinfolist,"GetBuildingBaseInfoList"},
                {autobuildinginfolist,"GetAutoBuildingInfoList"},
                {houseunitlist,"GetHouseUnitList"},
                {housefloorlist,"GetHouseNoList"},
                {housedropdownlist,"GetHouseDropDownList"},
                {autohouselistlist,"GetAutoHouseList"},
                {projectdetail,"GetProjectInfoById"},
                {projectphoto,"GetProjectPhotoById"},
                {datavgpricemonthlist,"GetDATAvgPriceMonthList"},
                {projectcase,"GetProjectCase"},
                {autoprojectdetails,"GetProjectSimpleDetails"},
                {projectdropdownlist,"GetProjectDropDownList"},
                {plist,"GetProjectList"},
                {caselist,"GetCaseList"},
                {caselistbywhere,"GetCaseListByCaseIds"},
                {casecount,"GetProjectCaseCount"},
                {cmbautoprice,"GetEValueByProjectIdWithCmb"},
                {avgprice,"GetProjectAvgPriceList"},
                {avgpricetrend,"GetCityAreaAvgPriceTrend"},
                {sameproject,"GetSameProjectCasePrice"},
                {otherchannel,"GetOtherChannelCasePrice"},
                {mapprice,"GetMapPrice"},
                {addcase,"AddCaseInfo"},
                {queryhistorylist,"GetDATQueryHistoryList"},
                {importprojectdata,"ImportProjectAndBuildingAndHouse"},
                {addprojectphoto,"AddProjectPhoto"},
                {collateralreassessment,"GetCollateralReassessment"},
                {casautoprice,"GetCASEValueByPId"},
                {provincelist,"GetProvinceList"},
                {citylist,"GetSYSCityList"},
                {matchingdata,"GetMatchingData"}
            };
    }
}
