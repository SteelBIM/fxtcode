using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace FxtDataAcquisition.Application.Services
{
    public class CityService : ICityService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CityService));
        private readonly IUnitOfWork _unitOfWork;
        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<FxtApi_SYSProvince> GetProvinceCityListBy(string userName, string signName, List<UserCenter_Apps> appList)
        {
            List<FxtApi_SYSProvince> provinceList = CacheHelper.GetCache("provinceList") as List<FxtApi_SYSProvince>;
            if (provinceList == null)
            {
                log.Info("写入省份缓存");
                provinceList = DataCenterProvinceApi.GetProvinceAll(userName, signName, appList);// SYSProvinceApi.GetAllProvince();//省份
                HttpHelper.CurrentCache.Insert("provinceList", provinceList, null, Cache.NoAbsoluteExpiration, DateTime.Now.AddDays(1) - DateTime.Now);
            }
            List<FxtApi_SYSCity> cityList = CacheHelper.GetCache("cityList") as List<FxtApi_SYSCity>;
            if (cityList == null)
            {
                log.Info("写入城市缓存");
                cityList = DataCenterCityApi.GetCityAll(userName, signName, appList);// SYSProvinceApi.GetAllProvince();//省份
                HttpHelper.CurrentCache.Insert("cityList", cityList, null, Cache.NoAbsoluteExpiration, DateTime.Now.AddDays(1) - DateTime.Now);
            }
            //获取当前公司开通产品（踩盘助手）的城市ID
            int[] cityIds = UserCenterUserInfoApi.GetCompanyProductCityIds(signName, FxtAPI.FxtUserCenter.Common.systypeCode, userName, signName, appList);
            //获取当前公司开通产品（数据中心）的城市ID
            int[] dataCityIds = UserCenterUserInfoApi.GetCompanyProductCityIds(signName, 1003002, userName, signName, appList);
            if (dataCityIds != null && dataCityIds.Length > 0)
            {
                cityList = cityList.Where(m => dataCityIds.Contains(m.CityId)).ToList();
                provinceList = provinceList.Where(m => cityList.Any(c => m.ProvinceId == c.ProvinceId)).ToList();
            }
            //获取当前公司开通产品模块（数据中心）的城市ID
            int[] moduleCityIds = UserCenterUserInfoApi.GetCompanyProductMuduleCityIds(signName, 1003002, userName, signName, appList);
            if (moduleCityIds != null && moduleCityIds.Length > 0)
            {
                cityList = cityList.Where(m => moduleCityIds.Contains(m.CityId)).ToList();
                provinceList = provinceList.Where(m => cityList.Any(c => m.ProvinceId == c.ProvinceId)).ToList();
            }

            //数据中心角色城市
            var sysRoleUserIds = DataCenterSysRoleUser.GetSysRoleUserIds(userName, signName, appList);
            if (sysRoleUserIds != null && !sysRoleUserIds.Contains(0))
            {
                cityList = cityList.Where(m => sysRoleUserIds.Contains(m.CityId)).ToList();
                provinceList = provinceList.Where(m => cityList.Any(c => m.ProvinceId == c.ProvinceId)).ToList();
                //cityIds = cityIds.Where(m => sysRoleUserIds.Any(r => r == m)).ToArray();
            }

            //踩盘角色权限
            var roleUser = _unitOfWork.SysRoleUserRepository.Get(m => m.UserName == userName).Select(m => m.CityID).ToList();
            if (roleUser != null && !roleUser.Contains(0))
            {
                cityList = cityList.Where(m => roleUser.Contains(m.CityId)).ToList();
                provinceList = provinceList.Where(m => cityList.Any(c => m.ProvinceId == c.ProvinceId)).ToList();
                //cityIds = cityIds.Where(m => sysRoleUserIds.Any(r => r == m)).ToArray();
            }

            if (cityIds == null || dataCityIds == null || moduleCityIds == null || sysRoleUserIds == null || sysRoleUserIds.Count < 1 || roleUser == null || roleUser.Count < 1)
            //    if (cityIds == null || sysRoleUserIds == null || sysRoleUserIds.Count < 1 || roleUser == null || roleUser.Count < 1)
            {
                cityList = new List<FxtApi_SYSCity>();
                provinceList = new List<FxtApi_SYSProvince>();
            }

            //所有城市
            if (!cityIds.Contains(0))
            {
                cityList = cityList.Where(m => cityIds.Contains(m.CityId)).ToList();
                provinceList = provinceList.Where(m => cityList.Any(c => m.ProvinceId == c.ProvinceId)).ToList();
            }

            provinceList.ForEach(m => m.CityList = cityList.Where(c => c.ProvinceId == m.ProvinceId).ToList());

            return provinceList;
        }
    }
}
