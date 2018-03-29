namespace FxtDataAcquisition.Web.Common
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Collections.Generic;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;

    using Newtonsoft.Json.Linq;

    public static class WebCommon
    {
        #region (各链接常量)
        //public const string Url_DepartmentInfo_DepartmentManager = "/DepartmentInfo/DepartmentManager";
        //public const string Url_UserInfo_UserManager = "/UserInfo/UserManager";
        //public const string Url_AllotFlowInfo_AllotFlowManager = "/AllotFlowInfo/AllotFlowManager";
        //public const string Url_AllotFlowInfo_AllotDetailed = "/AllotFlowInfo/AllotDetailed";
        public const string Url_DepartmentInfo_DepartmentManager = "/DepartmentInfo/DepartmentManager";
        public const string Url_DepartmentInfo_SetDepartment = "/DepartmentInfo/SetDepartment?departmentId={0}&companyName={1}";
        public const string Url_DepartmentInfo_SetDepartment_SubmitData_Api = "/DepartmentInfo/SetDepartment_SubmitData_Api";
        public const string Url_UserInfo_UserManager = "/UserInfo/UserManager";
        public const string Url_UserInfo_EditUser = "/UserInfo/EditUser?userName={0}";
        public const string Url_UserInfo_EditUser_SubmitData_Api = "/UserInfo/EditUser_SubmitData_Api";
        public const string Url_Login_Login = "/Login/Login";
        public const string Url_Login_Login_Out = "/Login/Login?type=signout";
        public const string Url_Login_Login_Open = "/Login/Login?type=open";
        public const string Url_Login_LoginBox = "/Login/LoginBox";
        public const string Url_Login_NotRight = "/Login/NotRight";
        public const string Url_AllotFlowInfo_AllotFlowManager = "/AllotFlowInfo/AllotFlowManager";//全部
        public const string Url_AllotFlowInfo_AllotFlowManager_1035001 = "/allotflowinfo/allotflowmanager?statuscode=1035001";//未分配
        public const string Url_AllotFlowInfo_AllotFlowManager_1035002 = "/allotflowinfo/allotflowmanager?statuscode=1035002";//待查勘
        public const string Url_AllotFlowInfo_AllotFlowManager_1035004 = "/allotflowinfo/allotflowmanager?statuscode=1035004";//查勘中
        public const string Url_AllotFlowInfo_AllotFlowManager_1035005 = "/allotflowinfo/allotflowmanager?statuscode=1035005";//已查勘
        public const string Url_AllotFlowInfo_AllotFlowManager_1035006 = "/allotflowinfo/allotflowmanager?statuscode=1035006";//待审批
        public const string Url_AllotFlowInfo_AllotFlowManager_1035008 = "/allotflowinfo/allotflowmanager?statuscode=1035008";//审批已通过
        public const string Url_AllotFlowInfo_AllotFlowManager_1035010 = "/allotflowinfo/allotflowmanager?statuscode=1035010";//已入库
        public const string Url_AllotFlowInfo_AssignAllotToUser = "/AllotFlowInfo/AssignAllotToUser";
        public const string Url_AllotFlowInfo_AllotDetailed = "/AllotFlowInfo/AllotDetailed";
        public const string Url_AllotFlowInfo_EditProject = "/AllotFlowInfo/EditProject";
        public const string Url_AllotFlowInfo_EditBuilding = "/AllotFlowInfo/EditBuilding";
        public const string Url_AllotFlowInfo_EditHouse = "/AllotFlowInfo/EditHouse";

        public const string Url_DatabaseCall_Index = "/DatabaseCall/Index";
        public const string Url_DatabaseCall_BuildingIndex = "/DatabaseCall/BuildingIndex";
        public const string Url_SurveyCount_Index = "/SurveyCount/Index";
        public const string Url_Role_Index = "/Role/Index";
        
        
        #endregion

        //public static readonly string CityDataBaseJson = "";
        static WebCommon()
        {
            //string configPath = HttpContext.Current.Server.MapPath("/CityDataBase.txt");
            //StringBuilder sb = new StringBuilder();
            //StreamReader sr = new StreamReader(configPath);
            //while (true)
            //{
            //    string str = sr.ReadLine();
            //    if (str == null)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        sb.Append(str);
            //    }
            //}
            //sr.Close();
            //sr.Dispose();
            //CityDataBaseJson = sb.ToString();
        }
        /// <summary>
        /// 获取当前用户开通产品的城市
        /// </summary>
        /// <param name="cityIds"></param>
        /// <param name="provinceList"></param>
        /// <param name="cityList"></param>
        /// <returns>1:所有城市,0:部分城市</returns>
        //public static int GetCityDataBaseByCityIds(int[] cityIds,out List<FxtApi_SYSProvince> provinceList,out List<FxtApi_SYSCity> cityList)
        //{
        //    provinceList = new List<FxtApi_SYSProvince>();
        //    cityList = new List<FxtApi_SYSCity>();
        //    //无城市
        //    if (cityIds == null || cityIds.Length < 1)
        //    {
        //        return 0;
        //    }
        //    GetAllCityDataBase(out provinceList, out  cityList);
        //    //所哟城市
        //    if (cityIds.Contains(0))
        //    {
        //        return 1;
        //    }
        //    cityList = cityList.Where(_jobj => cityIds.Contains(_jobj.CityId)).ToArray().ToJSONjss().ParseJSONList<FxtApi_SYSCity>();
        //    List<int> provinceIds = cityList.Select(obj => obj.ProvinceId).ToList();
        //    provinceList = provinceList.Where(_jobj => provinceIds.Contains(_jobj.ProvinceId)).ToArray().ToJSONjss().ParseJSONList<FxtApi_SYSProvince>();
        //    return 0;
        //}
        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <param name="provinceList"></param>
        /// <param name="cityList"></param>
        //public static void GetAllCityDataBase(out List<FxtApi_SYSProvince> provinceList, out List<FxtApi_SYSCity> cityList)
        //{
        //    provinceList = new List<FxtApi_SYSProvince>();
        //    cityList = new List<FxtApi_SYSCity>();
        //    JObject jobj = JObject.Parse(CityDataBaseJson);
        //    cityList = jobj["cityList"].ToArray().ToJSONjss().ParseJSONList<FxtApi_SYSCity>();
        //    provinceList = jobj["provinceList"].ToArray().ToJSONjss().ParseJSONList<FxtApi_SYSProvince>();
        //}

    }
}