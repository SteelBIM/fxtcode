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
        private const string gsubarealist = "gsubarealist";//获取片区列表 
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

        private const string houseunitlist = "queryhouseunitlist";//获取单元列表
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
        private const string mcasbhautoprice = "mcasbhautoprice";//MCAS自动估价：楼栋，房号
        private const string mcasprojectprice = "mcasprojectprice";//MCAS自动估价：楼盘
        private const string projecttypeprice = "projecttypeprice";//楼盘细分类型均价 

        private const string gsubarealistbiz = "gsubarealistbiz";//获取商业片区列表    
        private const string gsubarealistoffice = "gsubarealistoffice";//获取办公片区列表
        private const string gcaselistbiz = "gcaselistbiz";//获取商业案例列表
        private const string gcaselistoffice = "gcaselistoffice";//获取办公案例列表 
        private const string gcaselistbizmcas = "gcaselistbizmcas";//获取商业案例列表forMCAS
        private const string gcaselistofficemcas = "gcaselistofficemcas";//获取办公案例列表forMCAS 
        private const string gcaseinfobizmcas = "gcaseinfobizmcas";//获取商业案例详情forMCAS

        private const string gprojectlistinfomcas = "gprojectlistinfomcas";//MCAS传入[projectid1,cityid1],[projectid2,cityid2],,,数据中心返回各个楼盘的坐标及照片总数（20150209）
        private const string gcasetypecountmcas = "gcasetypecountmcas";//获取单个楼盘近1、3、6个月案例总数 库晶晶20150210
        private const string gcasetypecountsmcas = "gcasetypecountsmcas";//获取多个楼盘近n个月案例总数 tanql20150922

        private const string gsyl = "gsyl";//获取内部收益率数据

        private const string addcompanydata = "addcompanydata"; //添加查看公司权限数据
        private const string updatecompanydata = "updatecompanydata"; //修改查看公司权限数据
        private const string getcompanydata = "getcompanydata";//查询公司权限数据

        private const string projectdropdownlistmcas = "projectdropdownlistmcas";//获取楼盘下拉框forMCAS
        private const string projectdropdownlistmcassdk = "projectdropdownlistmcassdk";//获取楼盘简易下拉列表MCAS(招行sdk专用,只返回楼盘表数据)  zhoub 20161102
        private const string buildingbaseinfolistmcas = "buildingbaseinfolistmcas";//获取楼栋下拉列表forMCAS
        private const string housefloorlistmcas = "housefloorlistmcas";//获取楼层列表forMCAS
        private const string housedropdownlistmcas = "housedropdownlistmcas";//获取房号下拉列表forMCAS
        private const string projectcasemcas = "projectcasemcas";//获取住宅案例列表forMCAS
        private const string projectphotomcas = "projectphotomcas";//获取住宅图片列表forMCAS

        private const string gcaseinfobiz = "gcaseinfobiz";//获取商业案例详情
        private const string gcaseinfooffice = "gcaseinfooffice";//获取办公案例详情
        private const string gcaselistland = "gcaselistland";//获取土地案例列表
        private const string gcaseinfoland = "gcaseinfoland";//获取土地案例详情
        private const string gcaseinfo = "gcaseinfo";//获取住宅案例详情
        private const string gcaselistindustry = "gcaselistindustry";//获取工业案例列表
        private const string gcaseinfoindustry = "gcaseinfoindustry";//获取工业案例详情

        private const string isallowcpm = "isallowcpm";//判断是否开通产品模块权限
        private const string gapricelist = "gapricelist";//城市区域均价、环比、同比
        private const string gcaselistnew = "gcaselistnew";//获取住宅案例列表
        private const string gcaseprice = "gcaseprice";//获取住宅案例最高单价、最低单价、平均单价
        private const string gprocareaavglist = "gprocareaavglist";//获取行政区价格监测
        private const string gprocsubareaavglist = "gprocsubareaavglist";//获取片区价格监测
        private const string gprocprojectavglist = "gprocprojectavglist";//获取楼盘价格监测
        private const string avgpricemonthnf = "avgpricemonthnf";//价格走势，无flash版
        private const string buildingbaseinfolistlist = "buildingbaseinfolistlist";//楼栋列表forMCAS
        private const string areayearavglist = "areayearavglist";//dat_sample表获取行政区、片区近一年价格
        private const string difftypeavglist = "difftypeavglist";//dat_sample表获取行政区、片区、楼盘近一年走势
        private const string gpappendagelist = "gpappendagelist";//获取项目配套
        private const string gmcaswpp = "gmcaswpp";//获取楼盘建筑类型均价formcas
        private const string gmcaspsh = "gmcaspsh";//获取楼盘附属房屋信息forMCAS

        private const string inquiry = "inquiry";//询价单formcas
        private const string psubhouselist = "psubhouselist";//获取楼盘附属房屋价格列表formcas
        private const string fitmentpricelist = "fitmentpricelist";//获取装修单价列表formcas
        private const string gpdinfo = "gpdinfo";//获取楼盘详细信息(包含codeName)formcas
        private const string gbdinfo = "gbdinfo";//获取楼栋详细信息formcas
        private const string ghdinfo = "ghdinfo";//获取房号详细信息formcas
        private const string cityzcodeList = "cityzcodeList";//获取城市列表，根据省份zipcode
        private const string getcityinfo = "getcityinfo";//获取城市信息
        private const string getareainfo = "getareainfo";//获取行政区信息

        //private const string getcitycasemonth = "getcitycasemonth";//获取城市案例月份

        private const string ghnlfvq = "ghnlfvq";//获取楼层列表forVQ
        private const string ghlfvq = "ghlfvq";//获取房号列表forVQhousedropdownlistmcas
        private const string gblfvq = "gblfvq";//获取楼栋列表forVQ
        private const string gpafvq = "gpafvq";//楼盘自动估价forVQ
        private const string gplfvq = "gplfvq";//获取楼盘列表forVQ
        private const string ghafvq = "ghafvq";//房号自动估价forVQ
        private const string gpafvqe = "gpafvqe";//楼盘自动估价forVQ（传入参数不加密）
        private const string ghafvqe = "ghafvqe";//房号自动估价forVQ（传入参数不加密）
        private const string gphafvqe = "gphafvqe";//楼盘、房号自动估价forVQ（传入参数不加密）
        private const string gphnafvqe = "gphnafvqe";//楼盘名称、房号名称自动估价forVQ（传入参数不加密）

        private const string getprojectbuildinghouselist = "getprojectbuildinghouselist";//获取楼盘楼栋房号列表（踩盘）
        private const string getsysroleuserids = "getsysroleuserids";//获取数据中心用户角色的城市权限（踩盘）
        private const string buildinghousetotal = "buildinghousetotal";//根据楼盘ID获取楼栋、房号数量（踩盘）

        private const string addprivicompanyvq = "addprivicompanyvq";//新增公司forVQ
        private const string ghnlfout = "ghnlfout";//获取楼层列表forOUT

        private const string splistforcasout = "splistforcasout";//获取楼盘列表ForCAS_OUT
        private const string buildinglistforcasout = "buildinglistforcasout";//获取楼栋下拉列表ForCAS_OUT
        private const string housefloorlistforcasout = "housefloorlistforcasout";//获取楼层列表ForCAS_OUT
        private const string houselistforcasout = "houselistforcasout";//获取房号列表ForCAS_OUT
        private const string casautopriceforcasout = "casautopriceforcasout";//自动估价，不往数据中心插入自动估价记录ForCAS_OUT

        private const string collateralreassessmentforvq = "collateralreassessmentforvq";//vq押品复估
        private const string citylistbycompany = "citylistbycompany";//获取公司开通的城市列表，vqapi
        private const string projectcountbycityid = "projectcountbycityid";//获取楼盘数量，民生银行

        private const string addexecutetimelog = "addexecutetimelog";//日志
        private const string addexecutetimecountlog = "addexecutetimecountlog";//日志

        private const string projectlistforreautoprice = "projectlistforreautoprice";//获取楼盘列表for复估

        private const string projectlistforzipcode = "projectlistforzipcode";//浦发银行获取楼盘列表
        private const string caselistbyareaanduse = "caselistbyareaanduse";//浦发银行获取案例列表

        private const string tempfinancetransfer = "tempfinancetransfer";//招行二期中间表
        private const string projectoperatedrecord = "projectoperatedrecord";//楼盘信息更新查询接口
        private const string buildingoperatedrecord = "buildingoperatedrecord";//楼栋信息更新查询接口
        private const string houseoperatedrecord = "houseoperatedrecord";//房号信息更新查询接口

        private const string glistbiz = "glistbiz";//获取商业街列表
        private const string gbuildinglistbiz = "gbuildinglistbiz";//获取商业楼栋列表
        private const string gfloorlistbiz = "gfloorlistbiz";//获取商业楼层列表
        private const string ghouselistbiz = "ghouselistbiz";//获取商业房号列表

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
                {gsubarealist,"GetSubAreaList"},
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
                {mcasbhautoprice,"GetMCASBHAutoPrice"},
                {mcasprojectprice,"GetMCASProjectAutoPrice"},
                {matchingdata,"GetMatchingData"},
                {projecttypeprice,"GetProjectTypePrice"},
                {gsubarealistbiz,"GetSubAreaListBiz"},
                {gsubarealistoffice,"GetSubAreaListOffice"},
                {gcaselistbiz,"GetCaseListBiz"},
                {gcaselistoffice,"GetCaseListOffice"},
                {gcaselistbizmcas,"GetCaseListBiz_MCAS"},
                {gcaselistofficemcas,"GetCaseListOffice_MCAS"},
                {gcaseinfobizmcas,"GetCaseInfoBiz_MCAS"},
                {gprojectlistinfomcas,"GetProjectListInfo_MCAS"},   //20150209新增
                {gcasetypecountmcas,"GetCaseCountByProjectId_MCAS"},   //20150209新增
                {gcasetypecountsmcas,"GetCaseCountByProjectIds_MCAS"},   //20150922新增
                {gsyl,"GetSYLList"},   //20150227新增
                {addcompanydata,"AddCompanyShowData"},
                {updatecompanydata,"UpdateCompanyShowData"},
                {getcompanydata,"GetCompanyShowData"},
                {projectdropdownlistmcas,"GetProjectDropDownList_MCAS"}, //20150313
                {projectdropdownlistmcassdk,"GetProjectDropDownList_MCAS_SDK"}, //20161102
                {buildingbaseinfolistmcas,"GetBuildingBaseInfoList_MCAS"}, //20150316
                {housefloorlistmcas,"GetHouseNoList_MCAS"}, //20150316
                {housedropdownlistmcas,"GetHouseDropDownList_MCAS"}, //20150316
                {projectcasemcas,"GetProjectCase_MCAS"}, //20150316
                {projectphotomcas,"GetProjectPhotoById_MCAS"}, //20150316
                {gcaseinfobiz,"GetCaseInfoBiz"}, //20150319
                {gcaseinfooffice,"GetCaseInfoOffice"}, //20150319
                {gcaselistland,"GetCaseListLand"}, //20150319
                {gcaseinfoland,"GetCaseInfoLand"}, //20150319
                {gcaseinfo,"GetCaseInfo"}, //20150320
                {gcaselistindustry,"GetCaseListIndustry"}, //20150319
                {gcaseinfoindustry,"GetCaseInfoIndustry"}, //20150319
                {isallowcpm,"IsAllowCompanyProductModule"}, //20150407
                {gapricelist,"GetAvgPriceList"}, //20150413
                {gcaselistnew,"GetCaseListNew"}, //20150415
                {gcaseprice,"GetCasePrice"}, //20150416
                {gprocareaavglist,"GetProcAreaAvgList"}, //20150421
                {gprocsubareaavglist,"GetProcSubAreaAvgList"}, //20150422
                {gprocprojectavglist,"GetProcProjectAvgList"}, //20150422
                {avgpricemonthnf,"GetDATAvgPriceMonthList_NoFlash"},//20150424
                {buildingbaseinfolistlist,"GetBuildingBaseInfoListList"},//20150424
                {areayearavglist,"GetAreaYearAvgList"},//20150507
                {difftypeavglist,"GetDiffTypeAvgList"},//20150507
                {gpappendagelist,"GetPAppendageByProjectId"},//20150521
                {gmcaswpp,"GetMCASWeightProjectPrice"},//20150612
                {gmcaspsh,"GetMCASProjectSubHouse"},//20150714
                {inquiry,"GetMCASInquiry_ForVQ"},//20150907
                {psubhouselist,"GetProjectSubHouse"},//20150908
                {fitmentpricelist,"GetFitmentPriceList"},//20150909
                {gbdinfo,"GetBuildingDetailInfo"},//20150911
                {ghdinfo,"GetHouseDetailInfo"},//20150911
                {gpdinfo,"GetProjectDetailInfo"},//20150911
                {cityzcodeList,"GetSYSCityListByPZipCode"},//20150911
                {getcityinfo,"GetCityInfo"},//20150929
                {getareainfo,"GetAreaInfo"},//20151026
                

                {ghnlfvq,"GetHouseNoList_MCAS_ForVQ"},//20151012
                {ghlfvq,"GetHouseDropDownList_MCAS_ForVQ"},//20151012
                {gblfvq,"GetBuildingBaseInfoList_MCAS_ForVQ"},//20151012
                {gpafvq,"GetMCASProjectAutoPrice_ForVQ"},//20151012
                {gplfvq,"GetProjectDropDownList_MCAS_ForVQ"},//20151012
                {ghafvq,"GetMCASHouseAutoPrice_ForVQ"},//20151012
                {gpafvqe,"GetMCASProjectAutoPriceExpress_ForVQ"},//20151020
                {ghafvqe,"GetMCASHouseAutoPriceExpress_ForVQ"},//20151020
                {gphafvqe,"GetMCASProjectHouseAutoPrice_ForVQ"},//20160408
                {gphnafvqe,"GetMCASProjectHouseNameAutoPrice_ForVQ"},//20160408
                
                {getprojectbuildinghouselist,"GetProjectBuildingHouseByProjectIds"},//20151112
                {getsysroleuserids,"GetSysRoleUserids"},//tanql20160107

                {addprivicompanyvq,"AddPriviCompanyForVQ"},//tanql20160303
                {buildinghousetotal,"GetProjectBuildingHouseTotal"},//tanql20160307
                {ghnlfout,"GetHouseNoList_MCAS_ForOUT"},//20160317 tanql

                {splistforcasout,"GetProjectListByKey_ForCAS_OUT"},//20160329 tanql
                {buildinglistforcasout,"GetBuildingList_ForCAS_OUT"},//20160329 tanql
                {housefloorlistforcasout,"GetHouseNoList_ForCAS_OUT"},//20160329 tanql
                {houselistforcasout,"GetHouseList_ForCAS_OUT"},//20160329 tanql
                {casautopriceforcasout,"GetCASEValueByPId_ForCAS_OUT"},//20160329 tanql

                {collateralreassessmentforvq,"GetCollateralReassessmentForVQ"},//20160418 tanql
                {citylistbycompany,"GetSYSCityListByCompany"},//20160621 tanql
                {projectcountbycityid,"GetProjectCountByCityId"},//20160622 tanql

                {addexecutetimelog,"AddExecuteTimeLog"},//20160812 tanql
                {addexecutetimecountlog,"AddExecuteTimeCountLog"},//20160812 tanql

                {projectlistforreautoprice,"ProjectListForReAutoPrice"},//20160906 tanql

                {projectlistforzipcode , "GetProjectList_SPDB"},//20161013 raocl
                {caselistbyareaanduse , "GetCaselist_SPDB"},//20161013 raocl

                {tempfinancetransfer , "AddTransfer"},//20161110 raocl
                {projectoperatedrecord , "GetProjectOperatedRecord"},//20161110 raocl
                {buildingoperatedrecord , "GetBuildingOperatedRecord"},//20161114 raocl
                {houseoperatedrecord , "GetHouseOperatedRecord"},//20161114 raocl

                {glistbiz,"GetListBiz"},//20161207 kings
                {gbuildinglistbiz,"GetBuildingListBiz"},//20161207 kings
                {gfloorlistbiz,"GetFloorListBiz"},//20161207 kings
                {ghouselistbiz,"GetHouseListBiz"}//20161207 kings
            };
    }
}
