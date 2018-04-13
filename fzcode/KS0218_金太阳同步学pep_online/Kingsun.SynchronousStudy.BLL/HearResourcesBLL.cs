using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class HearResourcesBLL
    {
        HearResourcesDAL hearResourcesDAL = new HearResourcesDAL();
        /// <summary>
        /// 条件查询
        /// </summary>    
        public IList<TB_HearResources> GetModuleList(string where)
        {
            return hearResourcesDAL.GetModuleList(where);
        }

        /// <summary>
        /// 条件查询
        /// </summary>    
        public IList<TB_UserHearResources> GetUserHearResourcesList(string where)
        {
            return hearResourcesDAL.GetUserHearResourcesList(where);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TB_UserHearResources GetUserHearResources(string where)
        {
            return hearResourcesDAL.GetUserHearResources(where);
        }

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public bool AddResources(TB_UserHearResources resources)
        {
            if (hearResourcesDAL.AddResources(resources))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
