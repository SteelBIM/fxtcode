using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;
using CAS.Entity.FxtDataCenter;

namespace FxtCenterService.Logic
{
    public class SYLBL
    {
        /// <summary>
        /// 获取内部收益率数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="areaid">指定区域ID集合</param>
        /// <returns></returns>
        public static List<SYLDat> GetSYLList(int cityId)
        {
            var result = DatSYLDA.GetSYLList(cityId);
            return result;
        }
    }
}
