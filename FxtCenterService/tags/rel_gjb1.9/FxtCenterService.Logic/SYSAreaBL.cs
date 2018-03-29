using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;

namespace FxtCenterService.Logic
{
    public class SYSAreaBL
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="areaid">指定区域ID集合</param>
        /// <returns></returns>
        public static List<SYSArea> GetSYSAreaList(SearchBase search, int[] areaid)
        {
            string areaidstr = (areaid == null || areaid.Length == 0) ? null : string.Join(",", areaid.Select(i => i.ToString()).ToArray());
            return SYSAreaDA.GetSYSAreaList(search, areaidstr);
        }
         /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="areaid">指定区域ID集合</param>
        /// <returns></returns>
        public static List<SYSCity> GetSYSCityList(int provinceid)
        {
            return SYSAreaDA.GetSYSCityList(provinceid);
        }
        /// <summary>
        /// 获取区域
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static SYSArea GetSYSArea( int areaId)
        {
            return SYSAreaDA.GetSYSAreaById(areaId);
        }
        /// 获取省份列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<SYSProvince> GetProvinceList()
        {
            return SYSAreaDA.GetProvinceList();
        }
    }
}
