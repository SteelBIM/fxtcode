using FxtDataAcquisition.BLL;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Text;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using System.IO;
using FxtDataAcquisition.Application.Interfaces;
using Ninject;
using FxtDataAcquisition.Framework.Ioc;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Web.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Domain.Models;
using System;
using FxtDataAcquisition.Common;
using System.Web;
using FxtDataAcquisition.Common.NPOI;
using System.Data;

namespace FxtDataAcquisition.Web.Controllers
{
    public class TestController : BaseController
    {
        public TestController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }

        public ActionResult AllotFlowTest(AllotFlow allotFlow)
        {
            //名称管理器
            //var excelHelper = new ExcelHandle("E:\\TempletForTV.xlsx");
            //DataTable dt = excelHelper.ExcelToDataTable("周(List)", true);
            allotFlow.Remark = "太神奇了2";
            //_unitOfWork.AllotFlowRepository.Update(allotFlow);
            //_unitOfWork.Commit();

            //fluent api 关系配置
            AllotFlow unitOfWork = _unitOfWork.AllotFlowRepository.GetById(4923);
            //AllotSurvey allotSurvey = _unitOfWork.AllotSurveyRepository.GetBy(m => m.AllotId == 4925);
            //var a = allotSurvey.AllotFlow;

            unitOfWork.Project.ProjectName = "楼盘啊32";
            unitOfWork.Remark = "金地翠园32";
            _unitOfWork.Commit();
            //Project project = _unitOfWork.ProjectRepository.GetById(4954);
            return Content("test");
            //return View();
        }

        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        /// <returns>请求结果</returns>
        public static string request(string url, string param)
        {
            string strURL = url + '?' + param;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "GET";
            // 添加header
            request.Headers.Add("apikey", "264a5ba3e9065323d2a030af536c5d70");
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }

        public ActionResult index2()
        {
            var f = _unitOfWork.AllotFlowRepository.Get().FirstOrDefault();
            var s = _unitOfWork.AllotSurveyRepository.Get().FirstOrDefault();
            return View();
        }
        public ActionResult index3()
        {
            return View();
        }

        public ActionResult AllotFlowInfo(string statuscode)
        {
            List<UserCenter_Apps> appList = new List<UserCenter_Apps>();
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser(out appList);
            int cityId = WebUserHelp.GetNowCityId();
            List<SYSCode> colist = DataCenterCodeApi.GetCodeById(1035, loginUserInfo.UserName, loginUserInfo.SignName, appList);
            List<FxtApi_SYSArea> areaList = SYSAreaManager.GetAreaByCityId(cityId, loginUserInfo.UserName, loginUserInfo.SignName, appList);
            ViewBag.AreaList = areaList;
            //状态code
            ViewBag.AllotStatus1 = SYSCodeManager.STATECODE_1;
            ViewBag.AllotStatus2 = SYSCodeManager.STATECODE_2;
            ViewBag.AllotStatus4 = SYSCodeManager.STATECODE_4;
            //功能code
            ViewBag.FunctionCode14 = SYSCodeManager.FunOperCode_14;
            ViewBag.FunctionCode15 = SYSCodeManager.FunOperCode_15;
            ViewBag.FunctionCode16 = SYSCodeManager.FunOperCode_16;
            //获取当前用户在此页面所有的操作权限
            List<int> functionCodes = WebUserHelp.GetNowPageFunctionCodes(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, WebCommon.Url_AllotFlowInfo_AllotFlowManager);
            ViewBag.FunctionCodes = functionCodes;
            if (!string.IsNullOrEmpty(statuscode) && statuscode != "0")
            {
                ViewBag.NowStatus = statuscode;
            }
            ViewBag.IomportAllotRight = 0;
            if (functionCodes.Contains(SYSCodeManager.FunOperCode_10))
            {
                ViewBag.IomportAllotRight = 1;
            }
            return View();
        }

        //
        // GET: /Test/

        public ActionResult Index()
        {
            //获取正式数据文件根目录
            string basePath2 = CommonUtility.GetConfigSetting("upload_DataAcquisition");
            string folder2 = System.Web.Hosting.HostingEnvironment.MapPath(basePath2);
            var path = folder2 + "/fff664ab41fa4abb9d4d_1_4118.zip";
            ZipHelper.UnZipFile(folder2, path);

            //var p = _unitOfWork.ProjectRepository.Insert(new Project() { ProjectName = "测试" ,PurposeCode=0,CityID = 6,AreaID = 72});

            //_unitOfWork.AllotFlowRepository.Insert(new AllotFlow() { DatId = p.ProjectId ,CityId = 6,FxtCompanyId = 25,DatType = 0,StateCode = 0,CreateTime = DateTime.Now});
            //_unitOfWork.Commit();

            //var a = _unitOfWork.AllotFlowRepository.Get().FirstOrDefault();
            //Project p = a.Project;
            //int d = 1;


            //GeocodingResponse response = GeocodingManager.Search(new GeocodingRequest()
            //{
            //    ak = "264a5ba3e9065323d2a030af536c5d70",
            //    output = OutputType.json,
            //    pois = 0,
            //    location = "22.55932000000000,114.04508000000000"
            //});

            //BaiduAPI.NearRequest r = new BaiduAPI.NearRequest();
            //r.apikey = "264a5ba3e9065323d2a030af536c5d70";
            //r.keyWord = "学校";
            //r.location = new BaiduAPI.Location() { lat = 116.305145m, lng = 39.982368m };
            //r.tag = BaiduAPI.TagType.全部;
            //r.radius = 30;
            //r.cityName = "北京";
            //r.sort_rule = 0;
            //r.number = 10;
            //r.page = 1;
            //r.output = BaiduAPI.OutputType.json;
            //r.coord_type = BaiduAPI.CoordType.bd09ll;
            //r.out_coord_type = BaiduAPI.CoordType.bd09ll;
            //BaiduAPI.NearManager.Search(r);

            //BaiduAPI.PlaceRequest rq = new BaiduAPI.PlaceRequest();
            //rq.ak = "WjUg5aNHrFolqN5Utm0GFwVl";
            //rq.query = "学校$银行$公交";
            //rq.scope = BaiduAPI.ScopeType.Details;
            //rq.output = BaiduAPI.OutputType.json;
            //rq.region = "全国";
            //rq.page_num = 0;
            //rq.page_size = 20000;
            //rq.location = "22.55932,114.04508";
            //rq.radius = 3000;
            //BaiduAPI.PlaceResponse rs = BaiduAPI.PlaceAPIManger.SearchPOI(rq);


            //LNKPPhotoManager.test();
            //ProjectApi.test();
            //IList<SYSCode> sys = new Class1().getlist();
            //string json = JsonConvert.SerializeObject(sys);
            //json = json.TrimEnd('}') + ",\"aaa\":333}";
            //string json = "{\"Id\":1109,\"Code\":1109001,\"CodeName\":\"待分配\",\"aaa\":333}";
            //SYSCode sys2 = JsonConvert.DeserializeObject<SYSCode>(json);

            //string url = "http://localhost:50887/API/FxtMobileAPI.svc/Entrance/A";
            //HttpClient web = new HttpClient();
            //var par = new
            //{
            //    sinfo = (new { appid = "", apppwd = "", signname = "", time = "", code = "" }).ToJson(),
            //    info = (new
            //    {
            //        appinfo = "",
            //        uinfo = "",
            //        funinfo = new { type = "test", code = 1109001 }
            //    }).ToJson()
            //};
            //HttpResponseMessage hrm = web.PostAsJsonAsync(url, par).Result;
            //string str = hrm.Content.ReadAsStringAsync().Result;

            // DATProject p = new DATProject();
            // Dictionary<string, string> projectKey = new Dictionary<string, string>();
            // projectKey.Add("build_lng", "X,decimal?,null,物业精度");//物业经度
            // projectKey.Add("build_lat", "Y,decimal?,null,物业纬度");//物业纬度
            // projectKey.Add("locale_lng", "AllotFlowX,decimal?,not null,查勘员现场经度(插入表Dat_AllotFlow)");//查勘员现场经度
            // projectKey.Add("locale_lat", "AllotFlowY,decimal?,not null,查勘员现场纬度(插入表Dat_AllotFlow)");//查勘员现场纬度
            // projectKey.Add("project_name", "ProjectName,string,not null,楼盘名称");//楼盘名称
            // projectKey.Add("cityid", "CityID,int,not null,城市ID(选择)");//城市ID
            // projectKey.Add("areaid", "AreaID,int,not null,行政区ID(选择)");//行政区ID
            // projectKey.Add("address", "Address,string,not null,物业地址");//物业地址
            // projectKey.Add("complete_date", "EndDate,DateTime?,not null,竣工时间");//竣工时间
            // projectKey.Add("orientation_east", "East,string,null,四至朝向-东");//四至朝向-东
            // projectKey.Add("orientation_west", "West,string,null,四至朝向-西");//四至朝向-西
            // projectKey.Add("orientation_south", "South,string,null,四至朝向-南");//四至朝向-南
            // projectKey.Add("orientation_north", "North,string,null,四至朝向-北");//四至朝向-北
            // projectKey.Add("project_area", "BuildingArea,decimal?,not null,建筑面积");//建筑面积
            // projectKey.Add("floor_area", "LandArea,decimal?,not null,占地面积");//占地面积
            // projectKey.Add("plot_ratio", "CubageRate,decimal?,not null,容积率");//容积率
            // projectKey.Add("greening_rate", "GreenRate,decimal?,not null,绿化率");//绿化率
            // projectKey.Add("manager_company", "manager_company,string,null,物业管理公司(插入表LNK_P_Company,CompanyType=2001004)");//物业管理公司
            // projectKey.Add("manager_fees", "ManagerPrice,nvarchar,not null,物业管理费");//物业管理费
            // projectKey.Add("developers", "developers,string,null,开发商(插入表LNK_P_Company,CompanyType=2001001)");//开发商
            // projectKey.Add("parking_num", "ParkingNumber,int,not null,车位数");//车位数
            // projectKey.Add("house_num", "TotalNum,int,not null,总户数or总套数");//总户数or总套数
            // projectKey.Add("open_time", "SaleDate,DateTime?,not null,开盘时间");//开盘时间
            // projectKey.Add("start_time", "BuildingDate,DateTime?,not null,开工时间");//开工时间
            // projectKey.Add("collection_date", "Dat_AllotSurvey$StateDate,DateTime,not null,采集时间");//采集时间
            // projectKey.Add("detail", "Detail,string,null,楼盘备注");//楼盘备注
            // projectKey.Add("remarks", "AllotFlowRemark,string,null,任务备注(插入表Dat_AllotFlow)");//任务备注
            // projectKey.Add("fxtprojectid", "FxtProjectId,int,null,正式库的楼盘ID");//正式库的楼盘ID


            // Dictionary<string, string> appendageKey = new Dictionary<string, string>();
            // appendageKey.Add("appendagecode", "SYS_Code$AppendageCode$2008,int,not null,配套类型(学校;医院..)(选择)");
            // appendageKey.Add("p_aname", "P_AName,string,not null,配套名字");
            // appendageKey.Add("classcode", "SYS_Code$ClassCode$1012,int,not null,配套等级");


            // Dictionary<string, string> buildingKey = new Dictionary<string, string>();
            // buildingKey.Add("build_name", "BuildingName,string,not null,楼栋名称");//楼栋名称
            // buildingKey.Add("build_card_name", "Doorplate,string,null,门牌号");//门牌号
            // buildingKey.Add("build_nick_name", "OtherName,string,null,楼栋别称");//楼栋别称
            // buildingKey.Add("build_struct", "SYS_Code$StructureCode$2010,int?,not null,建筑结构(选择)");//建筑结构
            // buildingKey.Add("position", "SYS_Code$LocationCode$2011,int?,not null,位置(选择)");//位置
            // buildingKey.Add("average_price", "AveragePrice,decimal?,not null,楼栋均价");//楼栋均价
            // buildingKey.Add("complete_date", "BuildDate,DateTime?,not null,楼栋竣工时间(建筑时间)");//楼栋竣工时间(建筑时间)
            // buildingKey.Add("lift", "IsElevator,int,not null,是否带电梯");//是否带电梯
            // buildingKey.Add("house_lift", "ElevatorRate,string,null,梯户数(梯户比)");//梯户数(梯户比)
            // buildingKey.Add("price_commtent", "PriceDetail,string,null,价格说明");//价格说明
            // buildingKey.Add("remarks", "Remark,string,null,备注");//备注
            // projectKey.Add("fxtbuildingid", "FxtBuildingId,int,null,正式库的楼栋ID");//正式库的楼栋ID

            // Dictionary<string, string> houseKey = new Dictionary<string, string>();
            // houseKey.Add("unit", "UnitNo,string,null,单元名称");//单元名称
            // houseKey.Add("roomNO", "HouseName,string,not null,房号");//房号
            // houseKey.Add("orientation", "SYS_Code$FrontCode$2004,string,not null,朝向(选择)");//朝向
            // houseKey.Add("area", "BuildArea,decimal?,not null,面积");//面积
            // houseKey.Add("houseStruct", "SYS_Code$HouseTypeCode$4001,int?,not null,户型(选择)");//户型结构
            // houseKey.Add("remarks", "Remark,string,null,备注");//备注
            // projectKey.Add("fxthouseid", "FxtHouseId,int,null,正式库的房号ID");//正式库的房号ID

            // StringBuilder sb = new StringBuilder();
            // sb.Append("{");
            // foreach (KeyValuePair<string, string> kvp in projectKey)
            // {
            //     string[] values = kvp.Value.Split(',');
            //     string name = (values[0].Contains("$") ? values[0].Split('$')[1] : values[0]).ToLower();
            //     string value = values[1] + "," + values[2] + "," + values[3];
            //     value = "\"(" + value + ")\"";
            //     sb.Append(name).Append("=").Append(value).Append(",");
            // }
            // sb.Append("appendage").Append("==new[]{ new {");

            // foreach (KeyValuePair<string, string> kvp in appendageKey)
            // {
            //     string[] values = kvp.Value.Split(',');
            //     string name = (values[0].Contains("$") ? values[0].Split('$')[1] : values[0]).ToLower();
            //     string value = values[1] + "," + values[2] + "," + values[3];
            //     value = "\"(" + value + ")\"";
            //     sb.Append(name).Append("=").Append(value).Append(",");
            // }
            // sb.Append("}}");
            // sb.Append(",buildingList==new[]{  new {");

            // foreach (KeyValuePair<string, string> kvp in buildingKey)
            // {
            //     string[] values = kvp.Value.Split(',');
            //     string name = (values[0].Contains("$") ? values[0].Split('$')[1] : values[0]).ToLower();
            //     string value = values[1] + "," + values[2] + "," + values[3];
            //     value = "\"(" + value + ")\"";
            //     sb.Append(name).Append("=").Append(value).Append(",");
            // }
            // sb.Append("houseList==new[]{ new {");

            // foreach (KeyValuePair<string, string> kvp in houseKey)
            // {
            //     string[] values = kvp.Value.Split(',');
            //     string name = (values[0].Contains("$") ? values[0].Split('$')[1] : values[0]).ToLower();
            //     string value = values[1] + "," + values[2] + "," + values[3];
            //     value = "\"(" + value + ")\"";
            //     sb.Append(name).Append("=").Append(value).Append(",");
            // }
            // sb.Append("}}");
            // sb.Append("}}");
            // sb.Append("}");
            // string str = sb.ToString();
            // var obj = new
            // {
            //     x = "(decimal?,null,物业精度)",
            //     y = "(decimal?,null,物业纬度)",
            //     allotflowx = "(decimal?,not null,查勘员现场经度(插入表Dat_AllotFlow))",
            //     allotflowy = "(decimal?,not null,查勘员现场纬度(插入表Dat_AllotFlow))",
            //     projectname = "(string,not null,楼盘名称)",
            //     cityid = "(int,not null,城市ID(选择))",
            //     areaid = "(int,not null,行政区ID(选择))",
            //     address = "(string,not null,物业地址)",
            //     enddate = "(DateTime?,not null,竣工时间)",
            //     east = "(string,null,四至朝向-东)",
            //     west = "(string,null,四至朝向-西)",
            //     south = "(string,null,四至朝向-南)",
            //     north = "(string,null,四至朝向-北)",
            //     buildingarea = "(decimal?,not null,建筑面积)",
            //     landarea = "(decimal?,not null,占地面积)",
            //     cubagerate = "(decimal?,not null,容积率)",
            //     greenrate = "(decimal?,not null,绿化率)",
            //     manager_company = "(string,null,物业管理公司(插入表LNK_P_Company)",
            //     managerprice = "(nvarchar,not null,物业管理费)",
            //     developers = "(string,null,开发商(插入表LNK_P_Company)",
            //     parkingnumber = "(int,not null,车位数)",
            //     totalnum = "(int,not null,总户数or总套数)",
            //     saledate = "(DateTime?,not null,开盘时间)",
            //     buildingdate = "(DateTime?,not null,开工时间)",
            //     statedate = "(DateTime,not null,采集时间)",
            //     detail = "(string,null,楼盘备注)",
            //     allotflowremark = "(string,null,任务备注(插入表Dat_AllotFlow))",
            //     fxtprojectid = "(int,null,正式库的楼盘ID)",
            //     appendage =new[]{  new
            //     {
            //         appendagecode = "(int,not null,配套类型(学校;医院..)(选择)",
            //         p_aname = "(string,not null,配套名字)",
            //         classcode = "(int,not null,配套等级)(选择)"
            //     }},
            //     buildingList = new[]{ new { 
            //                                            buildingname = "(string,not null,楼栋名称)", 
            //                                            doorplate = "(string,null,门牌号)", 
            //                                            othername = "(string,null,楼栋别称)", 
            //                                            structurecode = "(int?,not null,建筑结构(选择))", 
            //                                            locationcode = "(int?,not null,位置(选择))", 
            //                                            averageprice = "(decimal?,not null,楼栋均价)", 
            //                                            builddate = "(DateTime?,not null,楼栋竣工时间(建筑时间))", 
            //                                            iselevator = "(int,not null,是否带电梯)", 
            //                                            elevatorrate = "(string,null,梯户数(梯户比))", 
            //                                            pricedetail = "(string,null,价格说明)", 
            //                                            remark = "(string,null,备注)", 
            //                                            fxtbuildingid="(int,null,正式库的楼栋ID)",
            //                                            houseList =new[]{ new { 
            //                                                              unitno = "(string,null,单元名称)", 
            //                                                              housename = "(string,not null,房号)", 
            //                                                              frontcode = "(string,not null,朝向(选择))", 
            //                                                              buildarea = "(decimal?,not null,面积)", 
            //                                                              housetypecode = "(int?,not null,户型(选择))", 
            //                                                              remark = "(string,null,备注)" ,
            //                                                              fxthouseid="(int,null,正式库的房号ID)"
            //                                                             } }
            //                                            }
            //                       }
            // };
            // string json = new  { data = obj }.ToJson();
            //{"data":{
            //         "x":"(decimal?,null,物业精度)",
            //         "y":"(decimal?,null,物业纬度)",
            //         "allotflowx":"(decimal?,not null,查勘员现场经度(插入表Dat_AllotFlow))",
            //         "allotflowy":"(decimal?,not null,查勘员现场纬度(插入表Dat_AllotFlow))",
            //         "projectname":"(string,not null,楼盘名称)",
            //         "cityid":"(int,not null,城市ID(选择))",
            //         "areaid":"(int,not null,行政区ID(选择))",
            //         "address":"(string,not null,物业地址)",
            //         "enddate":"(DateTime?,not null,竣工时间)",
            //         "east":"(string,null,四至朝向-东)",
            //         "west":"(string,null,四至朝向-西)",
            //         "south":"(string,null,四至朝向-南)",
            //         "north":"(string,null,四至朝向-北)",
            //         "buildingarea":"(decimal?,not null,建筑面积)",
            //         "landarea":"(decimal?,not null,占地面积)",
            //         "cubagerate":"(decimal?,not null,容积率)",
            //         "greenrate":"(decimal?,not null,绿化率)",
            //         "manager_company":"(string,null,物业管理公司(插入表LNK_P_Company)",
            //         "managerprice":"(nvarchar,not null,物业管理费)",
            //         "developers":"(string,null,开发商(插入表LNK_P_Company)",
            //         "parkingnumber":"(int,not null,车位数)",
            //         "totalnum":"(int,not null,总户数or总套数)",
            //         "saledate":"(DateTime?,not null,开盘时间)",
            //         "buildingdate":"(DateTime?,not null,开工时间)",
            //         "statedate":"(DateTime,not null,采集时间)",
            //         "detail":"(string,null,楼盘备注)",
            //         "allotflowremark":"(string,null,任务备注(插入表Dat_AllotFlow))",
            //         "fxtprojectid":"(int,null,正式库的楼盘ID)",
            //         "appendage":[
            //                         {
            //                         "appendagecode":"(int,not null,配套类型(学校;医院..)(选择)",
            //                         "p_aname":"(string,not null,配套名字)",
            //                         "classcode":"(int,not null,配套等级)(选择)"
            //                         },
            //                         ...
            //                     ],
            //         "buildinglist":[
            //                             {
            //                             "buildingname":"(string,not null,楼栋名称)",
            //                             "doorplate":"(string,null,门牌号)",
            //                             "othername":"(string,null,楼栋别称)",
            //                             "structurecode":"(int?,not null,建筑结构(选择))",
            //                             "locationcode":"(int?,not null,位置(选择))",
            //                             "averageprice":"(decimal?,not null,楼栋均价)",
            //                             "builddate":"(DateTime?,not null,楼栋竣工时间(建筑时间))",
            //                             "iselevator":"(int,not null,是否带电梯)",
            //                             "elevatorrate":"(string,null,梯户数(梯户比))",
            //                             "pricedetail":"(string,null,价格说明)",
            //                             "remark":"(string,null,备注)",
            //                             "fxtbuildingid":"(int,null,正式库的楼栋ID)",
            //                             "houselist":[
            //                                           {
            //                                            "unitno":"(string,null,单元名称)",
            //                                            "housename":"(string,not null,房号)",
            //                                            "frontcode":"(string,not null,朝向(选择))",
            //                                            "buildarea":"(decimal?,not null,面积)",
            //                                            "housetypecode":"(int?,not null,户型(选择))",
            //                                            "remark":"(string,null,备注)",
            //                                            "fxthouseid":"(int,null,正式库的房号ID)"
            //                                            }, 
            //                                            ...
            //                                          ]
            //                              },
            //                              ...
            //                         ]
            //         }
            //  }

            return View();
        }
        public ActionResult test1()
        {
            return View();
        }
        public ActionResult test2()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }


    }
}
