using CAS.Common;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.Logic
{
    /// <summary>
    /// 复估业务
    /// </summary>
    public class ReAutoPriceBL
    {
        /// <summary>
        /// 复估楼盘列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<DATProject> GetProjectList(SearchBase search)
        {
            return DatProjectDA.GetProjectList(search);
        }
    }
}
