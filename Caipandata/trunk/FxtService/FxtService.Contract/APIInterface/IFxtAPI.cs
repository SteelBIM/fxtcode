using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
/**
 * 作者: 李晓东
 * 时间: 2013.12.03
 * 摘要: 新建Wcf (契约)Contract IFxtAPI(接口)   
 *       2013.12.18 针对 Cross 把参数 string crossType更换成 int codeType 修改人:李晓东
 *       2013.12.19 新增接口方法GetProvince 
 *       GetArea 修改人:李晓东
 *       2014.01.23 修改人:李晓东
 *                  新增:GetCityListByCityName
 *       2014.02.13 修改人:李晓东
 *                  新增:GetProjectByCityIDAndLikePrjName
                         GetProjectJoinPMatchByPNameOrPAddressCityId
 *       2014.02.20 修改人:李晓东
 *                  新增:Entrance API入口
 *                  新增:GetHouseByHouse_City_Building 得到房号
 *                       GetBuildingByProject_City_Build 得到楼栋
 *       2014.03.14 修改人:曾智磊
 *                  新增GetPriviCompanyShowDataByCompanyIdAndCityId 得到可查询数据的company
 *       2014.05.27 修改人:李晓东
 *                  新增:GetProvinceADOById、GetCityADOByCityId、GetAreaADOByAreaId
 *                       GetProjectJoinPMatchADOByPNameOrPAddressCityId
 *                       GetProjectJoinProjectMatchADOByProjectNameCityId
 * **/
namespace FxtService.Contract.APIInterface
{
    [ServiceContract()]
    public interface IFxtAPI
    {

        #region 楼盘均价计算(DATProjectAvgPrice)
        /// <summary>
        /// 重置交叉值计算(根据条件更新,注意:此方法用于原交叉值已经存在的情况下)(up)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="purposeTypeCode"></param>
        /// <param name="buildingTypeCode">如果purposeTypeCode为别墅相关类型时则为0</param>
        /// <param name="buildingAreaType">如果purposeTypeCode为别墅相关类型时则为0</param>
        /// <param name="date"></param>
        /// <returns>DATProjectAvgPrice 结构json字符串(带UrlEncode编码)</returns>
        //[OperationContract]
        string ResetCrossBy(int projectId, int cityId, int purposeTypeCode, int buildingTypeCode, int buildingAreaType, string date);


        /// <summary>
        /// 楼盘交叉值计算
        /// </summary>
        /// <param name="projectId">楼盘ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="codeType">交叉类型编号,普通住宅,别墅</param>
        /// <param name="date">时间</param>
        /// <returns></returns>
        //[OperationContract]
        string Cross(int projectId, int cityId, int codeType, string date);
        /// <summary>
        /// 获取指定楼盘and用途均价
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="codeType"></param>
        /// <param name="date">{type:1,message:"",data:均价,count:0}</param>
        /// <returns></returns>
        string CrossProjectByCodeType(int projectId, int cityId, int codeType, string date);
        #endregion
        #region 省份(SYS_Province)
        /// <summary>
        /// 获得省份
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        string GetProvince();

        /// <summary>
        /// 根据名称获取省份
        /// </summary>
        /// <param name="provinceName">省份名称</param>
        /// <returns></returns>
        //[OperationContract]
        string GetProvinceByName(string provinceName);
        /// <summary>
        /// 根据ID获取省份
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetProvinceById(int provinceId);

        /// <summary>
        /// 根据ID获取省份(类型ADO)
        /// </summary>
        /// <param name="provinceId">省份ID</param>
        /// <returns></returns>
        string GetProvinceADOById(int provinceId);
        #endregion

        #region 城市(SYS_City)
        /// <summary>
        /// 获得城市
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        string GetCity(int provinceId);
        /// <summary>
        /// 根据城市名称获取城市
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetCityByCityName(string cityName);

        /// <summary>
        /// 根据名称得到城市列表
        /// </summary>
        /// <param name="cityName">城市名</param>
        /// <returns></returns>
        //[OperationContract]
        string GetCityListByCityName(string cityName);
        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        string GetAllCity();
        /// <summary>
        /// 根据城市ID获得省份
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        //[OperationContract]
        string GetProvinceByCityId(int cityId);
        /// <summary>
        /// 根据城市ID获得城市
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        //[OperationContract]
        string GetCityByCityId(int cityId);

        /// <summary>
        /// 根据城市ID获得城市(类型ADO)
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        string GetCityADOByCityId(int cityId);

        #endregion

        #region 行政区(SYS_Area)
        /// <summary>
        /// 获得行政区
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        string GetArea(int cityId);
        /// <summary>
        /// 获得行政区
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        string GetAreaByCityName(string cityName);


        /// <summary>
        /// 根据区域名称得到相关所有区域
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <returns></returns>
        //[OperationContract]
        string GetAreaListByAraeName(string areaName);

        /// <summary>
        /// 根据行政区获得城市
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetCityByAreaId(int areaId);
        /// <summary>
        /// 更加多个areaId获取行政区
        /// </summary>
        /// <param name="areaIds"></param>
        /// <returns></returns>
        string GetSYSAreaByAreaIds(string areaIds);

        /// <summary>
        /// 根据行政区ID获得行政区
        /// </summary>
        /// <param name="areaId">行政区ID</param>
        /// <returns></returns>
        //[OperationContract]
        string GetAreaByAreaId(int areaId);

        /// <summary>
        /// 根据行政区ID获得行政区(类型ADO)
        /// </summary>
        /// <param name="areaId">行政区ID</param>
        /// <returns></returns>
        string GetAreaADOByAreaId(int areaId);
        #endregion

        #region 楼盘(DAT_Project)
        /// <summary>
        /// 根据城市名称+楼盘名称 模糊检索出楼盘信息
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="areaName"></param>
        /// <param name="projectName"></param>
        /// <param name="length"></param>
        /// <returns>DATProject</returns>
        //[OperationContract]
        string GetProjectByCityNameAndLikePrjName(string cityName, string projectName, int length);

        /// <summary>
        /// 根据城市ID+楼盘名称 模糊检索出楼盘信息 李晓东
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="projectName">楼盘名称</param>
        /// <returns></returns>
        //[OperationContract]
        string GetProjectByCityIDAndLikePrjName(int cId, int aId, string pName);

        /// <summary>
        /// 根据地址、楼盘名称查找信息 李晓东
        /// </summary>
        /// <param name="pName">楼盘名称</param>
        /// <param name="pAddress">楼盘地址</param>
        /// <param name="cId">城市</param>
        /// <returns></returns>
        //[OperationContract]
        string GetProjectJoinPMatchByPNameOrPAddressCityId(string pName, string pAddress, int cId);


        /// <summary>
        /// 根据地址、楼盘名称查找信息   李晓东 DO
        /// </summary>
        /// <param name="pName">楼盘名称</param>
        /// <param name="pAddress">楼盘地址</param>
        /// <param name="cId">城市</param>
        /// <returns></returns>
        string GetProjectJoinPMatchADOByPNameOrPAddressCityId(string pName, string pAddress, int cId);

        /// <summary>
        /// 根据城市名称+楼盘名称 模糊检索出楼盘信息
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="projectName"></param>
        /// <param name="length"></param>
        /// <returns>DATProjectView</returns>
        //[OperationContract]
        string GetProjectViewByCityNameAndLikePrjName(string cityName, string projectName, int length);

        /// <summary>
        /// 根据城市ID、楼盘ID、楼栋名称得到楼栋 李晓东
        /// </summary>
        /// <param name="cId">城市ID</param>
        /// <param name="pId">楼盘ID</param>
        /// <param name="bName">楼栋名称</param>
        /// <returns></returns>
        //[OperationContract]
        string GetBuildingByProjectIdCityIDAndLikeBuildingName(int cId, int pId, string bName);
         /// <summary>
        /// 根据城市ID、楼栋ID、房号名称等到房号
        /// </summary>
        /// <param name="cId">城市ID</param>
        /// <param name="bId">楼栋ID</param>
        /// <param name="hName">房号名称</param>
        /// <returns></returns>
        //[OperationContract]
        string GetHouseByBuildingIdCityIDAndLikeHouseName(int cId, int bId, string hName);
        /// <summary>
        /// 根据城市ID and 多个楼盘Ids获取案例信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectIds"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetProjectByCityIdAndProjectIds(int cityId, string projectIds);
        /// <summary>
        /// 根据名称成城市ID,获取楼盘信息(GetProjectJoinProjectMatchByProjectNameCityId)
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetProjectJoinProjectMatchByProjectNameCityId(string projectName, int cityId);

        /// <summary>
        /// 根据名称成城市ID,获取楼盘信息(关联网络名查询) ADO
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        string GetProjectJoinProjectMatchADOByProjectNameCityId(string projectName, int cityId);
        /// <summary>
        /// 根据楼盘名称+城市名获取楼盘信息
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="cityName"></param>
        /// <returns>DATProject结构json字符串(带UrlEncode编码)</returns>
        //[OperationContract]
        string GetProjectByProjectNameAndCityName(string projectName, string cityName);
        /// <summary>
        /// 获取指定城市的楼盘列表
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetProjectByCityNameAndPage(string cityName, int pageIndex, int pageSize, int isGetCount);

        /// <summary>
        /// 新建楼盘
        /// </summary>
        /// <param name="projectName">楼盘名称</param>
        /// <param name="cityId"></param>
        /// <param name="areaId">行政区ID</param>
        /// <param name="purposeCode">住用途ID</param>
        /// <param name="address">楼盘地址</param>
        /// <returns></returns>
        //[OperationContract]
        string InsertProject(string projectName, int cityId, int areaId, int purposeCode, string address);


        #endregion

        #region 楼盘网络名(SYS_ProjectMatch)
        /// <summary>
        /// 新建网络名
        /// </summary>
        /// <param name="netName"></param>
        /// <param name="projectName"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        //[OperationContract]
        string InsertSYSProjectMatch(string netName, string projectName, int projectId, int cityId, string ip, string validate);
        /// <summary>
        /// 新建网络名(多个)
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        //[OperationContract]
        string InsertSYSProjectMatchList(string jsonData);
        #endregion

        #region 案例(DAT_Case)


        #region(查询)

        /// <summary>
        /// 根据房号、城市、楼栋得到房号(物业)中的房号 李晓东
        /// </summary>
        /// <param name="houseName">房号</param>
        /// <param name="cId">城市</param>
        /// <param name="bId">楼栋(楼宇)</param>
        /// <returns></returns>
        //[OperationContract]
        string GetHouseByHouse_City_Building(string houseName, int cId, int bId);
        /// <summary>
        /// 根据楼盘、城市、楼宇名称(楼栋名称)得到楼宇(楼栋) 信息 李晓东
        /// </summary>
        /// <param name="pId">楼盘</param>
        /// <param name="cId">城市</param>
        /// <param name="bName">楼栋名称</param>
        /// <returns></returns>
        //[OperationContract]
        string GetBuildingByProject_City_Build(int pId, int cId, string bName);
        /// <summary>
        /// 根据城市ID and 多个案例ID获取案例信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetCaseByCityIdAndCaseIds(int cityId, string caseIds);
        /// <summary>
        /// 获取案例信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="caseId"></param>
        /// <returns>DATCase类型json格式,带UrlEncode编码</returns>
        //[OperationContract]
        string GetCaseByCityIdAndCaseId(int cityId, int caseId);
        /// <summary>
        /// 根据城市ID and 多个楼盘Ids获取案例Id+楼盘名
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="caseIds"></param>
        /// <returns>json:[{CaseId:CaseId,ProjectName:"ProjectName"},{}...]字符串属性带UrlEncode编码</returns>
        //[OperationContract]
        string GetCaseIdJoinProjectNameByCityIdAndCaseIds(int cityId, string caseIds);
        /// <summary>
        /// 获取案例列表
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="projectId">楼盘ID</param>
        /// <param name="fxtCompanyId">公司ID,房讯通为25</param>
        /// <param name="buildingTypeCode">建筑类型code</param>
        /// <param name="purposeCode">用途</param>
        /// <param name="buildingAreaCode">面积段code</param>
        /// <param name="startDate">案例时间start</param>
        /// <param name="endDate">案例时间end</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetCaseByCityIdAndProjectIdAndPurposeCodeAndBuildingTypeCodeAndAreaTypeAndDateAndPage(int cityId, int projectId, string fxtCompanyIds, int? buildingTypeCode, int purposeCode, int? buildingAreaCode, string startDate, string endDate, int pageIndex, int pageSize, int isGetCount);
        /// <summary>
        /// 获取指定日期楼盘下案例个数(别墅,普通住宅的个数)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyIds">由逗号分隔的多个公司ID</param>
        /// <param name="dates">逗号分隔的日期数组(例如:"2012-03,2012-04")</param>
        /// <returns>json:[{ ProjectId:1, CityId:1, Date:"2012-03",PurposePublicCount:0, PurposeVillaCount:0},{}...]</returns>
        //[OperationContract]
        string GetCaseCountJoinProjectJoinPurposeTypeByCityIdAndProjectIdAndDates(int cityId, int projectId, string fxtCompanyIds, string dates);
        /// <summary>
        /// 获取指定日期楼盘下普通住宅的案例个数(各建筑面积,建筑类型下的案例个数)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyIds">由逗号分隔的多个公司ID</param>
        /// <param name="date"></param>
        /// <returns>json:[{ ProjectId:1, CityId:1, Date:"2012-03", BuildingTypeCode:1, BuildingAreaTypeCode:1, Count:0, },{}...]</returns>
        //[OperationContract]
        string GetCaseCountJoinProjectJoinBuildingTypeJoinAreaTypeByPublicPurposeAndCityIdAndProjectIdAndDate(int cityId, int projectId, string fxtCompanyIds, string date);
        /// <summary>
        /// 获取指定日期楼盘下各别墅用途的案例个数
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyIds">由逗号分隔的多个公司ID</param>
        /// <param name="date">日期(例如:"2012-03")</param>
        /// <returns>json:[{ ProjectId:1, CityId:1, Date:"2012-03", PurposeTypeCode:1, Count:0, },{}...]</returns>
        //[OperationContract]
        string GetCaseCountJoinProjectJoinPurposeTypeByVillaPurposeAndCityIdAndProjectIdAndDate(int cityId, int projectId, string fxtCompanyIds, string date);

        #endregion

        #region (更新)
        /// <summary>
        /// 删除案例,根据caseId,cityName
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="caseIds"></param>
        /// <param name="ip"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        //[OperationContract]
        string DeleteCaseByCityNameAndCaseIds(string cityName, string caseIds);
        /// <summary>
        /// 根据多个caseId删除案例(方法创建时间:2014-2-20,创建人:曾智磊)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        //[OperationContract]
        string DeleteCaseByCityIdAndCaseIds(int cityId, string caseIds);
        /// <summary>
        /// 新增案例
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="houseNo"></param>
        /// <param name="caseDate"></param>
        /// <param name="purposeCode"></param>
        /// <param name="buildingArea"></param>
        /// <param name="unitPrice"></param>
        /// <param name="totalPrice"></param>
        /// <param name="caseTypeCode"></param>
        /// <param name="structureCode"></param>
        /// <param name="buildingTypeCode"></param>
        /// <param name="floorNumber"></param>
        /// <param name="totalFloor"></param>
        /// <param name="houseTypeCode"></param>
        /// <param name="frontCode"></param>
        /// <param name="moneyUnitCode"></param>
        /// <param name="remark"></param>
        /// <param name="areaId"></param>
        /// <param name="buildingDate"></param>
        /// <param name="fitmentCode"></param>
        /// <param name="subHouse"></param>
        /// <param name="peiTao"></param>
        /// <param name="createUser"></param>
        /// <param name="sourceName"></param>
        /// <param name="sourceLink"></param>
        /// <param name="sourcePhone"></param>
        /// <param name="ip"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        //[OperationContract]
        string InsertCase(int cityId, int projectId, int? buildingId, string houseNo, DateTime caseDate, int? purposeCode,
            decimal? buildingArea, decimal? unitPrice, decimal? totalPrice, int? caseTypeCode, int? structureCode, int? buildingTypeCode,
            int? floorNumber, int? totalFloor, int? houseTypeCode, int? frontCode, int? moneyUnitCode, string remark, int? areaId,
            string buildingDate, int? fitmentCode, string subHouse, string peiTao, string createUser, string sourceName, string sourceLink, string sourcePhone);

        /// <summary>
        /// 修改案例
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="caseJson"></param>
        /// <param name="ip"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        //[OperationContract]
        string UpdateCase(int cityId, string caseJson);
        #endregion

        #endregion

        #region 基础数据(SYS_Code)
        /// <summary>
        /// 获取用于楼盘的所有用途
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        string GetAllProjectPurposeCode();
        /// <summary>
        /// 根据ID获取相应的code列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetSYSCodeByID(int id);
        /// <summary>
        /// 获取别墅相关的住宅用途
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        string GetPurposeTypeCodeVillaType();
        //[OperationContract]
        string GetSYSCodeByCode(int code);
        #endregion

        #region 楼盘均价(DAT_ProjectAvgPrice)
        /// <summary>
        /// 获取均价信息
        /// </summary>
        /// <param name="projectIds">逗号分隔的多个projectId</param>
        /// <param name="cityId"></param>
        /// <param name="dateRange">计算范围</param>
        /// <param name="avgPriceDates">逗号分隔的多个avgPriceDate</param>
        /// <returns></returns>
        //[OperationContract]
        string GetProjectAvgPriceByProjectIdsAndCityIdAndDateRangeAndAvgPriceDates(string projectIds, int cityId, int dateRange, string avgPriceDates);
        /// <summary>
        /// 查询DATProjectAvgPrice,根据用途,建筑类型,面积段,日期计算区间
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="purposeCode"></param>
        /// <param name="buildingTypeCode"></param>
        /// <param name="buildingAreaCode"></param>
        /// <param name="date"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetProjectAvgPriceByProjectIdAndCityIdAndBy(int projectId, int cityId, int purposeCode, int? buildingTypeCode, int? buildingAreaCode, string date, int dateRange);
        #endregion

        #region (Privi_Company_ShowData)
        string GetPriviCompanyShowDataByCompanyIdAndCityId(int fxtCompanyId, int cityId);

        /// <summary>
        /// 获得所有银行信息
        /// </summary>
        /// <returns></returns>
        string GetPriviCompanyAllBank();
        #endregion

        #region 公共入口
        /// <summary>
        /// API入口点
        /// </summary>
        /// <param name="date">验证时间</param>
        /// <param name="code">加密串</param>
        /// <param name="type">类型</param>
        /// <param name="name">方法</param>
        /// <param name="value">方法值</param>
        /// <returns></returns>
        [OperationContract]
        object Entrance(string date, string code, string type, string name, string value);
        #endregion

    }
}
