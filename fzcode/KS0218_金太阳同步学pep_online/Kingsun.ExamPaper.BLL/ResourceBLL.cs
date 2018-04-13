using Kingsun.ExamPaper.DAL;
using Kingsun.ExamPaper.Model;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.BLL
{
    public class ResourceBLL
    {
        public QTb_Resource GetResource(int resid)
        {
            return new ResourceDAL().GetResource(resid);
        }
        public QTb_Resource GetResource(int mod_ed, int gradeid, int bookreel)
        {
            return new ResourceDAL().GetResource(mod_ed, gradeid, bookreel);
        }
        /// <summary>
        /// 分页查询资源包列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderColumn"></param>
        /// <param name="orderType"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public IList<V_Resource> GetResourcePageList(int pageIndex, int pageSize, string strWhere, string orderColumn, int orderType, out int totalCount, out int totalPages)
        {
            IList<V_Resource> list = new ResourceDAL().GetResourcePageList(pageIndex, pageSize, strWhere, orderColumn, orderType, out totalCount, out totalPages);
            return list == null ? (new List<V_Resource>()) : list;
        }
        /// <summary>
        /// 更新资源包
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool UpdateResource(QTb_Resource resource)
        {
            return new ResourceDAL().UpdateResource(resource);
        }
        /// <summary>
        /// 新增资源记录
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool InsertResource(QTb_Resource resource)
        {
            return new ResourceDAL().InsertResource(resource);
        }
    }
}
