using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class YXHearResourcesDAL : BaseManagement
    {
        BaseManagement manage = new BaseManagement();

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_HearResources_YX> GetModuleList(string where)
        {
            IList<TB_HearResources_YX> list = Search<TB_HearResources_YX>(where);
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_UserHearResources_YX> GetUserHearResourcesList(string where)
        {
            IList<TB_UserHearResources_YX> list = Search<TB_UserHearResources_YX>(where);
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TB_UserHearResources_YX GetUserHearResources(string where)
        {
            TB_UserHearResources_YX userHearResourcesInfo = SelectByCondition<TB_UserHearResources_YX>(where);
            return userHearResourcesInfo;
        }

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public bool AddResources(TB_UserHearResources_YX resources)
        {
            return Insert<TB_UserHearResources_YX>(resources);
        }
    }
}
