using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;

//创建人:曾智磊,日期:2014-06-26
namespace FxtCenterService.Logic
{
    public class CityTableBL
    {
        /// <summary>
        /// 根据城市获取表信息
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static CityTable GetCityTable(int cityid)
        {
            return CityTableDA.GetCityTable(cityid);
        }
    }
}
