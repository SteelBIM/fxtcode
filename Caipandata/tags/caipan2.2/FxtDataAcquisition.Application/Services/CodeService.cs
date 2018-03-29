using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Services
{
    public class CodeService : ICodeService
    {
        /// <summary>
        /// 获取配套code
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetAppendageCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2008);
            return list;
        }
        /// <summary>
        /// 获取建筑类型(结构)code
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetStructureCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2010);
            return list;
        }
        /// <summary>
        /// 获取楼栋位置code
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetBuildingLocationCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2011);
            return list;
        }
        /// <summary>
        /// 获取朝向code
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetFrontCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2004);
            return list;
        }
        /// <summary>
        /// 获取户型code
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetHouseTypeCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(4001);
            return list;
        }
        /// <summary>
        /// 获取等级code
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetClassCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(1012);
            return list;
        }
        /// <summary>
        /// 获取景观code
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetSightCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2006);
            return list;
        }
        /// <summary>
        /// 获取图片类型Code
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetPhotoTypeCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2009).Where(m => m.Code < 2009010).ToList();
            return list;
        }
        /// <summary>
        /// 楼栋外墙
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetWallCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(6058);
            return list;
        }
        ///<summary>
        /// 户型结构
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetHouseStructureCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2005);
            return list;
        }
        ///<summary>
        /// 用途
        /// </summary>
        /// <returns></returns>
        public List<SYSCode> GetHousePurposeCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(1002);
            return list;
        }
        /// <summary>
        /// 获取所有状态code
        /// </summary>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public List<SYSCode> GetAllotStatusCodeList(string username, string signname, List<UserCenter_Apps> appList)
        {
            List<SYSCode> list = DataCenterCodeApi.GetCodeById(1035, username, signname, appList);
            return list;
        }

    }
}
