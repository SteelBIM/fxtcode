using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;


namespace FXT.DataCenter.Domain.Services
{
    public interface IDropDownList
    {
        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <returns></returns>
        IQueryable<SYS_Province> GetProvince();
        
        /// <summary>
        /// 根据省份ID获取所辖城市
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        IQueryable<SYS_City> GetCityByProId(int proId);

        /// <summary>
        /// 根据城市ID获取城市
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        IQueryable<SYS_City> GetCityByCityId(int cityId);

        /// <summary>
        /// 根据城市ID获取城市名称列表
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetCityNameByCityID(int cityId);

        /// <summary>
        /// 获取城市名称
        /// </summary>
        /// <param name="provId">省ID</param>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetCityName(int provId=0, int cityId=0);
        
        /// <summary>
        /// 根据省ID获得省名称
        /// </summary>
        /// <param name="ProvinceId">省ID</param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetProNameByProId(int ProvinceId);

        /// <summary>
        /// 获取行政区名称列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetAreaName(int cityId);


        /// <summary>
        /// 获取片区区名称列表
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetSubAreaName(int areaId);

        /// <summary>
        /// 获取土地用途
        /// </summary>
        /// <param name="code">土地用途code</param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetLandPurpose(int code);

          /// <summary>
        /// 根据城市Id,产品Code获取公司名称
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="companyTypeCode">产品Code</param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetCompanyName(int cityId, int companyTypeCode);

         /// <summary>
        /// 根据城市Id,产品Code获取公司名称
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="companyTypeCode">产品Code</param>
        /// <returns></returns>
        string GetCompanyName(int cityId, int companyTypeCode, string split);
        /// <summary>
        /// 根据ID获取相关字典信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetDictById(int id);
        /// <summary>
        /// 根据subcode获取相关
        /// </summary>
        /// <param name="code">subCode</param>
        /// <returns></returns>
        IList<SYS_Code> GetDictBySubCode(int code);
        /// <summary>
        /// 根据名称获取CODE
        /// </summary>
        /// <returns></returns>
        int GetCodeByName(string name,params int[] typeId);
        /// <summary>
        /// 根据code获取name
        /// </summary>
        /// <returns></returns>
        string GetNameByCode(int code);

        /// <summary>
        /// 根据行政区名称获取其ID
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetAreaIdByName(int cityId,string name);

         /// <summary>
        /// 根据片区Id获取片区名称
        /// </summary>
        /// <param name="subAreaId"></param>
        /// <returns></returns>
        string GetSubAreaNameBySubAreaId(int subAreaId);
        /// <summary>
        /// 根据行政区ID获取其名称
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        string GetAreaNameByAreaId(int areaId);

        /// <summary>
        /// 根据片区获取其ID
        /// </summary>
        /// <param name="name">片区名称</param>
        /// <param name="areaId">行政区ID</param>
        /// <returns></returns>
        int GetSubAreaIdByName(string name,int areaId);
        /// <summary>
        /// 根据名称获取城市ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetCityIdByName(string name);

        /// <summary>
        /// 根据名称获取环线ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetAreaLineIdByName(string name);

        /// <summary>
        /// 根据商圈名称获取其ID
        /// </summary>
        /// <param name="subAreaName">商圈名称</param>
        /// <param name="areaId">行政区ID</param>
        /// <returns></returns>
        int GetBizSubAreaIdByName(string subAreaName,int areaId);

        /// <summary>
        /// 根据商圈ID获取商圈名称
        /// </summary>
        /// <param name="subAreaName"></param>
        /// <returns></returns>
        string GetBizSubAreaNameBySubAreaId(int subAreaId);

        /// <summary>
        /// 根据商业街获取商业楼栋
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IQueryable<Dat_Building_Biz> GetBusinessBuilding(long projectId);

        /// <summary>
        /// 获取指定城市的行政区
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        IQueryable<SYS_Area> GetAreaIds(int cityId);

        /// <summary>
        /// 获取系统所有code
        /// </summary>
        /// <returns></returns>
        IQueryable<SYS_Code> GetCodes();
    }
}
