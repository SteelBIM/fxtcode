using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class HearResourcesDAL : BaseManagement
    {
        BaseManagement manage = new BaseManagement();

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_HearResources> GetModuleList(string where)
        {
            IList<TB_HearResources> list = Search<TB_HearResources>(where);
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_UserHearResources> GetUserHearResourcesList(string where)
        {
            IList<TB_UserHearResources> list = Search<TB_UserHearResources>(where);
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TB_UserHearResources GetUserHearResources(string where)
        {
            TB_UserHearResources userHearResourcesInfo = SelectByCondition<TB_UserHearResources>(where);
            return userHearResourcesInfo;
        }

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public bool AddResources(TB_UserHearResources resources)
        {
            return Insert<TB_UserHearResources>(resources);
        }
    }
}
