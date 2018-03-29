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
    public class LNKPAppendageBL
    {
        public static int Add(LNKPAppendage model)
        {
            return LNKPAppendageDA.Add(model);

        }
        public static int Update(LNKPAppendage model)
        {
            return LNKPAppendageDA.Update(model);

        }
        /// <summary>
        /// 获取楼盘所有配套信息
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static List<LNKPAppendage> GetPAppendageByProjectId(int projectId, int cityId)
        {
            return LNKPAppendageDA.GetPAppendageByProjectId(projectId, cityId);
        }
    }
}
