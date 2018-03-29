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
    public class SYSProvinceBL
    {
        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<SYSProvince> GetSYSProvinceList(SearchBase search)
        {
            return SYSProvinceDA.GetSYSProvinceList(search);
        }
    }
}
